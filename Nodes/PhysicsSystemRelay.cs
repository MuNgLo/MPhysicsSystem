using Godot;

namespace MPhysicsSystem;

[GlobalClass]
public partial class PhysicsSystemRelay : PhysicsSystemComponent
{
    [Export] PhysicsSystemComponent rotationDeltaProvider;
    public override Vector3 RotationDelta
    {
        get => rotationDeltaProvider.RotationDelta;
        set => base.RotationDelta = value;
    }

    [Export] PhysicsSystemComponent rotationConstraint;
    public override float DriverValueY
    {
        get => rotationConstraint.DriverValueY;
    }

}// EOF CLASS
