using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 Clamps position in local space relative to the initial global transform<br/>
/// Only does Y axis now and will control linear velocity Y fully
/// </summary>
[GlobalClass]
public partial class PositionDriver : PhysicsSystemComponent, IPhysicsComponent
{

	[Export] float maxPosition = 0.0f;
	[Export] float minPosition = 0.0f;

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
		Vector3 localLinearVelocity = initialGlobalTransform.Basis.Inverse() * state.LinearVelocity;
		influence = Vector3.Zero;
		Vector3 projectedPosition = localTransform.Origin + localLinearVelocity * state.Step;

		float targetPos = float.Lerp(minPosition, maxPosition, MonitoredDriverNormalizedValue());
		float posShift = targetPos - projectedPosition.Y;

		influence.Y = posShift / state.Step;

		state.LinearVelocity = initialGlobalTransform.Basis * (localLinearVelocity + influence);
	}
}// EOF CLASS
