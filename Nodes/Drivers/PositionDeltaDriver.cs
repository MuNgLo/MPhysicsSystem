using System;
using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 Fixed so it doesn't touch the state.transform. But untested
/// </summary>
[GlobalClass]
public partial class PositionDeltaDriver : PhysicsSystemComponent, IPhysicsComponent
{
	Vector3 influence = Vector3.Zero;

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
		Vector3 localLinearVelocity = initialGlobalTransform.Basis.Inverse() * state.LinearVelocity - influence;
		influence = Vector3.Up * DeltaProviderRotation();
		state.LinearVelocity = initialGlobalTransform.Basis * (localLinearVelocity + influence);
    }
}// EOF CLASS
