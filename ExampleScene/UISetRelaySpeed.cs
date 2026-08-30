using Godot;

namespace MPhysicsSystem.Example;

public partial class UISetRelaySpeed : HSlider
{
	[Export] PhysicsSystemComponent relay;
	[Export] PhysicsSystemComponent relay2;

	public override void _Ready()
	{
		ValueChanged += WhenSliderChange;
		relay.Speed = (float)Value;
		if (relay2 is not null) { relay2.Speed = (float)Value; }
	}

	private void WhenSliderChange(double value)
	{
		relay.Speed = (float)value;
		if (relay2 is not null) { relay2.Speed = (float)value; }
	}
}// EOF CLASS
