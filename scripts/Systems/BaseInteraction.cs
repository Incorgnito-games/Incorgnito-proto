namespace Incorgnito.scripts.Systems;

using Godot;
using System;

public enum EInteractionType
{
    Instantaneous = 0,
    OverTime = 1
}
public abstract partial class BaseInteraction: Node
{
    [Export] protected string _DisplayName;
    [Export] protected EInteractionType _interactionType = 0;
    [Export] protected float _interactionDuraction = 0f; 
    [Export]public PhysicsBody3D ObjectBody;
    
    public string DisplayName => _DisplayName;
    public EInteractionType InteractionType => _interactionType;
    public float InteractionDuraction => _interactionDuraction;

    public abstract bool CanPerform();
    
    /* allows for only one actor to act apon the object if required
     * (onely one person can open the cupboard at a time)
     */
    public abstract void LockInteraction();
    //maybe change to characterbody2d after the dust has settled or inherited player class
    public abstract void Perform(Node performer, Action<BaseInteraction>onCompleted);
    public abstract void UnLockInteraction();


}