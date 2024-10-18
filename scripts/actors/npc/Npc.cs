using System;
using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.actors.npc;

using System.Collections.Generic;
using Godot;
using components.UtilityAI;
using System.Collections.Generic;

public partial class Npc : CharacterBody3D

{
	//actor traits
	public List<UtilityTrait> Traits;
	private Timer _coolDownTimer = new Timer();
	
	[Export] private float _startingHunger = .8f;
	[Export] private float _startingSocial = .5f;
	[Export] private float _startingEnergy = .8f;
	
	[Export] private float _hungerImportance = 0.7f;
	[Export] private float _socialImportance = 0.6f;
	[Export] private float _restImportance = 0.3f;
	
	[Export] private float _hungerDrain = 0.005f;
	[Export] private float _socialDrain =0.006f;
	[Export] private float _energyDrain=0.0005f;
	
	private int _hungerMultiplier = 1;
	private int _restMultiplier = 1;
	private int _socialMultiplier = 1;
	
	//character 
	[Export] public float Speed{ get; set; } = 0.5f;
	
	//movement
	private Vector3 velocity;
	
	//signals
	[Signal]
	public delegate void UpdateNpcTraitStatsEventHandler(float hunger, float rest, float social);
	
	//debug
	public string DebugMessage;
	
	//enviromental knowledge
	[Export] public Node BuildingMap;
	public Dictionary<string, Vector3> BuildingLocations;
	
	//state
	private ActionStateMachine _stateMachine;
	private bool _onCoolDown = false;

	public override void _Ready()
	{
		BuildingLocations = new Dictionary<string, Vector3>();
		_stateMachine = GetNode<ActionStateMachine>("ActionStateMachine");
			
		_coolDownTimer.SetAutostart(false);
		_coolDownTimer.OneShot = true;
		_coolDownTimer.WaitTime = 3;
		_coolDownTimer.Timeout += OnCoolDownTimerTimeoutSignal;
		
		var eatAction = new UtilityAction("Eat", _hungerImportance, _stateMachine.StateDict["GoToRestaurant"]);
		var restAction = new UtilityAction("Rest", _restImportance, _stateMachine.StateDict["GoToHome"]);
		var socializeAction = new UtilityAction("Socialize", _socialImportance, _stateMachine.StateDict["GoToBar"]);

		Traits = new List<UtilityTrait>
		{
			new UtilityTrait("Hunger", _startingHunger, eatAction),
			new UtilityTrait("Tiredness", _startingEnergy, restAction),
			new UtilityTrait("Loneliness", _startingSocial, socializeAction)
		};

		foreach (var building in BuildingMap.GetChildren())
		{
			GD.Print(building.ToString());
			BuildingLocations.Add(building.ToString(), ((StaticBody3D)building).GlobalPosition);
		}
		
		
	}

	public override void _Process(double delta)
	{
		PreformUtilityAiAction();
		//add timers or mechanic to reduce values--> naive for now
		GD.Print(Velocity);
		
		if (!_onCoolDown)
		{
			Traits[0].Value = (Traits[0].Value > 0 && Traits[0].Value < 1) ? Traits[0].Value += _hungerDrain* (float)delta * _hungerMultiplier: Traits[0].Value ; //hunger
			Traits[1].Value = (Traits[1].Value > 0 && Traits[1].Value < 1) ? Traits[1].Value += _energyDrain * (float)delta *_restMultiplier: Traits[1].Value ; //rest
			Traits[2].Value = (Traits[2].Value > 0 && Traits[2].Value < 1) ? Traits[2].Value += _socialDrain * (float)delta *_socialMultiplier: Traits[2].Value ; //social
		}
		
		EmitSignal(SignalName.UpdateNpcTraitStats, Traits[0].Value,Traits[1].Value,Traits[2].Value);
}
	
	public void PreformUtilityAiAction()
	{
		UtilityAction bestAction = null;
		float highestUtility = -1;
		
		
		foreach (var trait in Traits)
		{
			//should this be normailized
			float utility = trait.EvaluateUtility();
			
			if (utility > highestUtility)
			{ 
				highestUtility = utility; 
				bestAction = trait.AssociatedAction;
			}
		}

		if (bestAction != null)
		{
			_stateMachine.StateSignals.EmitSignal(nameof(StateSignals.TransitionState), _stateMachine.CurrentActionState, bestAction.State.ToString());
		}
		else
		{
			GD.Print($"{Name} has no action to perform.");
		}
	}

	public void OnBarEntered(Node3D body)
	{
		GD.Print("Ah Water for my Horses!");
		_socialMultiplier = -2;
	}
	public void OnBarExited(Node3D body)
	{
		GD.Print("12 dollars a pint! pfff...");
		_socialMultiplier = 1;
	}

	public void OnRestaurantEntered(Node3D body)
	{
		GD.Print("I could eat a horse!");
		_hungerMultiplier = -4;

	}
	public void OnRestaurantExited(Node3D body)
	{
		GD.Print("oh god i think i ate a horse!");
		_hungerMultiplier = 1;

	}

	public void OnHomeEntered(Node3D body)
	{
		GD.Print("Honey im Home!");
		_restMultiplier = -5;
	}
	public void OnHomeExited(Node3D body)
	{
		GD.Print("Astalavista Baby!");
		_restMultiplier = 1;
	}

	public void OnWorkEntered(Node3D body)
	{
		GD.Print("Ahh fuck..");
		_restMultiplier = -2;
	}
	public void OnWorkExited(Node3D body)
	{
		GD.Print("Freedom!");
		_restMultiplier = 1;
	}
	public void OnCoolDownTimerTimeoutSignal()
	{
		_onCoolDown = false;

	}
}
