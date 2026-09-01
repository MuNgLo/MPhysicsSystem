using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 cleared. Doesn't itself actually manipulate the physics body
/// </summary>
[GlobalClass]
public partial class RotationDeltaProvider : PhysicsSystemComponent, IPhysicsComponent
{

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
