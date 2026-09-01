using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 Working and uses velocities
/// </summary>
[GlobalClass]
public partial class PositionPingPongDriver : PhysicsSystemComponent, IPhysicsComponent
{
	[Export] MONITOREDAXIS drivenAxis = MONITOREDAXIS.Y;
	[Export] float maxPosition = 0.0f;
	[Export] float minPosition = 0.0f;

	bool reversed = false;
	float tickValue = 0.0f;
	float axisPosition = 0.0f;

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
		Transform3D localTransform = initialGlobalTransform.Inverse() * state.Transform;
		Vector3 localLinearVelocity = initialGlobalTransform.Basis.Inverse() * state.LinearVelocity - influence;
		influence = Vector3.Zero;
		Vector3 projectedPosition = localTransform.Origin + localLinearVelocity * state.Step;

		axisPosition = projectedPosition[(int)drivenAxis];

		if (axisPosition < minPosition) { reversed = true; }
		if (axisPosition > maxPosition) { reversed = false; }

		if (!reversed)
		{
			influence = -MonitoredAxisAsVector(drivenAxis) * MonitoredRotationDelta() / state.Step;
		}
		else
		{
			influence = MonitoredAxisAsVector(drivenAxis) * MonitoredRotationDelta() / state.Step;
		}
		if (debug)
		{
			GD.Print($"axisPosition[{axisPosition}] reversed[{reversed}] influence[{influence}]");
		}

		localLinearVelocity.X = 0.0f;

		state.LinearVelocity = initialGlobalTransform.Basis * (localLinearVelocity + influence);
	}
}// EOF CLASS
