namespace Incorgnito.scripts.objects;
using Godot;
using Systems;
using System.Collections.Generic;

public partial class Building : StaticBody3D
{ 
	private Label3D _debugLabel;
    public List<string> Owners;
    [Export] public bool IsPublicBuilding { get; set; } = false;

	private SmartObject SmartObject;
	public override void _Ready()
	{
		Owners = new List<string>();
		_debugLabel = GetNode<Label3D>("BuildingBox/Label3D");
		SmartObject = GetNode<SmartObject>("SmartObject");
		CallDeferred(nameof(SetOwnership));
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
		_debugLabel.Text = new string($"{ownersString} {SmartObject.DisplayName}");
		
	}

	public override void _Process(double delta)
	{
	}
}
