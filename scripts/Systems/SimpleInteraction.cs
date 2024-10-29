using Incorgnito.scripts.actors.ai;
using Incorgnito.scripts.objects;
using Microsoft.VisualBasic;

namespace Incorgnito.scripts.Systems;
using Godot;
using System;
using System.Collections.Generic;

public partial class SimpleInteraction: BaseInteraction
{
    protected class PerformerData
    {
        public BaseAi PerformingAi;
        public float ElapsedTime;
        public Action<BaseInteraction> Oncomplete;
    }
    
    [Export] protected int MaxSimultaneousActors = 1;
    protected int NumCurrentActors = 0;

    private List<PerformerData> CurrentPerformers = new List<PerformerData>();
    public override bool CanPerform()
    {
        //this will be breaking if not building TODO: not this-->
        
        // if (areaObj IsPublicArea: false} or {Owners.Count:<= 0} )
        // {
        //     return false;
        // }
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

    
    public override void Perform(BaseAi performer,Action<BaseInteraction>onCompleted)
    {
        if (NumCurrentActors <= 0)
        {
            GD.PrintErr($"Trying to preform an interaction without any actors: {_DisplayName}");
            return;
        }

        if (InteractionType == EInteractionType.Instantaneous)
        {
            if (StatChanges.Count > 0)
            {
                ApplyStatChanges(performer, 1f);
            }
            onCompleted?.Invoke(this);
        }else if (InteractionType == EInteractionType.OverTime)
        {
            CurrentPerformers.Add(new PerformerData(){PerformingAi = performer, ElapsedTime = 0, Oncomplete = onCompleted});
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

            float previousElapsedTime = performer.ElapsedTime;
            performer.ElapsedTime = Mathf.Min(performer.ElapsedTime + (float)delta, _interactionDuraction);
           
            if (StatChanges.Count > 0)
            {
                ApplyStatChanges(performer.PerformingAi, (performer.ElapsedTime - previousElapsedTime)/_interactionDuraction);
            }
            //interaction complete
            if (performer.ElapsedTime >= _interactionDuraction)
            {
                performer.Oncomplete.Invoke(this);
                CurrentPerformers.RemoveAt(index);
            }
        }
    }
}