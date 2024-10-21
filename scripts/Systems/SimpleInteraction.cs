using Microsoft.VisualBasic;

namespace Incorgnito.scripts.Systems;
using Godot;
using System;
using System.Collections.Generic;

public partial class SimpleInteraction: BaseInteraction
{
    protected class PerformerData
    {
        public float ElapsedTime;
        public Action<BaseInteraction> Oncomplete;
    }
    
    [Export] protected int MaxSimultaneousActors = 1;
    protected int NumCurrentActors = 0;

    private List<PerformerData> CurrentPerformers = new List<PerformerData>();
    public override bool CanPerform()
    {
        return NumCurrentActors < MaxSimultaneousActors;
    }

    public override void LockInteraction()
    {
        NumCurrentActors++;
        if (NumCurrentActors > MaxSimultaneousActors)
        {
            GD.Print($"Too many actors have locked this interaction: {_DisplayName}");
        }
    }

    
    public override void Perform(Node performer,Action<BaseInteraction>onCompleted)
    {
        if (NumCurrentActors <= 0)
        {
            GD.PrintErr($"Trying to preform an interaction without any actors: {_DisplayName}");
            return;
        }

        if (InteractionType == EInteractionType.Instantaneous)
        {
            onCompleted?.Invoke(this);
        }else if (InteractionType == EInteractionType.OverTime)
        {
            CurrentPerformers.Add(new PerformerData(){ElapsedTime = 0, Oncomplete = onCompleted});
        }
    }

    public override void UnLockInteraction()
    {
        if (NumCurrentActors <= 0)
        {
            GD.PrintErr($"Trying to unlock an already unlocked interaction: {_DisplayName}");
        }
        
        NumCurrentActors--;
    }

    public override void _Process(double delta)
    {
        //update any current performers
        for (int index = CurrentPerformers.Count - 1; index >= 0; index--)
        {
            PerformerData performer = CurrentPerformers[index];

            performer.ElapsedTime += (float)delta;
            
            //interaction complete
            if (performer.ElapsedTime >= _interactionDuraction)
            {
                performer.Oncomplete.Invoke(this);
                CurrentPerformers.RemoveAt(index);
            }
        }
    }
}