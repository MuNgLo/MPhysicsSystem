using Godot;
namespace MPhysicsSystem;

/// <summary>
/// Anything implementing this can be registered into an IPhysicsSystem
/// </summary>
public interface IPhysicsComponent
{
    StringName Name { get; set; }
    NodePath GetPath();
    float DriverNormalizedValueX { get; }
    float DriverNormalizedValueY { get; }
    float DriverNormalizedValueZ { get; }
    /// <summary>
    /// To get the up to date local transfrom from the initialTransform and state do...<br/>
    /// <br/>
    /// Transform3D localTransform = initialTransform.Inverse() * state.Transform;<br/>
    /// Remember to do that in this method since other components will have changed state so it needs to recalculated<br/>
    /// to be useful in this component
    /// </summary>
    void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialGlobalTransform);
}// EOF INTERFACE
