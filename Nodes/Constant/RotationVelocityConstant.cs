using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationVelocityConstant : PhysicsSystemComponent, IPhysicsComponent
{
	[Export] PhysicsSystemRelay sourceRelay;
	[Export] public bool useRelaySpeedAsMultiplier = false;

	[Export(PropertyHint.Range, "0,1")]
	public float Dampening
	{
		get => 1.0f - dampening;
		set => dampening = 1.0f - value;
	}
	float dampening = 0.0f;

	[Export(PropertyHint.GroupEnable), ExportGroup("Local X Axis")] private bool axisX = false;
	[Export(PropertyHint.Range, "-360, 360, 0.01, radians_as_degrees, suffix:°/s")]
	public float VelocityX { get; set; } = 0.0f;
	[Export(PropertyHint.Range, "0,1")] public float complianceX = 1.0f;


	[Export(PropertyHint.GroupEnable), ExportGroup("Local Y Axis")] private bool axisY = false;
	[Export(PropertyHint.Range, "-360, 360, 0.01, radians_as_degrees, suffix:°/s")]
	public float VelocityY { get; set; } = 0.0f;
	[Export(PropertyHint.Range, "0,1")] public float complianceY = 1.0f;


	[Export(PropertyHint.GroupEnable), ExportGroup("Local Z Axis")] private bool axisZ = false;
	[Export(PropertyHint.Range, "-360, 360, 0.01, radians_as_degrees, suffix:°/s")]
	public float VelocityZ { get; set; } = 0.0f;
	[Export(PropertyHint.Range, "0,1")] public float complianceZ = 1.0f;


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
		Vector3 localAngVel = initialGlobalTransform.Basis.Inverse() * state.AngularVelocity;
		localAngVel *= dampening;

		if (axisX)
		{
			localAngVel.X = float.Lerp(localAngVel.X, useRelaySpeedAsMultiplier ? VelocityX * sourceRelay.Speed : VelocityX, complianceX);
		}

		if (axisY)
		{
			localAngVel.Y = float.Lerp(localAngVel.Y, useRelaySpeedAsMultiplier ? VelocityY * sourceRelay.Speed : VelocityY, complianceX);
		}

		if (axisZ)
		{
			localAngVel.Z = float.Lerp(localAngVel.Z, useRelaySpeedAsMultiplier ? VelocityZ * sourceRelay.Speed : VelocityZ, complianceX);
		}


		//GD.Print($"localAngVel[{localAngVel}] AngularVelocity[{AngularVelocity}]");
		state.AngularVelocity = state.Transform.Basis * localAngVel;
		//GD.Print($"state.AngularVelocity[{state.AngularVelocity}]");

	}
}// EOF CLASS
