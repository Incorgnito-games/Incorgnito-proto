namespace Incorgnito.scripts.components.state;

using Godot;

public partial class StateSignals : Node
{
    [Signal]
    public delegate void TransitionStateEventHandler(State state, string stateName);


}