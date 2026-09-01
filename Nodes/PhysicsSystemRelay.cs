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
    [Export] bool flipped = false;


    float driverValueY;

    delegate float ValueResolver();

    ValueResolver ResolveDriverValueY;

    public override float DriverNormalizedValueY
    {
        get => ResolveDriverValueY();
    }
    public void SetDriverValueY(float newValue) { driverValueY = newValue; }


    public override void _Ready()
    {
        if(rotationConstraint is not null){
            ResolveDriverValueY = () => { return rotationConstraint.DriverNormalizedValueY; };
            return;
        }
        if(flipped){
            ResolveDriverValueY = () => { return 1.0f - driverValueY; };
            return;
        }
        ResolveDriverValueY = () => { return driverValueY; };
    }
}// EOF CLASS
