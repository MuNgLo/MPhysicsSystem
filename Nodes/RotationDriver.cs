using System;
using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationDriver : PhysicsSystemComponent, IPhysicsComponent
{
    [Export] PhysicsSystemComponent monitoredComponent;
    [Export] MONITOREDAXIS monitoredAxis = MONITOREDAXIS.Y;

    [Export] float MaxAngleDegree = -180.0f;
    [Export] float MinAngleDegree = 180.0f;

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
        Transform3D localTransform = initialTransform.Inverse() * state.Transform;
        float rad = Mathf.DegToRad(MinAngleDegree) + (Mathf.DegToRad(MaxAngleDegree) - Mathf.DegToRad(MinAngleDegree)) * MonitoredValue();
        state.Transform = initialTransform * new Transform3D(new Basis(Vector3.Up, rad), localTransform.Origin);
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
