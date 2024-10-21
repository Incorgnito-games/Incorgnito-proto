namespace Incorgnito.scripts;
using Godot;
public partial class CustomSignals: Node
{
    [Signal]
    public delegate void DebugMessageEventHandler(string debugMessage); 
}