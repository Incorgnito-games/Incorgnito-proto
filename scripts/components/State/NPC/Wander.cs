namespace Incorgnito.scripts.components.state.NPC;

using Godot;

public partial class Wander: State
{
    //character modifiers
    [Export] private Npc _npc;
    private Vector3 _currentBearing;
    
    //signal fields
    [Export] private Area2D _detectionArea;
    private Timer _randSpeedAdjustmentTimer = new Timer();
    
    public override void Enter()
    {
        if (_npc is null)
        {
            return;
        }
        _setRandomBearing();
    }

    public override void Exit()
    {
    }

    public override void UpdateProcess(double delta)
    {
    }
    public override void _Ready()
    {
        base._Ready();
        //state transition setup
        // _detectionArea.BodyEntered += OnDetectionAreaBodyEntered;
        
        //Timer Setup
        _randSpeedAdjustmentTimer.SetAutostart(true);
        _randSpeedAdjustmentTimer.SetWaitTime(GD.RandRange(1,8));
        _randSpeedAdjustmentTimer.Timeout += OnBearingAdjustmentTimeout;
        AddChild(_randSpeedAdjustmentTimer);

        _currentBearing = new Vector3(GD.RandRange(-1,1),0,GD.RandRange(-1,1));
         // _meat.Velocity = _currentBearing * (float)_meat.baseSpeed;
    } 

    public override void UpdatePhysicsProcess(double delta)
    {
      
        if (_npc is not null)
        {
            _randomWalk();
        }
    }

    private void _randomWalk()
    {
        _npc.Velocity = _currentBearing * _npc.Speed;

        _npc.MoveAndSlide();
    }
    private void _setRandomBearing()
    {
        
        var direction = GD.Randi() % 20;

        _currentBearing = new Vector3(GD.RandRange(-1,1),0,GD.RandRange(-1,1));
        
    }
    
    //**************************
    // Signal Callbacks
    //**************************
	
    private void OnBearingAdjustmentTimeout()
    {
        this._setRandomBearing();
    }

   
}