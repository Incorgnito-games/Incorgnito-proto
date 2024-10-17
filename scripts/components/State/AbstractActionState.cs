namespace Incorgnito.scripts.components.state;

using Godot;
using actors.npc;
public abstract partial class AbstractActionState : Node, IState
{
    protected StateSignals StateTransitionSignal;
    [Export] protected Npc Npc;

    public override void _Ready()
    {
        StateTransitionSignal = GetNode<StateSignals>("/root/StateSignals");
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateProcess(double delta);
    public abstract void UpdatePhysicsProcess(double delta);
    
}