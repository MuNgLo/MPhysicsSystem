using Godot;

namespace MPhysicsSystem;

[GlobalClass]
public partial class PhysicsSystemComponent : Node
{
	[Export] protected bool debug = false;
	[Export] protected PhysicsSystemComponent monitoredComponent;
	[Export] protected MONITOREDAXIS monitoredAxis = MONITOREDAXIS.Y;
	[Export] protected float sensitivity = 1.0f;
	[Export] protected bool useRelaySpeedAsMultiplier = false;

	[Export] protected PhysicsSystemComponent deltaProvider;



	public float Speed = 1.0f;

	public virtual Vector3 RotationDelta { get; set; } = Vector3.Zero;
	public virtual float DriverNormalizedValueX { get; } = 0.0f;
	public virtual float DriverNormalizedValueY { get; } = 0.0f;
	public virtual float DriverNormalizedValueZ { get; } = 0.0f;

	protected RigidBody3D body;
	protected Vector3 influence = Vector3.Zero;


	protected float MonitoredDriverNormalizedValue()
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
	/// <summary>
	/// Returns the targeted DeltaProvider's rotation along the MonitoredAxis<br/>
	/// multiplied with sensitivity;
	/// </summary>
	/// <returns></returns>
	protected float DeltaProviderRotation()
	{
		switch (monitoredAxis)
		{
			case MONITOREDAXIS.X:
				return deltaProvider.RotationDelta.X * sensitivity;
			case MONITOREDAXIS.Z:
				return deltaProvider.RotationDelta.Z * sensitivity;
		}
		return deltaProvider.RotationDelta.Y * sensitivity;
	}
	/// <summary>
	/// Returns the monitored component's rotation delta of the monitored Axis,<br/>
	/// multiplied with sensitivity. If useRelaySpeedAsMultiplier is True, it will also be multiplied with<br/>
	/// monitoredComponent's Speed.
	/// </summary>
	/// <returns></returns>
	protected float MonitoredRotationDelta()
	{
		switch (monitoredAxis)
		{
			case MONITOREDAXIS.X:
				if (useRelaySpeedAsMultiplier)
				{
					return monitoredComponent.RotationDelta.X * sensitivity * monitoredComponent.Speed;
				}
				return monitoredComponent.RotationDelta.X * sensitivity;
			case MONITOREDAXIS.Z:
				if (useRelaySpeedAsMultiplier)
				{
					return monitoredComponent.RotationDelta.Z * sensitivity * monitoredComponent.Speed;
				}
				return monitoredComponent.RotationDelta.Z * sensitivity;
		}
		if (useRelaySpeedAsMultiplier)
		{
			return monitoredComponent.RotationDelta.Y * sensitivity * monitoredComponent.Speed;
		}
		return monitoredComponent.RotationDelta.Y * sensitivity;
	}

	protected Vector3 MonitoredAxisAsVector()
	{
		switch (monitoredAxis)
		{
			case MONITOREDAXIS.X:
				return Vector3.Right;
			case MONITOREDAXIS.Z:
				return Vector3.Forward;
		}
		return Vector3.Up;
	}
	protected Vector3 MonitoredAxisAsVector(MONITOREDAXIS axis)
	{
		switch (axis)
		{
			case MONITOREDAXIS.X:
				return Vector3.Right;
			case MONITOREDAXIS.Z:
				return Vector3.Forward;
		}
		return Vector3.Up;
	}
	/// <summary>
	/// Resolves nad returns the Vector3<br/>
	/// X being Right
	/// </summary>
	/// <param name="axis"></param>
	/// <returns></returns>
	protected Vector3 DrivenAxis(MONITOREDAXIS axis)
	{
		switch (axis)
		{
			case MONITOREDAXIS.X:
				return Vector3.Right;
			case MONITOREDAXIS.Z:
				return Vector3.Forward;
		}
		return Vector3.Up;
	}
}// EOF CLASS
