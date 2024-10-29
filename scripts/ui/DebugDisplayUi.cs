using System;
using System.Security.Cryptography;

namespace Incorgnito.scripts.ui;

using System.Globalization;

using Godot;
using Godot.Collections;
using Incorgnito.scripts;

public enum ENpc
{
	Steve,
	Bob,
	Jennifer,
	Suzy,
	JohneyNpc
}
public partial class DebugDisplayUi : Control
{
	
	private Label[] _debugMessageLabel = new Label[4];
	private string[] _debugMessage = new string[4];
	private CustomSignals _debugSignal;
	private Label _worldTimeClockLabel;
	
	private Label[] _hungerValueLabel = new Label[4];
	private Label[] _energyValueLabel = new Label[4];
	private Label[] _socialValueLabel = new Label[4];
	private Label[] _moneyValueLabel = new Label[4];
	
	private float[] _hungerValue = new float[4];
	private float[] _energyValue = new float[4];
	private float[] _socialValue = new float[4];
	private float[] _moneyValue = new float[4];
	
	
	public override void _Ready()
	{
		
		//steve needs display
		_hungerValueLabel[(int)ENpc.Steve] = GetNode<Label>("DebugInfoContainer/SteveContainer/HungerCont/HungerLabelValue");
		_energyValueLabel[(int)ENpc.Steve] = GetNode<Label>("DebugInfoContainer/SteveContainer/EnergyCont/EnergyLabelValue");
		_socialValueLabel[(int)ENpc.Steve] = GetNode<Label>("DebugInfoContainer/SteveContainer/SocialCont/SocialLabelValue");
		_moneyValueLabel[(int)ENpc.Steve] = GetNode<Label>("DebugInfoContainer/SteveContainer/MoneyCont/MoneyLabelValue");
		_debugMessageLabel[(int)ENpc.Steve] = GetNode<Label>("DebugInfoContainer/SteveContainer/DebugMessage");
	
		//bob
		_hungerValueLabel[(int)ENpc.Bob] = GetNode<Label>("DebugInfoContainer/BobContainer/HungerCont/HungerLabelValue");
		_energyValueLabel[(int)ENpc.Bob] = GetNode<Label>("DebugInfoContainer/BobContainer/EnergyCont/EnergyLabelValue");
		_socialValueLabel[(int)ENpc.Bob] = GetNode<Label>("DebugInfoContainer/BobContainer/SocialCont/SocialLabelValue");
		_moneyValueLabel[(int)ENpc.Bob] = GetNode<Label>("DebugInfoContainer/BobContainer/MoneyCont/MoneyLabelValue");
		_debugMessageLabel[(int)ENpc.Bob] = GetNode<Label>("DebugInfoContainer/BobContainer/DebugMessage");	
		
		//jennifer
		_hungerValueLabel[(int)ENpc.Jennifer] = GetNode<Label>("DebugInfoContainer/JenniferContainer/HungerCont/HungerLabelValue");
		_energyValueLabel[(int)ENpc.Jennifer] = GetNode<Label>("DebugInfoContainer/JenniferContainer/EnergyCont/EnergyLabelValue");
		_socialValueLabel[(int)ENpc.Jennifer] = GetNode<Label>("DebugInfoContainer/JenniferContainer/SocialCont/SocialLabelValue");
		_moneyValueLabel[(int)ENpc.Jennifer] = GetNode<Label>("DebugInfoContainer/JenniferContainer/MoneyCont/MoneyLabelValue");
		_debugMessageLabel[(int)ENpc.Jennifer] = GetNode<Label>("DebugInfoContainer/JenniferContainer/DebugMessage");	
		
		//suzy
		_hungerValueLabel[(int)ENpc.Suzy] = GetNode<Label>("DebugInfoContainer/SuzyContainer/HungerCont/HungerLabelValue");
		_energyValueLabel[(int)ENpc.Suzy] = GetNode<Label>("DebugInfoContainer/SuzyContainer/EnergyCont/EnergyLabelValue");
		_socialValueLabel[(int)ENpc.Suzy] = GetNode<Label>("DebugInfoContainer/SuzyContainer/SocialCont/SocialLabelValue");
		_moneyValueLabel[(int)ENpc.Suzy] = GetNode<Label>("DebugInfoContainer/SuzyContainer/MoneyCont/MoneyLabelValue");
		_debugMessageLabel[(int)ENpc.Suzy] = GetNode<Label>("DebugInfoContainer/SuzyContainer/DebugMessage");	
		
		//debug message display
		_debugSignal = GetNode<CustomSignals>("/root/CustomSignals");
		_debugSignal.DebugMessage += OnDebugMessage;
		_debugSignal.DebugStatsDisplay += OnDebugStatsDisplay;
		
		//World Time
		_worldTimeClockLabel = GetNode<Label>("WorldClockLabel");
	}

