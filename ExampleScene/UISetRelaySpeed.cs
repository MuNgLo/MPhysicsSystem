using Godot;

namespace MPhysicsSystem.Example;

public partial class UISetRelaySpeed : HSlider
{
    [Export] PhysicsSystemComponent relay;

    public override void _Ready()
    {
        ValueChanged += WhenSliderChange;
        relay.Speed = (float)Value;
    }

    private void WhenSliderChange(double value)
    {
        relay.Speed = (float)value;
    }
}// EOF CLASS
