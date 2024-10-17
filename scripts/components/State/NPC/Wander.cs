namespace Incorgnito.scripts.components.state.NPC;

using Godot;

using actors.npc;
using state;

public partial class Wander: AbstractActionState
{
    //character modifiers
    private Vector3 _currentBearing;
    
    //signal fields
    [Export] private Area2D _detectionArea;
    private Timer _randSpeedAdjustmentTimer = new Timer();
    
    public override void Enter()
    {
        if (Npc is null)
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
    } 

    public override void UpdatePhysicsProcess(double delta)
    {
      
        if (Npc is not null)
        {
            _randomWalk();
        }
    }

    private void _randomWalk()
    {
        Npc.Velocity = _currentBearing * Npc.Speed;

        Npc.MoveAndSlide();
    }
    private void _setRandomBearing()
    {
        
        var direction = GD.Randi() % 20;

        _currentBearing = new Vector3(GD.RandRange(-1,1),0,GD.RandRange(-1,1)).Normalized();
        
    }
    
    //**************************
    // Signal Callbacks
    //**************************
	
    private void OnBearingAdjustmentTimeout()
    {
        this._setRandomBearing();
    }

    public override string ToString()
    {
        return "Wander";
    }

   
}