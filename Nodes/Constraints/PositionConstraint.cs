using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 Works fine with LocalLinearVelocity and projected position to constrain position for next tick<br/>
/// remember that the order of components in the tree matters. Constraints should be at the bottom so they run last<br/>
/// At least, they should usually be last.
/// </summary>
[GlobalClass]
public partial class PositionConstraint : PhysicsSystemComponent, IPhysicsComponent
{
	[Export] private protected bool lockX = true;
	[Export] private protected bool lockY = true;
	[Export] private protected bool lockZ = true;
	[Export] private Vector3 posMax { get; set; } = Vector3.Zero;
	[Export] private Vector3 posMin { get; set; } = Vector3.Zero;


	public override void _Ready()
	{
		if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
		{
			physicsSystem.RegisterPhysicsComponent(this);
		}
	}
	public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialGlobalTransform)
	{
		Transform3D localTransform = initialGlobalTransform.Inverse() * state.Transform;
		Vector3 localLinearVelocity = initialGlobalTransform.Basis.Inverse() * state.LinearVelocity;
		influence = Vector3.Zero;
		Vector3 projectedPosition = localTransform.Origin + localLinearVelocity * state.Step;

		if (lockX)
		{
			if (projectedPosition.X < posMin.X)
			{
				influence.X = (posMin.X - projectedPosition.X) / state.Step;
			}
			else if (localTransform.Origin.X > posMax.X)
			{
				influence.X = (posMin.X - projectedPosition.X) / state.Step;
			}
		}
		if (lockY)
		{
			if (projectedPosition.Y < posMin.Y)
			{
				influence.Y = (posMin.Y - projectedPosition.Y) / state.Step;
			}
			else if (projectedPosition.Y > posMax.Y)
			{
				influence.Y = (posMax.Y - projectedPosition.Y) / state.Step;
			}
		}
		if (lockZ)
		{
			if (projectedPosition.Z < posMin.Z)
			{
				influence.Z = (posMin.Z - projectedPosition.Z) / state.Step;
			}
			else if (projectedPosition.Z > posMax.Z)
			{
				influence.Z = (posMax.Z - projectedPosition.Z) / state.Step;
			}
		}
		state.LinearVelocity = initialGlobalTransform.Basis * (localLinearVelocity + influence);
	}
}// EOF CLASS
