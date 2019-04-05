using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // Movement settings
    public float movementSpeed = 30f;
    public float jumpingMovementSpeed = 1.5f;
    public float jumpForce = 10f;
    public bool defaultDirectionLeft = true;
    public float groundCheckDistance = 5f;

    // Parameter references
    public readonly string groundLayer = "Ground";
    public readonly string animationMovementParameter = "Speed";
    public readonly string animationJumpParameter = "Jump";
    public readonly string animationGroundedParameter = "Grounded";

    // Required components
    private Animator animator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    // Mover inputs
    private float horizontalMovement;
    private bool pressedJump;

    // Jump state
    private bool grounded = true;


    // Must be called every frame by a Thinker
    public void Control(float horizontalMovement, bool pressedJump)
    {
        this.horizontalMovement = horizontalMovement;
        this.pressedJump = pressedJump;
    }

    void Awake()
    {
        // This is fine because this is just hooking up the reference, and not actually using anything within the RB, so it doesnt matter whether it's been initialized
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // As opposed to Awake() since we want this initialization to happen every time this mover is disabled->enabled
    void OnEnable()
    {
        horizontalMovement = 0f;
        pressedJump = false;
    }

    void Update()
    {
        // Check if we are touching the ground or not
        grounded = Physics2D.Linecast(transform.position, transform.position - new Vector3(0, -groundCheckDistance), 1 << LayerMask.NameToLayer(groundLayer)); // Put linecasts in Update as opposed to FixedUpdate why?
        animator.SetBool(animationGroundedParameter, grounded);
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }
    private void Move()
    {
        // Apply motion
        //Vector2 movement = new Vector2(horizontalMovement * (grounded ? movementSpeed : jumpingMovementSpeed) * Time.deltaTime, rigidBody.velocity.y);

        rigidBody.AddForce(new Vector2(horizontalMovement * movementSpeed, 0));
        //rigidBody.velocity = new Vector2(horizontalMovement * 10f, rigidBody.velocity.y);

        //float maxSpeed = 10f;
        //rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed), rigidBody.velocity.y);
        

        // Change direction if necessary
        if (horizontalMovement < 0)
            spriteRenderer.flipX = !defaultDirectionLeft;
        if (horizontalMovement > 0)
            spriteRenderer.flipX = defaultDirectionLeft;

        // Change animations
        animator.SetFloat(animationMovementParameter, Mathf.Abs(horizontalMovement));
    }
    private void Jump()
    {
        // Apply jump force only when grounded(to prevent jumping while in air)
        if (pressedJump && grounded)
        {
            //rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            // Change animations
            animator.SetTrigger(animationJumpParameter);
        }
    }

}
