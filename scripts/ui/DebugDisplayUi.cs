namespace Incorgnito.scripts.ui;

using System.Globalization;

using Godot;
using Godot.Collections;

public partial class DebugDisplayUi : Control
{
	private Label _debugMessageLabel;
	private string _debugMessage;
	private CustomSignals _debugSignal;
	
	private Label _hungerValueLabel;
	private Label _energyValueLabel;
	private Label _socialValueLabel;
	
	private float _hungerValue;
	private float _energyValue;
	private float _socialValue;
	

	public override void _Ready()
	{
		//needs display
		_hungerValueLabel = GetNode<Label>("BoxContainer/HungerCont/HungerLabelValue");
		_energyValueLabel = GetNode<Label>("BoxContainer/EnergyCont/EnergyLabelValue");
		_socialValueLabel = GetNode<Label>("BoxContainer/SocialCont/SocialLabelValue");
		
		//debug message display
		_debugSignal = GetNode<CustomSignals>("/root/CustomSignals");
		_debugMessageLabel = GetNode<Label>("DebugMessage");
		_debugSignal.DebugMessage += OnDebugMessage;
		_debugSignal.DebugStatsDisplay += OnDebugStatsDisplay;
	}

	public override void _Process(double delta)
	{
		_debugMessageLabel.Text = _debugMessage;
		_hungerValueLabel.Text = _hungerValue.ToString("F4",CultureInfo.InvariantCulture);
		_energyValueLabel.Text = _energyValue.ToString("F4",CultureInfo.InvariantCulture);
		_socialValueLabel.Text = _socialValue.ToString("F4",CultureInfo.InvariantCulture);
		
	}

	public void OnDebugMessage(string debugMessage)
	{
		_debugMessage = debugMessage;
	}

	public void OnDebugStatsDisplay(Dictionary<string, float> needsDictionary)
	{
		needsDictionary.TryGetValue("hunger", out _hungerValue);
		needsDictionary.TryGetValue("energy", out _energyValue);
		needsDictionary.TryGetValue("social", out _socialValue);
		
	}
}
