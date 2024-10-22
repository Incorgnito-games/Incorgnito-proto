using System.Collections.Generic;
using System.Data;
using Godot.Collections;

namespace Incorgnito.scripts.Systems;

using System;

using actors.ai;
using Godot;

public enum EInteractionType
{
    Instantaneous = 0,
    OverTime = 1
}


public class InteractionStatChange
{
    public EStat Target;
    public float Value;

}
public abstract partial class BaseInteraction: Node
{
    [Export] protected string _DisplayName;
    [Export] protected EInteractionType _interactionType = 0;
    [Export] protected float _interactionDuraction = 0f; 
    [Export] public PhysicsBody3D ObjectBody;
    
    //x => enum type, y=change(0-1)
    [Export]protected Godot.Collections.Array<Vector2> StatChangesVectors;
    protected List<InteractionStatChange> StatChanges;

    public override void _Ready()
    {
        StatChanges = new List<InteractionStatChange>();
        foreach (var statVector in StatChangesVectors)
        {
            try
            {
                StatChanges.Add(new InteractionStatChange() { Target = (EStat)statVector.X, Value = statVector.Y });
            }
            catch (InvalidCastException e)
            {
                GD.PrintErr($"error assigning proper EStat Value: {e}");
            }
        }
    }

    public string DisplayName => _DisplayName;
    public EInteractionType InteractionType => _interactionType;
    public float InteractionDuraction => _interactionDuraction;

    public abstract bool CanPerform();
    
    /* allows for only one actor to act apon the object if required
     * (onely one person can open the cupboard at a time)
     */
    public abstract void LockInteraction();
    //maybe change to characterbody2d after the dust has settled or inherited player class
    public abstract void Perform(BaseAi performer, Action<BaseInteraction>onCompleted);
    public abstract void UnLockInteraction();

    public void ApplyStatChanges(BaseAi performer, float proportion)
    {
        foreach (var statChange in StatChanges)
        {
            performer.UpdateIndividualStat(statChange.Target, statChange.Value * proportion);
        }
    }
    


}