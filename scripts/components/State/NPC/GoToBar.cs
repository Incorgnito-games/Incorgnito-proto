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
        GD.Print("Need to go to the Bar");
    }

    public override void UpdatePhysicsProcess(double delta)
    {
    }
    public override string ToString()
    {
        return "GoToBar";
    }
}