using Godot;
namespace MPhysicsSystem.Example;

public partial class UISetRelayNormal : HSlider
{
    [Export] PhysicsSystemRelay relay;

    public override void _Ready()
    {
        ValueChanged += WhenSliderChange;
        relay.SetDriverValueY((float)Value);
    }

    private void WhenSliderChange(double value)
    {
        relay.SetDriverValueY((float)value);
    }
}// EOF CLASS
