using Godot;
namespace MPhysicsSystem;

[GlobalClass]
public partial class RotationConstraint : PhysicsSystemComponent, IPhysicsComponent
{
    [Export] private protected bool lockX = true;
    [Export] private protected bool lockY = true;
    [Export] private protected bool lockZ = true;

    [Export] public Vector3 MinAngleDegrees { get; set; } = new(0, 0, 0);
    [Export] public Vector3 MaxAngleDegrees { get; set; } = new(0, 0, 0);
    public Vector3 CurrentAngleDegrees => new(Mathf.RadToDeg(_unwrappedAngles.X), Mathf.RadToDeg(_unwrappedAngles.Y), Mathf.RadToDeg(_unwrappedAngles.Z));

    // Tracks accumulated rotation per local axis (radians)
    private Vector3 _unwrappedAngles;

    // The initial basis captured on entry
    private Basis _initialBasis;

    public Vector3 angelSpan = Vector3.Zero;
    /// <summary>
    /// Returns a normalized value of its position between min and max of the constains on Y local axis
    /// </summary>
    public override float DriverValueY => Mathf.Abs(Mathf.RadToDeg(_unwrappedAngles.Y) + MinAngleDegrees.Y) / angelSpan.Y;


    public override void _Ready()
    {
        if (GetParent<RigidBody3D>() is IPhysicsSystem physicsSystem)
        {
            _initialBasis = physicsSystem.Transform.Basis;
            _unwrappedAngles = Vector3.Zero;
            physicsSystem.RegisterPhysicsComponent(this);
            angelSpan = MaxAngleDegrees - MinAngleDegrees;
        }
    }

    public void IntegrateForces(PhysicsDirectBodyState3D state, Transform3D initialTransform)
    {
        // 1. Get Angular Velocity in LOCAL space
        // Transform global angular velocity to local space to track rotation relative to object axes
        Vector3 localAngVel = state.Transform.Basis.Inverse() * state.AngularVelocity;
        //Vector3 localAngVel = initialTransform.Basis.Inverse() * state.AngularVelocity; // tested and no go

        // 2. Integrate to update unwrapped angles
        _unwrappedAngles += localAngVel * state.Step;

        // 3. Convert limits to radians
        Vector3 minRad = MinAngleDegrees * Mathf.DegToRad(1.0f);
        Vector3 maxRad = MaxAngleDegrees * Mathf.DegToRad(1.0f);

        Vector3 correctionAxis = Vector3.Zero;
        //float correctionAngle = 0f;
        bool needsClamp = false;

        // 4. Check limits per axis
        for (int i = 0; i < 3; i++)
        {
            // Skip non constrained axis
            if (i == 0 && !lockX) { continue; }
            if (i == 1 && !lockY) { continue; }
            if (i == 2 && !lockZ) { continue; }
            float val = _unwrappedAngles[i];
            float min = minRad[i];
            float max = maxRad[i];
            float correction = 0f;

            if (val < min)
            {
                correction = min - val;
                _unwrappedAngles[i] = min; // Sync tracker
                needsClamp = true;
            }
            else if (val > max)
            {
                correction = max - val;
                _unwrappedAngles[i] = max; // Sync tracker
                needsClamp = true;
            }

            if (correction != 0)
            {
                // Accumulate correction for this axis
                // We construct a correction vector where only the clamped axis has a value
                Vector3 axisVector = Vector3.Zero;
                axisVector[i] = 1.0f;

                // Create a basis rotation for this specific axis correction
                Basis axisCorrection = new Basis(axisVector, correction);

                // Apply to the state transform immediately (order matters: Local Axis Rotation)
                // Rotating around LOCAL axis: newBasis = oldBasis * axisCorrection
                state.Transform = new Transform3D(state.Transform.Basis * axisCorrection, state.Transform.Origin);
                //state.Transform = new Transform3D(initialTransform.Basis * axisCorrection, initialTransform.Origin); // tested and no go

                // Zero the specific local angular velocity component
                localAngVel[i] = 0.0f;
            }
        }

        // 5. Update Global Angular Velocity from modified Local Velocity
        if (needsClamp)
        {
            state.AngularVelocity = state.Transform.Basis * localAngVel;
            //state.AngularVelocity = initialTransform.Basis * localAngVel; // tested and no go
        }
    }
}// EOF CLASS
