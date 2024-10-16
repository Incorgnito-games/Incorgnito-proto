namespace Incorgnito.scripts.components.state;

using Godot;
public abstract partial class State : Node, IState
{
    [Signal]
    public delegate void OnChildTransitionEventHandler(State state, string newStateName);

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