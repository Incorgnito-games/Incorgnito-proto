namespace Incorgnito.scripts.components.state;

using Godot;
using System;

using System.Collections.Generic;

public partial class ActionStateMachine : Node
{
	[Export] private AbstractActionState _initialActionState;
	public readonly Dictionary <String, AbstractActionState> StateDict = new Dictionary<String, AbstractActionState>();
	private AbstractActionState _currentActionState;
	private StateSignals _transitionStateSignal;
    
	public override void _Ready()
	{
		_transitionStateSignal = GetNode<StateSignals>("/root/StateSignals");
		foreach (var child in this.GetChildren())
		{
			if (child is AbstractActionState)
			{
				if (child.Name is not null)
					StateDict.Add(((string)child.Name).ToLower(), (AbstractActionState)child);
                
				_transitionStateSignal.TransitionState += OnStateTransition;
			}
		}

		if (_initialActionState is not null)
		{
			_initialActionState.Enter();
			_currentActionState = _initialActionState;
		}
	}
    
	public override void _PhysicsProcess(double delta)
	{
		if (_currentActionState is not null)
		{
			_currentActionState.UpdatePhysicsProcess(delta); 
		}
	}

	private void OnStateTransition(AbstractActionState abstractActionState, string stateName)
	{
		if (abstractActionState != _currentActionState)
		{
			return;
		}

		var newState = StateDict[stateName.ToLower()];
		if (newState is null)
			return;

		if (_currentActionState is not null)
		{
			_currentActionState.Exit();
		}
        
		newState.Enter();
		_currentActionState = newState;

	}
}
