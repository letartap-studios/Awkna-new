﻿using UnityEngine;

// Script to controll the main character

public class PlayerController : MonoBehaviour
{
    #region Variables
    public float movementSpeed = 40f;           // The speed at which the player is moving.
    public float jumpForce = 400f;              // Amount of force added when the player jumps.    
    [Range(0, .3f)]
    [SerializeField]
    private float horizontalMovementSmoothing;  // How much to smooth out the horizontal movement.
    [Range(0, .3f)]
    [SerializeField]
    private float climbingSmoothing;            // How much to smooth out the climbing.

    public bool facingRight = true;            // For determining which way the player is currently facing.

    private bool isGrounded;                    // Whether or not the player is grounded.
    public Transform groundCheck;               // A position marking where to check if the player is grounded.
    public float checkRadius;                   // Radius of the overlap circle to determine if grounded.
    public LayerMask whatIsGround;              // A mask determining what is ground to the character.

    public LayerMask whatIsLadder;              // A mask determining what is ladder to the character.
    public float ladderDistance;                // Distance between the player and the ladder at which the player can climb.
    private bool isClimbing;                    // Whether the player is climbing.
    private RaycastHit2D ladderHitInfo;         // Whether above the player is ladder.
    private float initialGravity;               // The initial gravity of the character.
    public float climbSpeed = 30f;              // The speed of the character when climbing a ladder.

    private float horizontalMoveInput = 0f;     // Input for horizontal movement.
    private float verticalMoveInput;            // Input for vertical movement.

    private Rigidbody2D rb;                     // Rigidbody of the character.
    private Vector3 velocity = Vector3.zero;

    public GameObject bomb;                     // Instance of a bomb.
    public int bombsNumber;                     // The number of bombs.

    private bool top;                           // Whether the player is upside-down.

    public float energy;                        // Measures the amount of energy the player has.
    public float maxEnergy;                     // The maximum energy the player can have.
    #endregion

    private void Start()
    {
        energy = maxEnergy;
        rb = GetComponent<Rigidbody2D>();       // Get the rigidbody component from the player object.
        initialGravity = rb.gravityScale;       // Get the initial gravity of the charcter.
    }

    private void FixedUpdate()
    {
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        #region Horizontal movment

        horizontalMoveInput = Input.GetAxisRaw("Horizontal");                                             // Get the horizontal axis input.
        Vector3 targetVelocity = new Vector2(horizontalMoveInput * movementSpeed, rb.velocity.y);         // Move the character by finding the target velocity...       
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, horizontalMovementSmoothing);   // ...and then smoothing it out and applying it to the character.

        if (horizontalMoveInput > 0 && !facingRight)        // If the input is moving the player right and the player is facing left...
        {
            Flip();                                     // ... flip the player.
        }
        else if (horizontalMoveInput < 0 && facingRight)    // Otherwise if the input is moving the player left and the player is facing right...
        {
            Flip();                                     // ... flip the player.
        }

        #endregion

        #region Climbing the ladder

        // Send a raycast upwards to check if the player is on a ladder.
        ladderHitInfo = Physics2D.Raycast(transform.position, Vector2.up, ladderDistance, whatIsLadder);

        if (ladderHitInfo.collider != null)                     // Check whether the ray has collided with a ladder.
        {
            isClimbing = true;                                  // The player can climb...
        }
        else                                                    // ...else he can't.
        {
            isClimbing = false;
        }
        if (isClimbing == true)                                      // If the player is climbing...
        {
            verticalMoveInput = Input.GetAxisRaw("Vertical");        // ...get the vertical axis input and...
                                                                     // ...move the character by finding the target velocity...
            Vector3 verticalTargetVelocity = new Vector2(rb.velocity.x, verticalMoveInput * climbSpeed);
                                                                     // ...and then smoothing it out and applying it to the character.
            rb.velocity = Vector3.SmoothDamp(rb.velocity, verticalTargetVelocity, ref velocity, horizontalMovementSmoothing);

            rb.gravityScale = 0;                                     // Set the characters gravity to 0, in order to make the player climb.
        }
        else
        {
            rb.gravityScale = initialGravity;                        // Else set the gravity back to normal.
        }

        #endregion
    }

    private void Update()
    {
        #region Jump
        if (Input.GetButtonDown("Jump") && isGrounded)  // Check if the Jump button was pressed and give the players...
        {
            rb.velocity = Vector2.up * jumpForce;      //...rigidbody velocity on the y axis.
        }
        #endregion

        #region Bomb
        if (Input.GetButtonDown("Bomb") && bombsNumber > 0)
        {
            Instantiate(bomb, transform.position, Quaternion.identity);
            bombsNumber--;
        }
        #endregion

        Debug.Log(top);

        #region Switch Gravity
        if (energy > 0)                                 //Whether the player has energy
        {
            if (Input.GetButtonDown("SwitchGravity"))       // If the player has energy and the button was pressed change the gravity.
            {                                                        
                rb.gravityScale *= -1;                              
                Rotation();
                top = !top;
                facingRight = !facingRight;                             // Change the facing upon rotation
            }                                                      
        }                                                            
        #endregion

        #region Energy

        if (top == true)
        {
            energy -= Time.deltaTime;
            if(energy < 0)
            {
                energy = 0;
            }
        }
        else if(energy < maxEnergy)
        {
            energy += Time.deltaTime;
        }

        #endregion
    }

    private void Flip()
    {
        facingRight = !facingRight;                 // Switch the way the player is labelled as facing.

        Vector3 theScale = transform.localScale;    // Multiply the player's x local scale by -1.
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Rotation()
    {
        if (top == false)                                       // If the player is upside-down...
        {
            transform.eulerAngles = new Vector3(0, 0, 180f);    // ...rotate on the z axis.
        }
        else
        {
            transform.eulerAngles = Vector3.zero;               
        }

       

    }
}
