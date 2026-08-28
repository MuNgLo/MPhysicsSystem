using System;
using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class PositionPingPongDriver : PhysicsSystemComponent, IPhysicsComponent
{
    [Export] PhysicsSystemComponent monitoredComponent;
    [Export] MONITOREDAXIS monitoredAxis = MONITOREDAXIS.Y;
    [Export] MONITOREDAXIS drivenAxis = MONITOREDAXIS.Y;

    [Export] float maxPosition = 0.0f;
    [Export] float minPosition = 0.0f;
    [Export] float sensitivity = 1.0f;
    [Export] bool useRelaySpeedAsMultiplier = false;



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


    bool reversed = false;
    float tickValue = 0.0f;
    float axixTickValue = 0.0f;

    public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D otherTR)
    {
        Transform3D localTransform = initialTransform.Inverse() * state.Transform;

        axixTickValue = ResolveAxisValue(localTransform);

        if (!reversed)
        {
            tickValue = axixTickValue + MonitoredDeltaValue();
        }
        else
        {
            tickValue = axixTickValue - MonitoredDeltaValue();
        }
        if (tickValue < minPosition || tickValue > maxPosition) { reversed = !reversed; }

        localTransform.Origin[(int)drivenAxis] = Mathf.Clamp(tickValue, minPosition, maxPosition);

        state.Transform = initialTransform * localTransform;
    }

    private float ResolveAxisValue(Transform3D localTransform)
    {
        switch (drivenAxis)
        {
            case MONITOREDAXIS.X:
                return localTransform.Origin.X;
            case MONITOREDAXIS.Z:
                return localTransform.Origin.Z;
        }
        return localTransform.Origin.Y;
    }

    float AxisPosition()
    {
        switch (monitoredAxis)
        {
            case MONITOREDAXIS.X:
                return body.Position.X;
            case MONITOREDAXIS.Z:
                return body.Position.Z;
        }
        return body.Position.Y;
    }

    float MonitoredNormalizedValue()
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
    float MonitoredDeltaValue()
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
}// EOF CLASS
