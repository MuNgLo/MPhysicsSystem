using Godot;

namespace MPhysicsSystem;

[GlobalClass]
public partial class PhysicsSystemComponent : Node
{
    public float Speed = 1.0f;

    public virtual Vector3 RotationDelta { get; set; } = Vector3.Zero;
    public virtual float DriverValueX { get; } = 0.0f;
    public virtual float DriverValueY { get; } = 0.0f;
    public virtual float DriverValueZ { get; } = 0.0f;

    protected RigidBody3D body;

}// EOF CLASS
