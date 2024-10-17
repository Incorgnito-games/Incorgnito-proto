using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.components.state.npc;
using Godot;
public partial class GoToHome: AbstractActionState
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
    }

    public override void UpdateProcess(double delta)
    {
        GD.Print("Need to go to home");
    }

    public override void UpdatePhysicsProcess(double delta)
    {
    }
    public override string ToString()
    {
        return "GoToHome";
    }
}