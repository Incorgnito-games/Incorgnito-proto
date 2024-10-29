using System;
using Godot.Collections;
using Incorgnito.scripts.actors.npc;
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
	
	[Export] private float _interactionDelay = 3f;
	
	[ExportGroup("Dependencies")]
	[Export] protected NpcCharacter Npc;
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
		
		NeedsDictionary.Add("hunger", Npc.CurrentHunger);
		NeedsDictionary.Add("energy", Npc.CurrentEnergy);
		NeedsDictionary.Add("social", Npc.CurrentSocial);
		NeedsDictionary.Add("money", Npc.CurrentMoney);

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
	
        
		NeedsDictionary["hunger"] = Npc.CurrentHunger;
		NeedsDictionary["energy"] = Npc.CurrentEnergy;
		NeedsDictionary["social"] = Npc.CurrentSocial;
		NeedsDictionary["money"] = Npc.CurrentMoney;

	
		 _debugSignal.EmitSignal(nameof(_debugSignal.DebugStatsDisplay),NeedsDictionary, Npc.CharacterFirstName);
	}

	protected abstract void PickInteraction();
	
	protected virtual void OnInteractionFinished(BaseInteraction interaction)
	{
		interaction.UnLockInteraction();
		
		_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage), 
			new string($"finsihed interaction: {interaction.DisplayName} waiting for {_interactionDelay} seconds"),
			Npc.CharacterFirstName);
		CurrentInteraction = null;
		
		SelectedObject = null;
		_isPreforming = false;
	}

	public void UpdateIndividualStat(EStat target, float amount)
	{
		// GD.Print($"Update {target} by {amount}");
		switch (target)
		{
			case EStat.Hunger: Npc.CurrentHunger += amount;
				break;
			case EStat.Energy: Npc.CurrentEnergy += amount;
				break;
			case EStat.Social: Npc.CurrentSocial += amount;
				break;
			case EStat.Money: Npc.CurrentMoney += amount;
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
		
		Npc.Velocity = _newVelocity * Npc.Speed;
		if (!Npc.MoveAndSlide())
		{
			GD.PrintErr($"{Npc.CharacterFirstName} Hit something on the way to {SelectedObject.DisplayName}");
		}

		if (Npc.CharacterFirstName == "steve")
		{
			GD.Print($"{Npc.CharacterFirstName} -- Global Pos:{Npc.GlobalPosition}   ---   Target: {NavAgent.TargetPosition}   ---   Target Distance {NavAgent.DistanceToTarget()}");
		}
		_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage), 
				new string($"going to {CurrentInteraction.DisplayName} at {SelectedObject.DisplayName}"),
				Npc.CharacterFirstName);
	}
	
}