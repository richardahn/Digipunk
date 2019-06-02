using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchArcRenderer : MonoBehaviour
{
    // Parameters
    [SerializeField]
    protected Vector2 initialVelocity;
    [SerializeField]
    protected float projectileSpeed = 10f;
    [SerializeField]
    protected Vector2 acceleration = new Vector2(0f, -3f);
    [SerializeField]
    protected float timeStep = 0.1f;
    [SerializeField]
    protected int maxPoints = 30;
    [SerializeField]
    protected float maxHeight = 5f;

    // Layers
    [SerializeField]
    protected LayerMask groundLayer;

    // State

    public List<Vector3> points;
    public Vector2 origin;

    // Dependents
    protected LineRenderer lr;

    // Lifecycle
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void SetParameters(Vector2 initialVelocityDirection, Vector2 origin)
    {
        this.initialVelocity = initialVelocityDirection * projectileSpeed;
        this.origin = origin;
    }

    private void Update()
    {
        Render();
        points.Clear();
    }

    protected void Render()
    {
        // Add point and initialise recursion
        float startTime = 0f;
        Vector2 point1 = ArcDisplacement(initialVelocity, startTime, acceleration);
        points.Add(point1);
        RenderStep(point1, startTime);

        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }
    protected void RenderStep(Vector2 point1, float time1)
    {
        // Point1 is the start point, so calculate end point which is one time step forward
        float time2 = time1 + timeStep;
        Vector2 point2 = ArcDisplacement(initialVelocity, time2, acceleration);
        
        // Linecast between the two points
        RaycastHit2D hit = Physics2D.Linecast(point1, point2, groundLayer.value);
        

        if (hit.collider == null)
        {
            // If no hit, continue the arc
            points.Add(point2);
            if (points.Count > maxPoints)
                return;
            RenderStep(point2, time2);
        }
        else
        {
            // If hit, add the collision point and finish
            points.Add(hit.point);
            return;
        }
    }
    private Vector2 ArcDisplacement(Vector2 initialVelocity, float time, Vector2 acceleration)
    {
        float x = (initialVelocity.x * time) + (0.5f * acceleration.x * time * time);
        float y = (initialVelocity.y * time) + (0.5f * acceleration.y * time * time);
        return origin + new Vector2(x, y);
    }
    
    // TODO: Calculate a better aiming method: I originally thought to keep the height and acceleration fixed, then use the max height formula for trajectory to calculate v_y, then calculate time, but there's two possible times

}
