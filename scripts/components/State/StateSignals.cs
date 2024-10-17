namespace Incorgnito.scripts.components.state;

using Godot;

public partial class StateSignals : Node
{
    [Signal]
    public delegate void TransitionStateEventHandler(AbstractActionState abstractActionState, string stateName);
    [Signal]
    public delegate void StateDebugMessageEventHandler(AbstractActionState abstractActionState, string debugMessage);



}