namespace Incorgnito.scripts.components.state;

using Godot;
public abstract partial class AbstractActionState : Node, IState
{
    protected StateSignals StateTransitionSignal;

    public override void _Ready()
    {
        StateTransitionSignal = GetNode<StateSignals>("/root/StateSignals");
    }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateProcess(double delta);
    public abstract void UpdatePhysicsProcess(double delta);
}