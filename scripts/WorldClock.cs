namespace Incorgnito.scripts;
using System;
using Godot;

public partial class WorldClock: Node
{
    private double _timeAccumulator = 0.0;
    private int _secondsInDay = 86400;

    public static  WorldClock Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PrintErr($"Error: Trying to create second WorldClock on {Name}");
            QueueFree();
            return;
        }

        Instance = this;
        SetProcess(true);
    }
    void UpdateTime(double delta)
    {
        _timeAccumulator += delta;

        string timeString = Get24HourClockString(_timeAccumulator);
        // GD.Print(timeString);
        
        _timeAccumulator %= _secondsInDay;
    }

    public override void _Process(double delta)
    {
        UpdateTime(delta);
    }

    public string Get24HourClockTime()
    {
        return Get24HourClockString(_timeAccumulator);
    }

    public string Get24HourClockString(double time)
    {
        int totalSeconds = (int)time; 
        int seconds = totalSeconds % 60;
        int totalMinutes = totalSeconds / 60;
        int minutes = totalMinutes % 60;
        int hours = (totalMinutes / 60) % 24; 

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}