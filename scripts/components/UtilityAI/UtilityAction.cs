namespace Incorgnito.scripts.components.UtilityAI;

using Godot;

public class UtilityAction
{
    public string Name { get; set; }     
    public float Importance { get; set; } 

    public UtilityAction(string name, float importance)
    {
        Name = name;
        Importance = importance;
    }

    public void Execute(string npcName)
    {
        GD.Print($"{npcName} will: {Name}");
        //state change etc
    }
}