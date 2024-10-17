using Godot;
using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.components.state.npc;

public partial class GoToWork: AbstractActionState
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
    }

    public override void UpdateProcess(double delta)
    {
        StateSignals.EmitSignal(nameof(StateSignals.StateDebugMessage),this, "Need to go to work");
        // GD.Print("Need to go to work");
    }

    public override void UpdatePhysicsProcess(double delta)
    {
    }
    public override string ToString()
    {
        return "GoToWork";
    }
}