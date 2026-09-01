using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 Uses Velocities
/// </summary>
[GlobalClass]
public partial class RotationDriver : PhysicsSystemComponent, IPhysicsComponent
{
	[Export(PropertyHint.Range, "-50000, 50000, 0.01, radians_as_degrees, suffix:°/s")] float MaxAngle = 0.0f;
	[Export(PropertyHint.Range, "-50000, 50000, 0.01, radians_as_degrees, suffix:°/s")] float MinAngle = 0.0f;

	public override void _Ready()
	{
		if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
		{
            cumulativeLocalAngles = Vector3.Zero;
			body = GetParent<RigidBody3D>();
			physicsSystem.RegisterPhysicsComponent(this);
		}
	}


 	// Tracks accumulated rotation per local axis (radians)
    private Vector3 cumulativeLocalAngles;
	float angularVelTweak = 0.0f;
	public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialGlobalTransform)
	{
		Vector3 localAngularVelocity = state.Transform.Basis.Inverse() * state.AngularVelocity;
        cumulativeLocalAngles += localAngularVelocity * state.Step;
		// calculate where we should be
		float rad = MinAngle + (MaxAngle - MinAngle) * MonitoredValue();
		// calculate how much we need to change
		rad = rad - cumulativeLocalAngles.Y ;
		angularVelTweak = rad / state.Step;
		localAngularVelocity.Y = angularVelTweak;
		state.AngularVelocity = state.Transform.Basis * localAngularVelocity;
	}

	float AxisPosition()
	{
		switch (monitoredAxis)
		{
			case MONITOREDAXIS.X:
				return body.Position.X;
			case MONITOREDAXIS.Z:
				return body.Position.Z;
		}
		return body.Position.Y;
	}

	float MonitoredValue()
	{
		switch (monitoredAxis)
		{
			case MONITOREDAXIS.X:
				return monitoredComponent.DriverNormalizedValueX;
			case MONITOREDAXIS.Z:
				return monitoredComponent.DriverNormalizedValueZ;
		}
		return monitoredComponent.DriverNormalizedValueY;
	}
}// EOF CLASS
