using Godot;

namespace MPhysicsSystem;

[GlobalClass]
public partial class PhysicsSystemRelay : PhysicsSystemComponent
{
    [Export(PropertyHint.GroupEnable), ExportGroup("Rotation Delta")] private bool rotDeltaEnabled = false;
    [Export] PhysicsSystemComponent rotationDeltaProvider;
    [Export] float multiplier = 1.0f;

    public override Vector3 RotationDelta
    {
        get => rotationDeltaProvider.RotationDelta * multiplier;
        set => base.RotationDelta = value; // never used but here for compliance
    }

    [Export(PropertyHint.GroupEnable), ExportGroup("Rotation Normal")] private bool rotNormalEnabled = false;
    [Export] PhysicsSystemComponent rotationConstraint;
    public override float DriverValueY
    {
        get => rotationConstraint.DriverValueY;
    }

}// EOF CLASS
