using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbedProjectile : MonoBehaviour
{
    // Parameters
    [SerializeField]
    protected Vector2 initialVelocity;
    [SerializeField]
    protected Vector2 acceleration = new Vector2(0, -3f);
    [SerializeField]
    protected float projectileSpeed = 10f;

    public bool launch = false;

    // Dependents
    protected Rigidbody2D rb;

    // Public
    public void SetParameters(Vector2 initialVelocity)
    {
        this.initialVelocity = initialVelocity * projectileSpeed;
        launch = true;
    }

    // Lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (launch)
        {
            rb.AddForce(initialVelocity, ForceMode2D.Impulse);
            launch = false;
        }

        rb.velocity += acceleration * Time.deltaTime;
    }

}
