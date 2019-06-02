using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // Parameters
    [Header("Speed Settings")]
    [SerializeField]
    protected float maxSpeed = 7f;
    [Header("Grounded Settings")]
    [SerializeField]
    protected float groundAcceleration = 100f;
    [SerializeField]
    protected float groundDeceleration = 100f;
    [SerializeField]
    protected float groundStickModifier = 3f;
    [Header("Airborne Settings")]
    [SerializeField]
    public float gravity = 38f;
    [SerializeField]
    protected float jumpSpeed = 16.5f;
    [SerializeField]
    protected float jumpAbortDeceleration = 100f;
    [SerializeField]
    protected float fallingDeceleration = 100f;
    [SerializeField]
    protected float maxFallSpeed = 30f;
    [Range(0f, 1f)]
    [SerializeField]
    protected float horizontalAccelerationModifier = 0.5f;
    [Range(0f, 1f)]
    [SerializeField]
    protected float horizontalDecelerationModifier = 1.0f;
    [Header("Facing Direction Settings")]
    [SerializeField]
    protected bool originallyFacingRight = true;
    [Header("Ground/Ceiling Check Settings")]
    [SerializeField]
    protected float groundCheckDistance = 0.1f;
    [SerializeField]
    protected float ceilingCheckDistance = 0.1f;
    [SerializeField]
    protected LayerMask groundLayer;

    // Dependents
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Collider2D col;
    protected CharacterInput input;

    // State
    protected bool HasHorizontalMovement { get { return Mathf.Abs(input.Horizontal.Value) > 0f; } }
    [SerializeField]
    protected Vector2 velocity;
    public bool IsGrounded { get; protected set; }
    public bool IsCeilinged { get; protected set; }

    // Animator Parameter Hashes
    protected readonly int horizontalVelocityParameterHash = Animator.StringToHash("HorizontalVelocity");
    protected readonly int verticalVelocityParameterHash = Animator.StringToHash("VerticalVelocity");
    protected readonly int groundedParameterHash = Animator.StringToHash("Grounded");

    // Rotation constants
    protected readonly Quaternion originalRotation = Quaternion.AngleAxis(0, Vector2.up);
    protected readonly Quaternion flippedRotation = Quaternion.AngleAxis(180, Vector2.up);
    protected Quaternion RightRotation { get { return originallyFacingRight ? originalRotation : flippedRotation; } }
    protected Quaternion LeftRotation { get { return originallyFacingRight ? flippedRotation : originalRotation; }  }


    // Lifecycle
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        input = GetComponent<CharacterInput>();
    }
    private void Update()
    {
        UpdateHorizontalMovement();
        UpdateVerticalMovement();
        UpdateFacing();
        UpdateGrounded(); // I believe Grounded should be updated in Update() as there is a delay with checking it in FixedUpdate()
        UpdateCeilinged();
    }
    private void FixedUpdate()
    {
        // Move
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        // Animate
        anim.SetFloat(horizontalVelocityParameterHash, velocity.x);
        anim.SetFloat(verticalVelocityParameterHash, velocity.y);
    }

    // Public 
    // void Move(InputAxis)
    // Check if input was pressed bool Jump(InputKeyStatus) 
    
    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }
    public void IncrementVelocity(Vector2 velocity)
    {
        this.velocity += velocity;
    }
    public void FaceRight(bool right)
    {
        transform.rotation = right ? RightRotation : LeftRotation;
    }

    // Internal
    protected void UpdateGrounded()
    {
        IsGrounded = (Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, groundCheckDistance, groundLayer.value).collider != null);
        //IsGrounded = (Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0f, Vector2.down, groundCheckDistance, groundLayer.value).collider != null);
        anim.SetBool(groundedParameterHash, IsGrounded);
    }
    protected void UpdateCeilinged()
    {
        IsCeilinged = (Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.up, ceilingCheckDistance, groundLayer.value).collider != null);
    }
    protected void UpdateHorizontalMovement() // Move this to state too
    {
        float desiredSpeed = input.Horizontal.Value * maxSpeed;
        float acceleration;
        if (!IsGrounded)
        {
            acceleration = HasHorizontalMovement ? (groundAcceleration * horizontalAccelerationModifier) : (groundDeceleration * horizontalDecelerationModifier);
        }
        else
        {
            acceleration = HasHorizontalMovement ? groundAcceleration : groundDeceleration;
        }
        velocity.x = Mathf.MoveTowards(velocity.x, desiredSpeed, acceleration * Time.deltaTime);
    }
    protected void UpdateVerticalMovement()
    {
        // [Incremental] If jump was let go prematurely, shorten the jump 
        if (!input.Jump.Held && (velocity.y > 0f))
            velocity.y -= jumpAbortDeceleration * Time.deltaTime;

        // [Incremental] If falling, apply additional gravity
        if (velocity.y < 0f)
            velocity.y -= fallingDeceleration * Time.deltaTime;

        // [Incremental] Apply gravity
        velocity.y -= gravity * Time.deltaTime;

        // [Assign] Apply a stick factor while grounded so you don't float for a bit while walking off cliffs
        if (IsGrounded)
            velocity.y = Mathf.Max(velocity.y, -gravity * Time.deltaTime * groundStickModifier);

        // [Assign] Don't let the character stick to ceilings, only when the character is attempting to move up
        if (IsCeilinged && velocity.y > 0f)
            velocity.y = 0f;

        // [Assign] Clamp fall speed
        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);

        // [Assign] Jump
        if (IsGrounded && input.Jump.Pressed)
            velocity.y = jumpSpeed;
    }
    protected void UpdateFacing()
    {
        if (velocity.x > 0) // Moving right
        {
            transform.rotation = RightRotation;
        }
        else if (velocity.x < 0) // Moving left
        {
            transform.rotation = LeftRotation;
        }
    }

}
