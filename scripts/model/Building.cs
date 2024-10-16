namespace Incorgnito.scripts.model;

using Godot;


public partial class Building : CsgBox3D
{

	[Export]private string _typeName;

	
	private Label3D _buildingLabel;

	private StandardMaterial3D _buildingMaterial;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_buildingLabel = GetNode<Label3D>("Label3D");
		_buildingLabel.Text = _typeName;

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
