namespace Incorgnito.scripts.ui;
using Godot;


public partial class DebugDisplayUI : Control
{
	private Label _debugMessageLable;
	private string _debugMessage;
	private CustomSignals _debugSignal;

	public override void _Ready()
	{
		_debugSignal = GetNode<CustomSignals>("/root/CustomSignals");
		_debugMessageLable = GetNode<Label>("DebugMessage");
		_debugSignal.DebugMessage += OnDebugMessage;
	}

	public override void _Process(double delta)
	{
		_debugMessageLable.Text = _debugMessage;
	}

	public void OnDebugMessage(string debugMessage)
	{
		this._debugMessage = debugMessage;
	}
}
