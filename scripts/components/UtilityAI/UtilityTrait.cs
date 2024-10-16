namespace Incorgnito.scripts.components.UtilityAI;

using System.Collections.Generic;

// Trait class to represent NPC traits and associated actions
public class UtilityTrait
{
    public string Name { get; set; }     
    public float Value { get; set; }    
    public UtilityAction AssociatedAction { get; set; } 

    public UtilityTrait(string name, float value, UtilityAction associatedAction)
    {
        Name = name;
        Value = value;
        AssociatedAction = associatedAction;
    }

    public float EvaluateUtility()
    {
        return Value * AssociatedAction.Importance;
    }
}