	public override void _Process(double delta)
	{
		_worldTimeClockLabel.Text = WorldClock.Instance.Get24HourClockTime();
		
		//steve
		_debugMessageLabel[(int)ENpc.Steve].Text = _debugMessage[(int)ENpc.Steve];
		_hungerValueLabel[(int)ENpc.Steve].Text = _hungerValue[(int)ENpc.Steve].ToString("F4",CultureInfo.InvariantCulture);
		_energyValueLabel[(int)ENpc.Steve].Text = _energyValue[(int)ENpc.Steve].ToString("F4",CultureInfo.InvariantCulture);
		_socialValueLabel[(int)ENpc.Steve].Text = _socialValue[(int)ENpc.Steve].ToString("F4",CultureInfo.InvariantCulture);
		_moneyValueLabel[(int)ENpc.Steve].Text = _moneyValue[(int)ENpc.Steve].ToString("F4",CultureInfo.InvariantCulture);
		
		//bo
		_debugMessageLabel[(int)ENpc.Bob].Text = _debugMessage[(int)ENpc.Bob];
		_hungerValueLabel[(int)ENpc.Bob].Text = _hungerValue[(int)ENpc.Bob].ToString("F4",CultureInfo.InvariantCulture);
		_energyValueLabel[(int)ENpc.Bob].Text = _energyValue[(int)ENpc.Bob].ToString("F4",CultureInfo.InvariantCulture);
		_socialValueLabel[(int)ENpc.Bob].Text = _socialValue[(int)ENpc.Bob].ToString("F4",CultureInfo.InvariantCulture);
		_moneyValueLabel[(int)ENpc.Bob].Text = _moneyValue[(int)ENpc.Bob].ToString("F4",CultureInfo.InvariantCulture);
		
		//jennife
		_debugMessageLabel[(int)ENpc.Jennifer].Text = _debugMessage[(int)ENpc.Jennifer];
		_hungerValueLabel[(int)ENpc.Jennifer].Text = _hungerValue[(int)ENpc.Jennifer].ToString("F4",CultureInfo.InvariantCulture);
		_energyValueLabel[(int)ENpc.Jennifer].Text = _energyValue[(int)ENpc.Jennifer].ToString("F4",CultureInfo.InvariantCulture);
		_socialValueLabel[(int)ENpc.Jennifer].Text = _socialValue[(int)ENpc.Jennifer].ToString("F4",CultureInfo.InvariantCulture);
		_moneyValueLabel[(int)ENpc.Jennifer].Text = _moneyValue[(int)ENpc.Jennifer].ToString("F4",CultureInfo.InvariantCulture);
		
		//suz
		_debugMessageLabel[(int)ENpc.Suzy].Text = _debugMessage[(int)ENpc.Suzy];
		_hungerValueLabel[(int)ENpc.Suzy].Text = _hungerValue[(int)ENpc.Suzy].ToString("F4",CultureInfo.InvariantCulture);
		_energyValueLabel[(int)ENpc.Suzy].Text = _energyValue[(int)ENpc.Suzy].ToString("F4",CultureInfo.InvariantCulture);
		_socialValueLabel[(int)ENpc.Suzy].Text = _socialValue[(int)ENpc.Suzy].ToString("F4",CultureInfo.InvariantCulture);
		_moneyValueLabel[(int)ENpc.Suzy].Text = _moneyValue[(int)ENpc.Suzy].ToString("F4",CultureInfo.InvariantCulture);
		
	}

	public void OnDebugMessage(string debugMessage, string npcString)
	{
		Enum.TryParse(npcString, true, out ENpc npc);
		_debugMessage[(int)npc] = debugMessage;
	}

	public void OnDebugStatsDisplay(Dictionary<string, float> needsDictionary, string npcString)
	{

		Enum.TryParse(npcString, true, out ENpc npc);
		
		
		needsDictionary.TryGetValue("hunger", out _hungerValue[(int)npc]);
		needsDictionary.TryGetValue("energy", out _energyValue[(int)npc]);
		needsDictionary.TryGetValue("social", out _socialValue[(int)npc]);
		needsDictionary.TryGetValue("money", out _moneyValue[(int)npc]);
		
	}
}
