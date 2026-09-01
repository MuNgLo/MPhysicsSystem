using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationDeltaDriver : PhysicsSystemComponent, IPhysicsComponent
{
	[Export] MONITOREDAXIS drivenAxis = MONITOREDAXIS.Y;

	private Transform3D initialTransform;

	public override void _Ready()
	{
		if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
		{
			initialTransform = physicsSystem.GlobalTransform;
			body = GetParent<RigidBody3D>();
			physicsSystem.RegisterPhysicsComponent(this);
		}
	}

	public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D otherTR)
	{
		Transform3D localTransform = initialTransform.Inverse() * state.Transform;
		state.Transform = initialTransform * localTransform.RotatedLocal(DrivenAxis(drivenAxis), MonitoredRotationDelta() * sensitivity);
	
		//state.AngularVelocity = state.Transform.Basis * localAngularVelocity;
	
	}




}// EOF CLASS
