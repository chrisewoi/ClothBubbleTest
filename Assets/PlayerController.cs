using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; // Rate of acceleration

    public float maxSpeed; // Is a soft cap, can go over but will try to reduce to cap over time (maxSpeedSmoothTime)
    public float maxSpeedHardCap;
    public float maxSpeedSmoothTime;

    public float groundedStopTime;
    public Vector3 currentVelocity;

    public float jumpAmount;
    public float xMove;
    public bool grounded;
    public Rigidbody rb;
    public Collider playerCollider;
    private float distToGround;

    public float gravity;
    public float gravityMultPlayer; // the gravity mult that player modifies
    public float gravityMultAbility; // the gravity mult that abilities modify

    public bool moveEnabled;

    public WandAbility wandAbility;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = rb.GetComponent<Collider>();
        wandAbility = GetComponent<WandAbility>();
        distToGround = playerCollider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        if (grounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("jump");
            rb.AddForce(jumpAmount * Vector3.up, ForceMode.Impulse);
        }
        
    }

    void FixedUpdate()
    {
        //Debug.Log("Player velocity: " + rb.velocity);


        Vector3 moveVector = xMove * speed * Time.fixedDeltaTime * Vector3.right;

        // player x movement (if enabled) !MOVEMENT!
        if (moveEnabled) rb.AddForce(moveVector, ForceMode.Force);

        // Slow player to stop when no input on ground
        if (grounded && xMove == 0 && !Input.GetButton("Jump"))
        { 
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, groundedStopTime);
        }


        // Change player direction at consistent speed with player stop
            // Right to left
        if (grounded && xMove == -1 && !Input.GetButton("Jump") && rb.velocity.x > 0)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, groundedStopTime);
        }
            // Left to right
        if (grounded && xMove == 1 && !Input.GetButton("Jump") && rb.velocity.x < 0)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, Vector3.zero, ref currentVelocity, groundedStopTime);
        }


        // Increase gravity if falling (no jump held)
        if (!grounded && !Input.GetButton("Jump"))
        {
            gravityMultPlayer += Time.deltaTime;
        } else
        {
            gravityMultPlayer = 1;
        }




        // Clamp speed (soft clamp)
        if (grounded && rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, rb.velocity.normalized * maxSpeed, ref currentVelocity, maxSpeedSmoothTime);
        }

        // Max speed hard cap
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedHardCap);

        // APPLY GRAVITY (gravity can exceed hard cap)
        float gravityMult = gravityMultPlayer * gravityMultAbility;
        rb.AddForce(gravity * gravityMult * Vector3.down, ForceMode.Force);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") && Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f))
        {
            grounded = true;
            // Reset wandAbility uses
            wandAbility.remainingUses = wandAbility.maxUses;
        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    public void SetGravityMultAbility(float mult)
    {
        gravityMultAbility = mult;
    }
    public void SetMoveEnabled(bool enabled)
    {
        moveEnabled = enabled;
    }
}
