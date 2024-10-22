using Godot.Collections;
using Incorgnito.scripts.ui;

namespace Incorgnito.scripts.actors.ai;

using Godot;
using Incorgnito.scripts.Systems;
public enum EStat
{
	Hunger = 0,
	Energy = 1,
	Social = 2,
	Money = 3
}
public abstract partial class BaseAi : Node
{
	[ExportGroup("Initial Need Values")]
	[Export] private float _initialHunger = 0.5f;
	[Export] private float _initialEnergy = 0.5f;
	[Export] private float _initialSocial = 0.5f;
	[Export] private float _initialMoney = 1f;

	[ExportGroup("Need Decay Rates")]
	[Export] private float _baseHungerDecay = 0.005f;
	[Export] private float _baseEnergyDecay = 0.005f;
	[Export] private float _baseSocialDecay = 0.005f;
	[Export] private float _baseMoneyDecay = 0f;
    
	public float CurrentHunger { get; protected set; }
	public float CurrentEnergy { get; protected set; }
	public float CurrentSocial { get; protected set; }
	public float CurrentMoney { get; protected set; }
	
	[ExportGroup("Character Setup")]	
	[Export] protected float Speed = 5.0f;
	[Export] private float _interactionDelay = 3f;
	
	[ExportGroup("Dependencies")]
	[Export] protected CharacterBody3D Npc;
	[Export] protected NavigationAgent3D NavAgent;

	protected Dictionary<string, float> NeedsDictionary;
	private CustomSignals _debugSignal;
	private Vector3 _newVelocity;
	private bool _npcFreeOnTimer = true;
	private bool _isMoving = false;
	protected SmartObject SelectedObject;
	private bool _isPreforming = false;


	protected BaseInteraction CurrentInteraction;

	private float _timeUntilNextInteraction = -1f;

	public override void _Ready()
	{
		NeedsDictionary = new Dictionary<string, float>();
		CurrentHunger = _initialHunger;
		CurrentEnergy = _initialEnergy;
		CurrentSocial = _initialSocial;
		CurrentMoney = _initialMoney;
		NeedsDictionary.Add("hunger", CurrentHunger);
		NeedsDictionary.Add("energy", CurrentEnergy);
		NeedsDictionary.Add("social", CurrentSocial);
		NeedsDictionary.Add("money", CurrentMoney);

		_newVelocity = new Vector3();
		_debugSignal = GetNode<CustomSignals>("/root/CustomSignals");
		
		
		CurrentInteraction = null;
		SelectedObject = null;
		
		SetPhysicsProcess(false);
		CallDeferred(nameof(GetNavMap));
	}
	public async void GetNavMap()
	{
		await ToSignal(GetTree(), "physics_frame");
		SetPhysicsProcess(true);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (CurrentInteraction != null)
		{
			if (_isPreforming)
			{
				return;
			}
			if (!NavAgent.IsTargetReached())
			{
				MoveToTarget();
				return;
			}
			
			_isPreforming = true;
			// _debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage),
			// 	new string($"preforming: {CurrentInteraction.DisplayName} -- {CurrentInteraction.InteractionDuraction} seconds"));
			CurrentInteraction.Perform(this, OnInteractionFinished); 
			
		}
		else
		{
			_timeUntilNextInteraction -= (float)delta;
			if (_timeUntilNextInteraction > 0)
				return;
			
			_timeUntilNextInteraction = _interactionDelay; 
			PickInteraction();
		}
		
	}

	public override void _Process(double delta)
	{
		CurrentHunger = Mathf.Clamp(CurrentHunger - _baseHungerDecay * (float)delta, 0,1);
		CurrentEnergy = Mathf.Clamp(CurrentEnergy - _baseEnergyDecay * (float)delta, 0, 1);
		CurrentSocial = Mathf.Clamp(CurrentSocial - _baseSocialDecay * (float)delta, 0, 1);
		CurrentMoney = Mathf.Clamp(CurrentMoney - _baseMoneyDecay * (float)delta, 0, 1);
        
		NeedsDictionary["hunger"] = CurrentHunger;
		NeedsDictionary["energy"] = CurrentEnergy;
		NeedsDictionary["social"] = CurrentSocial;
		NeedsDictionary["money"] = CurrentMoney;
		
		 _debugSignal.EmitSignal(nameof(_debugSignal.DebugStatsDisplay),NeedsDictionary);
	}

	protected abstract void PickInteraction();
	
	protected virtual void OnInteractionFinished(BaseInteraction interaction)
	{
		interaction.UnLockInteraction();
		
		_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage), 
			new string($"finsihed interaction: {interaction.DisplayName} waiting for {_interactionDelay} seconds"));
		CurrentInteraction = null;
		
		SelectedObject = null;
		_isPreforming = false;
	}

	public void UpdateIndividualStat(EStat target, float amount)
	{
		GD.Print($"Update {target} by {amount}");
		switch (target)
		{
			case EStat.Hunger: CurrentHunger += amount;
				break;
			case EStat.Energy: CurrentEnergy += amount;
				break;
			case EStat.Social: CurrentSocial += amount;
				break;
			case EStat.Money: CurrentMoney += amount;
				break;
			default:
				GD.PrintErr("unknown stat fall through");
				break;
		}
	}
	
	

	private void MoveToTarget()
	{
		//move npc
		_newVelocity =(NavAgent.GetNextPathPosition() - Npc.GlobalPosition).Normalized();
		
		Npc.Velocity = _newVelocity * Speed;
		if (!Npc.MoveAndSlide())
		{
			GD.PrintErr($"Hit something on the way to {SelectedObject.DisplayName}");
		}
		_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage), 
				new string($"going to {CurrentInteraction.DisplayName} at {SelectedObject.DisplayName}"));
	}
	
}