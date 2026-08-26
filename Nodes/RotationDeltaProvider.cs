using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationDeltaProvider : PhysicsSystemComponent, IPhysicsComponent
{
    public Vector3 RotationDelta { get; set; } = Vector3.Zero;

    public override void _Ready()
    {
        if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
        {
            body = GetParent<RigidBody3D>();
            physicsSystem.RegisterPhysicsComponent(this);
        }
    }

    public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialTransform)
    {
        Vector3 localAngVel = state.Transform.Basis.Inverse() * state.AngularVelocity;
        RotationDelta = localAngVel * state.Step;
    }
}// EOF CLASS
