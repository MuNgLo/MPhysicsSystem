using Godot;
namespace MPhysicsSystem;

/// <summary>
/// Will run an internal collection of IPhysicsSystemComponent<br/>
/// Then call each component's InteGratedForces in its own _integratedForces
/// </summary>
public interface IPhysicsSystem
{
	Vector3 Position { get; }
	Transform3D Transform { get; }
	Transform3D GlobalTransform { get; }
	void RegisterPhysicsComponent(IPhysicsComponent component);
}// EOF INTERFACE
