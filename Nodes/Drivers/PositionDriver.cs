using System;
using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class PositionDriver : PhysicsSystemComponent, IPhysicsComponent
{
    [Export] PhysicsSystemComponent monitoredComponent;
    [Export] MONITOREDAXIS monitoredAxis = MONITOREDAXIS.Y;

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
        localTransform.Origin.Y = minPosition + (maxPosition - minPosition) * MonitoredValue();
        state.Transform = initialGlobalTransform * localTransform;
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
                return monitoredComponent.DriverValueX;
            case MONITOREDAXIS.Z:
                return monitoredComponent.DriverValueZ;
        }
        return monitoredComponent.DriverValueY;
    }
}// EOF CLASS
