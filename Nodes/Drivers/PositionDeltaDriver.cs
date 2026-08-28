using System;
using Godot;
namespace MPhysicsSystem;
[GlobalClass]
public partial class PositionDeltaDriver : PhysicsSystemComponent, IPhysicsComponent
{
    [Export] PhysicsSystemComponent deltaProvider;
    [Export] MONITOREDAXIS monitoredAxis = MONITOREDAXIS.Y;

    [Export] float maxPosition = 0.0f;
    [Export] float minPosition = 0.0f;
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
        Transform3D localTR = initialTransform.Inverse() * state.Transform;
        //GD.Print($"Current Y [{localTR.Origin.Y}] new [{Mathf.Clamp(localTR.Origin.Y + MonitoredValue(), minPosition, maxPosition)}] MonitoredValue[{MonitoredValue()}]");
        localTR.Origin.Y = Mathf.Clamp(localTR.Origin.Y + MonitoredValue(), minPosition, maxPosition);
        state.Transform = initialTransform * localTR;
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
                return deltaProvider.RotationDelta.X * sensitivity;
            case MONITOREDAXIS.Z:
                return deltaProvider.RotationDelta.Z * sensitivity;
        }
        return deltaProvider.RotationDelta.Y * sensitivity;
    }
}// EOF CLASS
