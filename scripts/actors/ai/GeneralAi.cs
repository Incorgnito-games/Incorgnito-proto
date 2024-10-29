using System;
using System.Collections.Generic;
using Incorgnito.scripts.Systems;
using System.Linq;

namespace Incorgnito.scripts.actors.ai;

using Godot;
using objects;

public partial class GeneralAi: BaseAi
{
    [Export] protected float DefaultInteractScore = 0f;
    [Export] protected int InteractionPickSize = 1;
    float ScoreInteraction(BaseInteraction interaction)
    {
        if (interaction.StatChanges.Count == 0)
        {
            return DefaultInteractScore;
        }

        float score = 0f;

        foreach (var change in interaction.StatChanges)
        {
            score += ScoreChange(change.Target, change.Value);
        }

        return score;
    }

    float ScoreChange(EStat target, float amount)
    {
        float currentValue = 0f;
        switch (target)
        {
            case EStat.Hunger:
                currentValue = Npc.CurrentHunger;
                break;
            case EStat.Energy:
                currentValue = Npc.CurrentEnergy;
                break;
            case EStat.Social:
                currentValue = Npc.CurrentSocial;
                break;
            case EStat.Money:
                currentValue = Npc.CurrentMoney;
                break;
            default:
                GD.PrintErr("unknown stat fall through");
                break;
        }

        //future --> adjust with curves-----utility papers/book
        return (1f - currentValue) * amount;

    }

    class ScoredInteraction
    {
        public SmartObject TargetObject;
        public BaseInteraction Interaction;
        public float Score;

    }
    
    //picks best interaction based on stats
    protected override void PickInteraction()
    {
        List<ScoredInteraction> unsortedInteractions = new List<ScoredInteraction>();
        
        foreach (var smartObject in SmartObjectManager.Instance.RegisteredObjects)
        {
            foreach (var interaction in smartObject.Interactions)
            {
                if (!interaction.CanPerform())
                    continue;

             

                if (!smartObject.IsPublic && !smartObject.Owners.Contains(Npc.ToString()))
                {
                    continue;
                }
                var score = ScoreInteraction(interaction);
                
                unsortedInteractions.Add(new ScoredInteraction(){ TargetObject = smartObject, 
                                                                Interaction = interaction, 
                                                                Score = score});
          
                
                
            }
        }
        
        if (unsortedInteractions.Count == 0)
            return;

        var sortedInteractions = unsortedInteractions.OrderByDescending(scoredInteraction => scoredInteraction.Score).ToList();

        var maxIndex = Mathf.Min(InteractionPickSize, sortedInteractions.Count);
        var selectedIndex = GD.RandRange(0, maxIndex);

        SelectedObject = sortedInteractions[selectedIndex].TargetObject;
        CurrentInteraction = sortedInteractions[selectedIndex].Interaction;
        
        
        NavAgent.TargetDesiredDistance = SelectedObject.TargetDistanceTolerance;
        NavAgent.SetTargetPosition(SelectedObject.ObjectBody.GlobalPosition);

        CurrentInteraction.LockInteraction();
        
    }
}