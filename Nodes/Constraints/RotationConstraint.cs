using Godot;
namespace MPhysicsSystem;
/// <summary>
/// 0.5 Working but has drift. Needs more work
/// </summary>
[GlobalClass]
public partial class RotationConstraint : PhysicsSystemComponent, IPhysicsComponent
{
    [Export] private protected bool lockX = true;
    [Export] private protected bool lockY = true;
    [Export] private protected bool lockZ = true;

    [Export(PropertyHint.Range, "-50000, 50000, 0.01, radians_as_degrees, suffix:°/s")] public Vector3 MinAngle { get; set; } = new(0, 0, 0);
    [Export(PropertyHint.Range, "-50000, 50000, 0.01, radians_as_degrees, suffix:°/s")] public Vector3 MaxAngle { get; set; } = new(0, 0, 0);
    
	public Vector3 CurrentAngleDegrees => new(Mathf.RadToDeg(cumulativeLocalRotation.X), Mathf.RadToDeg(cumulativeLocalRotation.Y), Mathf.RadToDeg(cumulativeLocalRotation.Z));

    // Tracks accumulated rotation per local axis (radians)
    private Vector3 cumulativeLocalRotation;

    public Vector3 angelSpan = Vector3.Zero;
    /// <summary>
    /// Returns a normalized value of its position between min and max of the contains on Y local axis
    /// </summary>
    public override float DriverNormalizedValueY => Mathf.Abs(Mathf.RadToDeg(cumulativeLocalRotation.Y) + MinAngle.Y) / angelSpan.Y;

    public override void _Ready()
    {
        if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
        {
            cumulativeLocalRotation = Vector3.Zero;
            physicsSystem.RegisterPhysicsComponent(this);
            angelSpan = MaxAngle - MinAngle;
        }
    }
	Vector3 lastEuler = Vector3.Zero;
    public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialGlobalTransform)
    {
		Transform3D localTransform = initialGlobalTransform.Inverse() * state.Transform;
        Vector3 localAngVel = initialGlobalTransform.Basis.Inverse() * state.AngularVelocity;
        //cumulativeLocalRotation += localAngVel * state.Step;
        cumulativeLocalRotation += lastEuler - localTransform.Basis.GetEuler();
        lastEuler = localTransform.Basis.GetEuler();

        bool needsClamp = false;

        // Check limits per axis
        for (int i = 0; i < 3; i++)
        {
            // Skip non constrained axis
            if (i == 0 && !lockX) { continue; }
            if (i == 1 && !lockY) { continue; }
            if (i == 2 && !lockZ) { continue; }
            float val = cumulativeLocalRotation[i];
            float min = MinAngle[i];
            float max = MaxAngle[i];
            float correction = 0f;

			if(max - min < float.Epsilon)
			{
				localAngVel[i] = 0.0f;
				continue;
			}
            if (val < min)
            {
                correction = min - val;
                needsClamp = true;
            }
            else if (val > max)
            {
                correction = max - val;
                needsClamp = true;
            }

            if (correction != 0)
            {
				localAngVel[i] = correction / state.Step;
            }
        }

        // 5. Update Global Angular Velocity from modified Local Velocity
        if (needsClamp)
        {
            state.AngularVelocity += initialGlobalTransform.Basis * localAngVel;
        }
    }
}// EOF CLASS
