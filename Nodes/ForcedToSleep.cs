using System;
using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class ForcedToSleep : PhysicsSystemComponent, IPhysicsComponent
{
    [Export(PropertyHint.GroupEnable), ExportGroup("Linear Tolerance")] private bool linearEnabled = false;
    [Export(PropertyHint.Range, " 0.001, 3.000, 0.001")] float linear = 0.01f;
    [Export(PropertyHint.GroupEnable), ExportGroup("Angular Tolerance")] private bool angularEnabled = false;
    [Export(PropertyHint.Range, " 0.001, 3.000, 0.001")] float angular = 0.01f;

    public override void _Ready()
	{
		if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
		{
			physicsSystem.RegisterPhysicsComponent(this);
		}
	}
    public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialGlobalTransform)
    {
        if(!state.Sleeping){
            if(linearEnabled){
                if (state.LinearVelocity.Length() <= linear) { SetToSleep(state); }
            }else if(angularEnabled){
                if (state.AngularVelocity.Length() <= angular) { SetToSleep(state); }
            }
        }
    }

    private void SetToSleep(PhysicsDirectBodyState3D state)
    {
        GD.Print("Forced to sleep");
        state.LinearVelocity = Vector3.Zero;
        state.AngularVelocity = Vector3.Zero;
        state.Sleeping = true;
    }
}// EOF CLASS
