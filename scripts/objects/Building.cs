namespace Incorgnito.scripts.objects;
using Godot;
using Systems;

public partial class Building : StaticBody3D
{
	private Label3D _debugLabel;

	private SmartObject _smartObject;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_debugLabel = GetNode<Label3D>("BuildingBox/Label3D");
		_smartObject = GetNode<SmartObject>("SmartObject");
		_debugLabel.Text = _smartObject.DisplayName;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
