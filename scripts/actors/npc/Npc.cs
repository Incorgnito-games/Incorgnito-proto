using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.actors.npc;

using System.Collections.Generic;
using Godot;
using components.UtilityAI;
using System.Collections.Generic;

public partial class Npc : CharacterBody3D
{

	[Signal]
	public delegate void UpdateNpcTraitStatsEventHandler(float hunger, float rest, float social);
	
	private ActionStateMachine _stateMachine;
	private Dictionary<string, Vector3> BuildingLocations;
	[Export] public CsgCombiner3D BuildingMap;
	[Export]
	public float Speed{
		get;
		set;
	} = 0.5f;
	public const float JumpVelocity = 4.5f;
	public List<UtilityTrait> Traits;

	public override void _Ready()
	{
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

		foreach (CsgBox3D building in BuildingMap.GetChildren())
		{
			//GD.Print(building.Name);
			//BuildingLocations.Add(building.ToString(), building.GlobalTransform.Origin);
		}
		
	}

	public override void _Process(double delta)
	{
		PreformUtilityAiAction();
		//add timers or mechanic to reduce values
		Traits[0].Value -= 0.1f;
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
			_stateMachine.TransitionStateSignal.EmitSignal(nameof(StateSignals.TransitionState), _stateMachine.CurrentActionState, bestAction.State.ToString());
			// bestAction.State;
		}
		else
		{
			GD.Print($"{Name} has no action to perform.");
		}
	}
}
