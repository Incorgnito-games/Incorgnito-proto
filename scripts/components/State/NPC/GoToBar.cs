using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.components.state.npc;
using Godot;
public partial class GoToBar: AbstractActionState
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
    }

    public override void UpdateProcess(double delta)
    {
        StateSignals.EmitSignal(nameof(StateSignals.StateDebugMessage),this,"Need to go to the bar");
        // GD.Print("Need to go to the Bar");
    }

    public override void UpdatePhysicsProcess(double delta)
    {
        var dir = new Vector3();
        var velocity = Npc.Velocity;
        // GD.Print(Npc.BuildingLocations["restaurant"]);
        NavAgent.TargetPosition = Npc.BuildingLocations["bar"];
        dir = (NavAgent.GetNextPathPosition() - Npc.GlobalPosition).Normalized();

        Npc.Velocity = dir * Npc.Speed;
        Npc.MoveAndSlide();
    }
    public override string ToString()
    {
        return "GoToBar";
    }
}