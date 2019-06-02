using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : MonoBehaviour
{
    // Parameters
    [SerializeField]
    protected Vector2 velocity;
    [SerializeField]
    protected float duration;

    // Dependents
    protected Rigidbody2D rb;

    // Public
    public void SetParameters(Vector2 velocity, float duration)
    {
        this.velocity = velocity;
        this.duration = duration;
    }
    // Lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

}
