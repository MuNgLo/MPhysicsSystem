using Godot;

namespace MPhysicsSystem;

[GlobalClass]
public partial class PhysicsSystemComponent : Node
{
    protected RigidBody3D body;

    public virtual float DriverValueX { get; } = 0.0f;
    public virtual float DriverValueY { get; } = 0.0f;
    public virtual float DriverValueZ { get; } = 0.0f;
}
