using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandAbility : MonoBehaviour
{
    public float timeToActivateFloat;
    private float currentActivationTime;

    public int remainingUses;
    public int maxUses;

    public float floatSpeed;
    public float floatVelocityCap;

    public float currentFloatGravity; // as mult
    public float floatGravityRate; // in seconds to complete

    public float momentumModeVelocityClamp;
    public bool activated => currentActivationTime >= timeToActivateFloat;
    public bool disableUntilGrounded;
    public bool endOfAbility;
    // Get only at time of activation. If player not moving then use float mode. otherwise use momentum mode
    public bool noXInput => Input.GetAxisRaw("Horizontal") == 0; 
    public bool setFloatMode; // Ability is either float mode (true) or momentum mode (false)


    public SkinnedMeshRenderer bubbleMesh;
    private ParticleSystem psBurst;

    public bool bubbleVisible;
    

    public Rigidbody rb;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        bubbleMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        psBurst = GetComponentInChildren<ParticleSystem>();
        psBurst.Stop();
        remainingUses = maxUses;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            remainingUses--;
            //momentumModeVelocityClamp = rb.velocity.x;

            if (playerController.grounded)
            {
                // Will set the Float Mode to the last known mode when grounded, and reset remaining uses
                // If no X input, float mode, else momentum mode (which is setFloatMode = false)
                setFloatMode = noXInput;
                
            }
        }

        if (Input.GetButton("Jump") && remainingUses >= 0) // Change to more than to make it work right
        {
            currentActivationTime += Time.deltaTime;
        } else
        {
            currentActivationTime -= Time.deltaTime;
        }
        currentActivationTime = Mathf.Clamp(currentActivationTime, 0, timeToActivateFloat);


        
        if (playerController.grounded)
        {
            //setFloatMode = noXInput;
            remainingUses = maxUses;

            // Disable the disabler on abilities when grounded and not holding jump
            if (! Input.GetButton("Jump")) disableUntilGrounded = false;

            if (noXInput)
            {
                
            }
        }
    }
    void FixedUpdate()
    {
        playerController.SetMoveEnabled(!activated);
        Debug.Log("Activated: " + (activated && !disableUntilGrounded));
        if (activated && !disableUntilGrounded)
        {
            bubbleMesh.enabled = true;
            endOfAbility = true; // Resets this in preparation for ability end

            if (setFloatMode) // Float Mode
            {
                rb.AddForce(Vector3.up * floatSpeed * Time.fixedDeltaTime, ForceMode.Force);
                Debug.Log("velocity: " + rb.velocity);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, floatVelocityCap);
                //rb.velocity.y = Mathf.Clamp(rb.velocity.y, -floatVelocityCap, floatVelocityCap);
            }
            else // Momentum mode
            {
                // Deactivates abilities when gravity returns to normal
                if (currentFloatGravity >= 1)
                {
                    disableUntilGrounded = true;
                    return;
                }
                
                Debug.Log("momentum mode");
                playerController.SetGravityMultAbility(currentFloatGravity);
                currentFloatGravity += (1f/floatGravityRate) * Time.fixedDeltaTime;
                currentFloatGravity = Mathf.Clamp01(currentFloatGravity);

                //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -momentumModeVelocityClamp, momentumModeVelocityClamp), rb.velocity.y, rb.velocity.z);
            }


        } else // Reset changes when abilities deactivated
        {
            playerController.SetGravityMultAbility(1f);
            currentFloatGravity = 0f;
            bubbleMesh.enabled = false;

            if(endOfAbility) // Only triggered once when ability ends
            {
                psBurst.Play();
                endOfAbility = false;
            }
        }

    }
}
