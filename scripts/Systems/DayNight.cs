using System.Globalization;

namespace Incorgnito.scripts.Systems;
using Godot;

using scripts;

public partial class DayNight : Node
{
	[Export] private DirectionalLight3D _lightSource;

	[Export]
	private int _realMinutesPerDay;

	private float _normalizedTime;
	public override void _Ready()
	{
		WorldClock.Instance.SetWorldClockSpeed(_realMinutesPerDay);
		_lightSource.SetRotation(new Vector3(0,0,0));
	}

	float GetRotationPerSecond()
	{
		return 360 / (WorldClock.Instance.RealMinutesPerDay * 60.0f);
	}

	public override void _Process(double delta)
	{
		_normalizedTime = WorldClock.Instance.GetNormalizedTime();
		_lightSource.SetRotation(new Vector3(GetRotationPerSecond() * _normalizedTime, 0, 0));	
		GD.Print(_normalizedTime.ToString("F4", CultureInfo.InvariantCulture));
	}
}
