namespace Incorgnito.scripts.Systems;

using Godot;

using Incorgnito.scripts.Systems;

public partial class BuildingInfo : StaticBody3D
{
	private SmartObject _smartObject;
	private Label3D _label;
	public override void _Ready()
	{
		_smartObject = GetNode<SmartObject>("SmartObject");
		_label = GetNode<Label3D>("Label3D");
		CallDeferred(nameof(AssignLabels));
	}

	void AssignLabels()
	{
		_label.Text = _smartObject.DebugLabel;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
