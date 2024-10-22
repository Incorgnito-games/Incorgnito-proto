namespace Incorgnito.scripts;

using Godot;
using Godot.Collections;
public partial class CustomSignals: Node
{
    [Signal]
    public delegate void DebugMessageEventHandler(string debugMessage);

    [Signal]
    public delegate void DebugStatsDisplayEventHandler(Dictionary<string, float> needsDictionary);

}