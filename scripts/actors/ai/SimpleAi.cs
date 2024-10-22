namespace Incorgnito.scripts.actors.ai;

using Godot;
using Systems;

public partial class SimpleAi : BaseAi
{

	protected override void PickInteraction()
	{

		SelectedObject = SmartObjectManager.Instance.RegisteredObjects[
			GD.RandRange(0, SmartObjectManager.Instance.RegisteredObjects.Count - 1)];
		NavAgent.TargetDesiredDistance = SelectedObject.TargetDistanceTolerance;
		NavAgent.SetTargetPosition(SelectedObject.ObjectBody.GlobalPosition);
		
		var selectedInteraction = SelectedObject.Interactions[GD.RandRange(0, SelectedObject.Interactions.Count-1)];

		if(!selectedInteraction.CanPerform())
		{
			return;
		}
		
		CurrentInteraction = selectedInteraction; 
		CurrentInteraction.LockInteraction();
		
	}
}