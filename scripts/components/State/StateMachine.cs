namespace Incorgnito.scripts.components.state;

using Godot;
using System;

using System.Collections.Generic;

public partial class StateMachine : Node
{
	[Export] private State _initialState;
	public readonly Dictionary <String, State> StateDict = new Dictionary<String, State>();
	private State _currentState;
	private StateSignals _transitionStateSignal;
    
	public override void _Ready()
	{
		_transitionStateSignal = GetNode<StateSignals>("/root/StateSignals");
		foreach (var child in this.GetChildren())
		{
			if (child is State)
			{
				if (child.Name is not null)
					StateDict.Add(((string)child.Name).ToLower(), (State)child);
                
				_transitionStateSignal.TransitionState += OnStateTransition;
			}
		}

		if (_initialState is not null)
		{
			_initialState.Enter();
			_currentState = _initialState;
		}
	}
    
	public override void _PhysicsProcess(double delta)
	{
		if (_currentState is not null)
		{
			_currentState.UpdatePhysicsProcess(delta); 
		}
	}

	private void OnStateTransition(State state, string stateName)
	{
		if (state != _currentState)
		{
			return;
		}

		var newState = StateDict[stateName.ToLower()];
		if (newState is null)
			return;

		if (_currentState is not null)
		{
			_currentState.Exit();
		}
        
		newState.Enter();
		_currentState = newState;

	}
}
