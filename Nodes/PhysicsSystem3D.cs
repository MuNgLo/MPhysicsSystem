using System.Collections.Generic;
using Godot;

namespace MPhysicsSystem;

/// <summary>
/// Minimal implementation of a MPhysicsSystem
/// </summary>
[GlobalClass]
public partial class PhysicsSystem3D : RigidBody3D, IPhysicsSystem
{
    [Export] bool debug = false;
    protected List<IPhysicsComponent> components;
    protected Transform3D initialTransform;

    /// <summary>
    /// Initialize teh component collection when entering the tree<br/>
    /// Components should register in under _Ready()
    /// </summary>
    public override void _EnterTree()
    {
        components = new List<IPhysicsComponent>();
        initialTransform = GlobalTransform;
    }
    /// <summary>
    /// Called when physics simulates the body. So here it is branching to all components.
    /// </summary>
	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
        //if (debug) { GD.Print($"System updating [{components.Count}] components"); }
        for (int i = 0; i < components.Count; i++)
		{
			components[i].IntegrateForces(state, initialTransform);
		}
	}
	/// <summary>
	/// Allows registration of components into the system
	/// </summary>
	public void RegisterPhysicsComponent(IPhysicsComponent component)
	{
        if (debug) { GD.Print($"PhysicsSystem3D::RegisterPhysicsComponent() Registering [{component.Name}] [{component.GetPath()}]"); }
        components.Add(component);
	}
}// EOF CLASS
