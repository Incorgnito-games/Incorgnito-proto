using Incorgnito.scripts.ui;

namespace Incorgnito.scripts.actors;

using Godot;
using Incorgnito.scripts.Systems;

public partial class SimpleAi : Node
{

	[Export] protected float Speed = 5.0f;
	[Export] protected CharacterBody3D Npc;
	[Export] protected NavigationAgent3D NavAgent;
	[Export] protected float InteractionDelay = 3f;

	private CustomSignals _debugSignal;
	
	private bool _npcFreeOnTimer = true;
	private bool _isMoving = false;
	private SmartObject _selectedObject;
	private bool _isPreforming = false;


	private BaseInteraction _currentInteraction;

	private float _timeUntilNextInteraction = -1f;

	public override void _Ready()
	{
		_debugSignal = GetNode<CustomSignals>("/root/CustomSignals");
		_currentInteraction = null;
		_selectedObject = null;
		
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
		if (_currentInteraction != null)
		{
			if (!_isPreforming)
			{
				if (!NavAgent.IsTargetReached())
				{
					MoveToTarget();
				}

				else
				{
					_isPreforming = true;
					_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage),
						new string($"preforming: {_currentInteraction.DisplayName} -- {_currentInteraction.InteractionDuraction} seconds"));
					 GD.Print($"preforming: {_currentInteraction.DisplayName} -- {_currentInteraction.InteractionDuraction} seconds");
					_currentInteraction.Perform(this, OnInteractionFinished);
				}
			}
		}
		else
		{
			_timeUntilNextInteraction -= (float)delta;
			if (_timeUntilNextInteraction <= 0)
			{
				
			//	GD.Print("picking interaction/object");
				_timeUntilNextInteraction = InteractionDelay;
				PickRandomInteraction();
				
			}
		}
	}
	
	public void OnInteractionFinished(BaseInteraction interaction)
	{
		interaction.UnLockInteraction();
		
		_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage), 
			new string($"finsihed interaction: {interaction.DisplayName} waiting for {InteractionDelay} seconds"));
		// GD.Print($"finsihed interaction: {interaction.DisplayName} waiting for {_interactionDelay} seconds");
		_currentInteraction = null;
		
		_selectedObject = null;
		_isPreforming = false;
	}

	public void PickRandomInteraction()
	{

		_selectedObject = SmartObjectManager.Instance.RegisteredObjects[
			GD.RandRange(0, SmartObjectManager.Instance.RegisteredObjects.Count - 1)];
		NavAgent.TargetDesiredDistance = _selectedObject.TargetDistanceTolerance;
		NavAgent.SetTargetPosition(_selectedObject.ObjectBody.GlobalPosition);
		GD.Print(_selectedObject.Interactions.Count);
		var selectedInteraction = _selectedObject.Interactions[0];

		if (selectedInteraction.CanPerform())
		{
		//	GD.Print("interaction preformable");
			_currentInteraction = selectedInteraction;
			_currentInteraction.LockInteraction();
		}
		else
		{
		//	GD.Print("interaction not preformable");
		}
	}

	public void MoveToTarget()
	{
		//move npc
		var dir = new Vector3();
		var velocity = Npc.Velocity;
		//GD.Print($"new target location: {NavAgent.TargetPosition}");
		dir = (NavAgent.GetNextPathPosition() - Npc.GlobalPosition).Normalized();
	
		Npc.Velocity = dir * Speed;
		if (!Npc.MoveAndSlide())
		{
			GD.PrintErr($"Could not reach {_selectedObject.DisplayName}");
			//_currentInteraction = null;
		}
		else
		{
			//GD.Print($"{(Npc.GlobalPosition - _selectedObject.ObjectBody.GlobalPosition).Length()}");
			
			_debugSignal.EmitSignal(nameof(_debugSignal.DebugMessage), 
				new string($"going to {_currentInteraction.DisplayName} at {_selectedObject.DisplayName}"));
			// GD.Print($"going to {_currentInteraction.DisplayName} at {_selectedObject.DisplayName}");
		}
	}
}