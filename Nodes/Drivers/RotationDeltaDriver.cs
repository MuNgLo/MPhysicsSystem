using System;
using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationDeltaDriver : PhysicsSystemComponent, IPhysicsComponent
{
	[Export] PhysicsSystemComponent monitoredComponent;
	[Export] MONITOREDAXIS monitoredAxis = MONITOREDAXIS.Y;
	[Export] MONITOREDAXIS drivenAxis = MONITOREDAXIS.Y;

	[Export] float sensitivity = 1.0f;


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
		state.Transform = initialTransform * localTransform.RotatedLocal(DrivenAxis(), MonitoredValue() * sensitivity);
	}

	private Vector3 DrivenAxis()
	{
		switch (drivenAxis)
		{
			case MONITOREDAXIS.X:
				return Vector3.Right;
			case MONITOREDAXIS.Z:
				return Vector3.Forward;
		}
		return Vector3.Up;
	}

	float AxisPosition()
	{
		switch (monitoredAxis)
		{
			case MONITOREDAXIS.X:
				return body.Position.Y;
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
				return monitoredComponent.RotationDelta.X;
			case MONITOREDAXIS.Z:
				return monitoredComponent.RotationDelta.Z;
		}
		return monitoredComponent.RotationDelta.Y;
	}
}// EOF CLASS
