namespace Incorgnito.scripts;
using System;
using Godot;

public partial class WorldClock: Node
{
    
    private float _timeAccumulator = 0.0f;
    private int _secondsInDay  = 86400;
    public float TimeRatioMultiplier { get; private set; }= 1f;
    public int RealMinutesPerDay = 1440;

    public static  WorldClock Instance { get; private set; }

    public void SetWorldClockSpeed(int realMinutesPerDay)
    {
     
        RealMinutesPerDay = realMinutesPerDay;
        TimeRatioMultiplier = 1440.0f/realMinutesPerDay;
        
    }

    public void SetTimeOfDay(float time)
    {
        _timeAccumulator = time * 60 * 60;
    } 
    
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
        _timeAccumulator += (float)delta * TimeRatioMultiplier;

        string timeString = Get24HourClockString(_timeAccumulator);
        // GD.Print(timeString);
        
        _timeAccumulator %= _secondsInDay;
    }

    public override void _Process(double delta)
    {
        UpdateTime(delta);
    }

    public float GetNormalizedTime()
    {
        return _timeAccumulator / _secondsInDay;
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