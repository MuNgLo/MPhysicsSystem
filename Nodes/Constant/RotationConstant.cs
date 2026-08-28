using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationConstant : PhysicsSystemComponent, IPhysicsComponent
{
    [Export(PropertyHint.GroupEnable), ExportGroup("Velocity")] private bool velocityEnabled = false;
    [Export(PropertyHint.Range, "-360, 360, 0.01, radians_as_degrees, suffix:°/s")]
    public Vector3 anglePerSecond { get; set; } = new(0, 0, 0);
    [Export(PropertyHint.Range, "0,1")]
    public float Dampening
    {
        get => 1.0f - dampening;
        set => dampening = 1.0f - value;
    }
    float dampening = 0.0f;
    [Export(PropertyHint.Range, "0,1")] public float compliance = 1.0f;

    [Export(PropertyHint.GroupEnable), ExportGroup("Force")] private bool forceEnabled = false;
    [Export] public Vector3 Torque { get; set; } = new(0, 0, 0);

    public override void _Ready()
    {
        if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
        {
            body = GetParent<RigidBody3D>();
            physicsSystem.RegisterPhysicsComponent(this);
        }
    }

    public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialGlobalTransform)
    {
        Vector3 localAngVel = initialGlobalTransform.Basis.Inverse() * state.AngularVelocity;
        if (velocityEnabled)
        {
            localAngVel *= dampening;
            localAngVel = localAngVel.Lerp(anglePerSecond, compliance);
        }
        state.AngularVelocity = state.Transform.Basis * localAngVel;
    }
}// EOF CLASS
