using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.components.UtilityAI;

using Godot;

public class UtilityAction
{
    public string Name { get; set; }     
    public float Importance { get; set; }
    public AbstractActionState State;


    public UtilityAction(string name, float importance, AbstractActionState state)
    {
        Name = name;
        Importance = importance; 
        State = state;
    }

    public void Execute(string npcName)
    {
        
        GD.Print($"{npcName} will: {Name}");
        //send state change signal
        // _stateTransitionSignal.EmitSignal(nameof(StateSignals.TransitionState), this, "idle");
    }
}