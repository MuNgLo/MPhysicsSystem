using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class PositionConstraint : PhysicsSystemComponent, IPhysicsComponent
{
	[Export] private protected Vector3 minPosition = Vector3.Zero;
	[Export] private protected Vector3 maxPosition = Vector3.Zero;

	[Export] private protected bool lockX = false;
	[Export] private protected bool lockY = false;
	[Export] private protected bool lockZ = false;

	private protected Vector3 ogLocalPosition;

	public override void _Ready()
	{
		if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
		{
			ogLocalPosition = physicsSystem.Position;
			physicsSystem.RegisterPhysicsComponent(this);
		}
	}

	public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialTransform)
	{
		Transform3D tr = initialTransform.Inverse() * state.Transform;

		if (lockX)
		{
			if (tr.Origin.X < minPosition.X) { tr.Origin.X = minPosition.X; state.LinearVelocity -= state.LinearVelocity.Project(-tr.Basis.X); }
			else if (tr.Origin.X > maxPosition.X) { tr.Origin.X = maxPosition.X; state.LinearVelocity -= state.LinearVelocity.Project(tr.Basis.X); }
		}
		if (lockY)
		{
			if (tr.Origin.Y < minPosition.Y) { tr.Origin.Y = minPosition.Y; state.LinearVelocity -= state.LinearVelocity.Project(-tr.Basis.Y); }
			else if (tr.Origin.Y > maxPosition.Y) { tr.Origin.Y = maxPosition.Y; state.LinearVelocity -= state.LinearVelocity.Project(tr.Basis.Y); }
		}
		if (lockZ)
		{
			if (tr.Origin.Z < minPosition.Z) { tr.Origin.Z = minPosition.Z; state.LinearVelocity -= state.LinearVelocity.Project(-tr.Basis.Z); }
			else if (tr.Origin.Z > maxPosition.Z) { tr.Origin.Z = maxPosition.Z; state.LinearVelocity -= state.LinearVelocity.Project(tr.Basis.Z); }
		}
		state.Transform = initialTransform * tr;
	}
}// EOF CLASS
