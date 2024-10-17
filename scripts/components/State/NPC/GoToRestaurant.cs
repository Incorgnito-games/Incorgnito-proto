using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.components.state.npc;
using Godot;
public partial class GoToRestaurant: AbstractActionState
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
    }

    public override void UpdateProcess(double delta)
    {
        GD.Print("Need to go to restaurant");
    }

    public override void UpdatePhysicsProcess(double delta)
    {
    }
    public override string ToString()
    {
        return "GoToRestaurant";
    }
}