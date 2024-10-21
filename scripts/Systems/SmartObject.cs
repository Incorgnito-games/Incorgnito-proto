namespace Incorgnito.scripts.Systems;
using Godot;
using System.Collections.Generic;

public partial class SmartObject : Node
{
   [Export] public string DisplayName;
   [Export] public PhysicsBody3D ObjectBody;
   [Export] public float TargetDistanceTolerance = 12f;
   
    protected List<BaseInteraction> CachedInteractions = null;
	
	//Dynamically fecth the interactions every time incase something has changed
	//ie item added drawer or something -- may move to _ready to only load once for optimization
	// may not need this on every get
	public List<BaseInteraction> Interactions
	{
		get
		{
			if (CachedInteractions == null)
			{
				CachedInteractions = new List<BaseInteraction>();

				foreach (var child in GetChildren())
				{
					if (child is BaseInteraction interaction)
					{
						CachedInteractions.Add(interaction);
						GD.Print(interaction.DisplayName);
					}
					
				}
			}
			return CachedInteractions;
		}
	}
	public override void _Ready()
	{
		SmartObjectManager.Instance.RegisterSmartObject(this);
		
	}

	private void OnDestroy()
	{
		SmartObjectManager.Instance.DeregisterSmartObject(this);
	}

	public override void _Process(double delta)
	{
	}
}
