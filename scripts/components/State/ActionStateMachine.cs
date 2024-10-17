namespace Incorgnito.scripts.components.state;

using Godot;
using System;

using System.Collections.Generic;

public partial class ActionStateMachine : Node
{
	[Export] private AbstractActionState _initialActionState;
	public readonly Dictionary <String, AbstractActionState> StateDict = new Dictionary<String, AbstractActionState>();
	public AbstractActionState CurrentActionState;
	public StateSignals TransitionStateSignal;
    
	public override void _Ready()
	{
		TransitionStateSignal = GetNode<StateSignals>("/root/StateSignals");
		foreach (var child in this.GetChildren())
		{
			if (child is AbstractActionState)
			{
				if (child.Name is not null)
				{
					GD.Print((string)child.Name);
					StateDict.Add(child.Name, (AbstractActionState)child);
				}
		
					
                
				TransitionStateSignal.TransitionState += OnStateTransition;
			}
		}

		if (_initialActionState is not null)
		{
			_initialActionState.Enter();
			CurrentActionState = _initialActionState;
		}
	}
    
	public override void _PhysicsProcess(double delta)
	{
		if (CurrentActionState is not null)
		{
			CurrentActionState.UpdatePhysicsProcess(delta); 
		}
	}
	public override void _Process(double delta)
	{
		if (CurrentActionState is not null)
		{
			CurrentActionState.UpdateProcess(delta); 
		}
	}

	private void OnStateTransition(AbstractActionState abstractActionState, string stateName)
	{
		if (abstractActionState != CurrentActionState)
		{
			return;
		}

		var newState = StateDict[stateName];
		if (newState is null)
			return;

		if (CurrentActionState is not null)
		{
			CurrentActionState.Exit();
		}
        
		newState.Enter();
		CurrentActionState = newState;

	}
}
