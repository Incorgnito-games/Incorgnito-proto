using Incorgnito.scripts.components.state;

namespace Incorgnito.scripts.ui;

using Godot;
using actors.npc;

public partial class NPCTraitStatsUI : Control
{
    private Label _hungerLabelValue;
    private Label _restLabelValue;
    private Label _socialLabelValue;
    private Label _debugMessageLabel;
    [Export]private Npc _npc;
    private StateSignals _stateSignals;

    private string _debugMessage;
    private float _hunger;
    private float _rest;
    private float _social;
    public override void _Ready()
    {
        _stateSignals = GetNode<StateSignals>("/root/StateSignals");
        _debugMessageLabel = GetNode<Label>("DebugMessage");
        _hungerLabelValue = GetNode<Label>("BoxContainer/HungerCont/HungerLabelValue");
        _restLabelValue = GetNode<Label>("BoxContainer/RestCont/RestLabelValue");
        _socialLabelValue = GetNode<Label>("BoxContainer/SocialCont/SocialLabelValue");
        if (_npc is not null)
        {
            _hunger = _npc.Traits[0].Value;
            _rest = _npc.Traits[1].Value;
            _social = _npc.Traits[2].Value;
            _npc.UpdateNpcTraitStats += OnUpdateNpcTraitsSignal;
            _stateSignals.StateDebugMessage += OnStateDebugMessage;
        }

    }

    public override void _Process(double delta)
    {
        _hungerLabelValue.Text = _hunger.ToString();
        _restLabelValue.Text = _rest.ToString();
        _socialLabelValue.Text = _social.ToString();
        _debugMessageLabel.Text = _debugMessage;
    }

    private void OnUpdateNpcTraitsSignal(float hunger, float rest, float social)
    {
        this._hunger = hunger;
        this._rest = rest;
        this._social = social;
        
    }

    private void OnStateDebugMessage(AbstractActionState state, string debugMessage)
    {
        this._debugMessage = $"{state.ToString()} -> {debugMessage}";
    }

}