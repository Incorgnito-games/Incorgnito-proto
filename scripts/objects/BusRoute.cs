namespace Incorgnito.scripts.objects;
using Godot;
using System;

public partial class BusRoute : PathFollow3D
{
	[Export] private float _busWaitTime;
	[Export] private float _busSpeed;
	private float _currentBusSpeed;
	private Timer _busStopTimer;
	public override void _Ready()
	{
		_busStopTimer = new Timer();
		_busStopTimer.SetAutostart(false);
		_busStopTimer.SetOneShot(false);
		_busStopTimer.SetWaitTime(_busWaitTime);
		_busStopTimer.Timeout += OnBusStopTimeout;
		AddChild(_busStopTimer);
		_currentBusSpeed = _busSpeed;

	}

	public override void _PhysicsProcess(double delta)
	{
		
		this.Progress += _currentBusSpeed;
	}

	public void OnBusStopTimeout()
	{
		// GD.Print("Bus stop");
		_currentBusSpeed = _busSpeed;
	}

	public void OnArea3dBodyEntered(Node body)
	{
		_currentBusSpeed = 0;
		_busStopTimer.Start();	
	}
}
