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
	private float _currentRotation = 0.0f;
	
	public override void _Ready()
	{
		WorldClock.Instance.SetWorldClockSpeed(_realMinutesPerDay);
		_lightSource.SetRotation(new Vector3(90,0,0));
	}

	float GetRotationPerSecond()
	{
		return 360 / (WorldClock.Instance.RealMinutesPerDay * 60.0f);
	}

	public override void _Process(double delta)
	{
		var normalizedTimeIncrement = 1.0f / (WorldClock.Instance.RealMinutesPerDay * 60.0f);
		_normalizedTime = (WorldClock.Instance.GetNormalizedTime() + normalizedTimeIncrement * (float)delta) % 1.0f;
		
		
		_lightSource.SetRotation(new Vector3(90 + (GetRotationPerSecond() * _normalizedTime), 0, 0));	
		
		// _lightSource.SetRotation(new Vector3(GetRotationPerSecond() * , 0, 0));	
		GD.Print(_normalizedTime.ToString("F4", CultureInfo.InvariantCulture));
	}
}
