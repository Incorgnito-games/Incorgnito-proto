namespace Incorgnito.scripts.actors.npc;

using System.Collections.Generic;
using Godot;
using components.UtilityAI;

public partial class Npc : CharacterBody3D
{
	[Export]
	public float Speed{
		get;
		set;
	} = 0.5f;
	public const float JumpVelocity = 4.5f;
	public List<UtilityTrait> Traits;

	public override void _Ready()
	{
		var eatAction = new UtilityAction("Eat", 1.0f);
		var restAction = new UtilityAction("Rest", 0.8f);
		var socializeAction = new UtilityAction("Socialize", 0.6f);

		Traits = new List<UtilityTrait>
		{
			new UtilityTrait("Hunger", 80, eatAction),
			new UtilityTrait("Tiredness", 30, restAction),
			new UtilityTrait("Loneliness", 50, socializeAction)
		};
	}

	public override void _Process(double delta)
	{
		PreformUtilityAiAction();
		Traits[0].Value -= 0.1f;
		GD.Print($"{Traits[0].Name} == > {Traits[0].Value}");
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
			bestAction.Execute(Name);
		}
		else
		{
			GD.Print($"{Name} has no action to perform.");
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// Vector3 velocity = Velocity;
		//
		// // Add the gravity.
		// if (!IsOnFloor())
		// {
		// 	velocity += GetGravity() * (float)delta;
		// }
		//
		// // Handle Jump.
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// {
		// 	velocity.Y = JumpVelocity;
		// }
		//
		// // Get the input direction and handle the movement/deceleration.
		// // As good practice, you should replace UI actions with custom gameplay actions.
		// Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		// Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		// if (direction != Vector3.Zero)
		// {
		// 	velocity.X = direction.X * Speed;
		// 	velocity.Z = direction.Z * Speed;
		// }
		// else
		// {
		// 	velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		// 	velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		// }
		//
		// Velocity = velocity;
		// MoveAndSlide();
	}
}
