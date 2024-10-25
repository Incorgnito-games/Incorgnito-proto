using Incorgnito.scripts.objects;

namespace Incorgnito.scripts.actors.npc;

using Godot;
using System;

public partial class NpcCharacter : CharacterBody3D
{
	[ExportGroup("Initial Need Values")]
	[Export] private float _initialHunger = 0.5f;
	[Export] private float _initialEnergy = 0.5f;
	[Export] private float _initialSocial = 0.5f;
	[Export] private float _initialMoney = 1f;

	[ExportGroup("Need Decay Rates")]
	[Export] private float _baseHungerDecay = 0.005f;
	[Export] private float _baseEnergyDecay = 0.005f;
	[Export] private float _baseSocialDecay = 0.005f;
	[Export] private float _baseMoneyDecay = 0f;
	
	public float CurrentHunger { get;  set; }
	public float CurrentEnergy { get; set; }
	public float CurrentSocial { get; set; }
	public float CurrentMoney { get; set; }
	
	[ExportGroup("Character Setup")]	
	[Export] public float Speed = 5.0f;

	[Export] public string CharacterFirstName = "John";
	[Export] public string CharacterLastName = "";
	[Export] public Building Home;
	[Export] public Building Work;

	public override string ToString()
	{
		return new string($"{CharacterFirstName} {CharacterLastName}");
	}
	
	public override void _Ready()
	{
		CurrentHunger = _initialHunger;
		CurrentEnergy = _initialEnergy;
		CurrentSocial = _initialSocial;
		CurrentMoney = _initialMoney;
		Home.Owners.Add($"{CharacterFirstName} {CharacterLastName}");
		Work.Owners.Add($"{CharacterFirstName} {CharacterLastName}");
	}

	public override void _Process(double delta)
	{
		CurrentHunger = Mathf.Clamp(CurrentHunger - _baseHungerDecay * (float)delta, 0,1);
		CurrentEnergy = Mathf.Clamp(CurrentEnergy - _baseEnergyDecay * (float)delta, 0, 1);
		CurrentSocial = Mathf.Clamp(CurrentSocial - _baseSocialDecay * (float)delta, 0, 1);
		CurrentMoney = Mathf.Clamp(CurrentMoney - _baseMoneyDecay * (float)delta, 0, 1);
	}
	public override void _PhysicsProcess(double delta)
	{
	
	}
}
