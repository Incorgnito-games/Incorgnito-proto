using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.actors.npc;

using System.Collections.Generic;
using Godot;
using components.UtilityAI;
using System.Collections.Generic;

public partial class Npc : CharacterBody3D
{
	private int _hungerMultiplier = 1;
	private int _restMultiplier = 1;
	private int _socialMultiplier = 1;

	[Signal]
	public delegate void UpdateNpcTraitStatsEventHandler(float hunger, float rest, float social);
	public string DebugMessage;
	private ActionStateMachine _stateMachine;
	public Dictionary<string, Vector3> BuildingLocations;
	[Export] public Node BuildingMap;
	[Export]
	public float Speed{
		get;
		set;
	} = 0.5f;
	public const float JumpVelocity = 4.5f;
	public List<UtilityTrait> Traits;

	public override void _Ready()
	{
		BuildingLocations = new Dictionary<string, Vector3>();
		_stateMachine = GetNode<ActionStateMachine>("ActionStateMachine");
		GD.Print(_stateMachine.StateDict.Count.ToString());
		var eatAction = new UtilityAction("Eat", 1.0f, _stateMachine.StateDict["GoToRestaurant"]);
		var restAction = new UtilityAction("Rest", 0.8f, _stateMachine.StateDict["GoToHome"]);
		var socializeAction = new UtilityAction("Socialize", 0.6f, _stateMachine.StateDict["GoToBar"]);

		Traits = new List<UtilityTrait>
		{
			new UtilityTrait("Hunger", 80, eatAction),
			new UtilityTrait("Tiredness", 60, restAction),
			new UtilityTrait("Loneliness", 50, socializeAction)
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
		
		Traits[0].Value = (Traits[0].Value > 0 && Traits[0].Value < 100) ? Traits[0].Value -= 0.05f * _hungerMultiplier: Traits[0].Value ; //hunger
		Traits[1].Value = (Traits[1].Value > 0 && Traits[1].Value < 100) ? Traits[1].Value -= 0.08f * _restMultiplier: Traits[1].Value ; //rest
		Traits[2].Value = (Traits[2].Value > 0 && Traits[2].Value < 100) ? Traits[2].Value -= 0.005f * _socialMultiplier: Traits[2].Value ; //social
		EmitSignal(SignalName.UpdateNpcTraitStats, Traits[0].Value,Traits[1].Value,Traits[2].Value);
		// GD.Print($"{Traits[0].Name} == > {Traits[0].Value}");
		// GD.Print($"{Traits[1].Name} == > {Traits[1].Value}");
		// GD.Print($"{Traits[2].Name} == > {Traits[2].Value}");
}
	
	public void PreformUtilityAiAction()
	{
		UtilityAction bestAction = null;
		float highestUtility = -1;
		
		
		foreach (var trait in Traits)
		{
			float utility = trait.EvaluateUtility();
            
			if (utility > highestUtility)
			{
				highestUtility = utility;
				bestAction = trait.AssociatedAction;
			}
		}

		if (bestAction != null)
		{
			//send state signal
			_stateMachine.StateSignals.EmitSignal(nameof(StateSignals.TransitionState), _stateMachine.CurrentActionState, bestAction.State.ToString());
			// bestAction.State;
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
		GD.Print("Ah Water for my Horses!");
		_socialMultiplier = 1;
	}

	public void OnRestaurantEntered(Node3D body)
	{
		GD.Print("I could eat a horse!");
		_hungerMultiplier = -3;

	}
	public void OnRestaurantExited(Node3D body)
	{
		GD.Print("I could eat a horse!");
		_hungerMultiplier = 1;

	}

	public void OnHomeEntered(Node3D body)
	{
		GD.Print("Honey im Home!");
		_restMultiplier = -2;
	}
	public void OnHomeExited(Node3D body)
	{
		GD.Print("Honey im Home!");
		_restMultiplier = 1;
	}

	public void OnWorkEntered()
	{
		GD.Print("Ahh fuck..");
		_restMultiplier = -2;
	}
	public void OnWorkExited()
	{
		GD.Print("Ahh fuck..");
		_restMultiplier = 1;
	}
}
