namespace Incorgnito.scripts.Systems;
using Godot;
using System.Collections.Generic;

public partial class SmartObject : Node
{
   [Export] public string DisplayName;
   public List<string> Owners;
   [Export] public PhysicsBody3D ObjectBody;
   [Export] public float TargetDistanceTolerance = 12f;
    [Export] public bool IsPublic { get; set; } = false;
	public string DebugLabel;
   
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
		Owners = new List<string>();
		// _debugLabel = GetNode<Label3D>("Label3D");
		CallDeferred(nameof(SetOwnership));
		SmartObjectManager.Instance.RegisterSmartObject(this);
		
	}

	private void OnDestroy()
	{
		SmartObjectManager.Instance.DeregisterSmartObject(this);
	}
	void SetOwnership()
	{
		string ownersString = "";
		for(var i = 0; i < Owners.Count; i++)
		{
			ownersString += Owners[i];
			ownersString += "'s";
			if (i == Owners.Count - 1)
			{
				continue;
			}

			ownersString += " and ";
		}
		 DebugLabel = new string($"{ownersString} {DisplayName}");
		
	}

	public override void _Process(double delta)
	{
	}
}
