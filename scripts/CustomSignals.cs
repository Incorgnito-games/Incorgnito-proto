namespace Incorgnito.scripts;

using Godot;
using Godot.Collections;
using ui;
public partial class CustomSignals: Node
{
    [Signal]
    public delegate void DebugMessageEventHandler(string debugMessage, string npcStringS);

    [Signal]
    public delegate void DebugStatsDisplayEventHandler(Dictionary<string, float> needsDictionary, string npcString);

}