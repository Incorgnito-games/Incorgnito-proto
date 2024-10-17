namespace Incorgnito.scripts.ui;

using Godot;
using actors.npc;

public partial class WorldStats : BoxContainer
{
    private Label _hungerLabelValue;
    private Label _restLabelValue;
    private Label _socialLabelValue;
    [Export]private Npc _npc;

    private float _hunger;
    private float _rest;
    private float _social;
    public override void _Ready()
    {
        _hungerLabelValue = GetNode<Label>("LionPopCont/LionPopValue");
        _restLabelValue = GetNode<Label>("RabbitPopCont/RabbitPopValue");
        if (_npc is not null)
        {
            _hunger = _npc.Traits[0].Value;
            _rest = _npc.Traits[1].Value;
            _social = _npc.Traits[2].Value;
            _npc.UpdateNpcTraitStats += OnUpdateNpcTraitsSignal;
        }

    }

    public override void _Process(double delta)
    {
        _hungerLabelValue.Text = _hunger.ToString();
        _restLabelValue.Text = _rest.ToString();
        _socialLabelValue.Text = _social.ToString();
    }

    private void OnUpdateNpcTraitsSignal(float hunger, float rest, float social)
    {
        this._hunger = hunger;
        this._rest = rest;
        this._social = social;
    }

}