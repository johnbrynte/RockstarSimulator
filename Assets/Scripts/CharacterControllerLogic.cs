﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class CharacterControllerLogic : MonoBehaviour {

    public float runSpeed = 6f;
    public float groundMaxVelocityChange = 10f;
    public float airMaxVelocityChange = 3f;
    public float jumpSpeed = 6f;
    public float airControl = .3f;
    public float simulatedGravity = 20f;
    public float turnSmoothing = 1f;
    public float dummyJumpOffset = 0.7f;
    public float slowDownSmoothing = 0.2f;
    public float dummyOffsetSmoothing = 0.2f;
    public float jumpMomentumSpeed = 0.8f;

    public ThirdPersonCamera gamecam;

    private bool grounded = false;
    private bool airbound = false;
    private float speed = 0;
    private Vector3 jumpMomentum;
    private Vector3 currentDirection = Vector3.zero;
    private Vector3 groundVelocity;
    private Vector3 groundPosition;
    private Quaternion currentRotation;
    private CapsuleCollider capsule;
    private GameObject playerModel;
    private GameObject dummy;

    private bool jumpFlag = false;

    // Use this for initialization
    void Start () {
        capsule = GetComponent<CapsuleCollider>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Player")
                playerModel = child.gameObject;
            else if (child.gameObject.tag == "Dummy")
                dummy = child.gameObject;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton("Jump"))
            jumpFlag = true;
        else
            jumpFlag = false;
    }

    // Update for physics
    void FixedUpdate() {
        Vector2 inputLeftStick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        speed = inputLeftStick.sqrMagnitude;

        if (speed > 0.1)
        {
            // calculate direction relative to camera
            Vector3 targetDirection = Vector3.zero;
            StickToWorldspace(this.transform, gamecam.transform, inputLeftStick, ref targetDirection);
            currentDirection = Vector3.Lerp(currentDirection, targetDirection, turnSmoothing);

            // rotate the player model
            Quaternion targetRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            currentRotation = Quaternion.Lerp(currentRotation, targetRotation, turnSmoothing);
            playerModel.transform.rotation = currentRotation;
            dummy.transform.rotation = currentRotation;
            
            // move the player
            Vector3 velocityChange = CalculateVelocityChange(currentDirection);
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else if (Mathf.Abs(inputLeftStick.x) > 0.01 || Mathf.Abs(inputLeftStick.y) > 0.01)
        {
            // init the direction so it starts immediately in the correct direction
            StickToWorldspace(this.transform, gamecam.transform, inputLeftStick, ref currentDirection);
        }
        else if (grounded)
        {
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, new Vector3(0, rigidbody.velocity.y, 0), slowDownSmoothing);
        }

        // apply simulated gravity
        rigidbody.AddForce(-Vector3.up*simulatedGravity);

        if (grounded)
        {
            if (airbound)
            {
                Debug.Log("touch down");
                groundPosition = this.transform.position;
                airbound = false;
            }

            // interpolate to the current ground position
            dummy.transform.localPosition = Vector3.Lerp(dummy.transform.localPosition, Vector3.zero, dummyOffsetSmoothing);

            if (jumpFlag)
            {
                jumpMomentum = rigidbody.velocity*jumpMomentumSpeed;
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + jumpSpeed, rigidbody.velocity.z);
                jumpFlag = false;
            }

            // By setting the grounded to false in every FixedUpdate we avoid checking if the character is not grounded on OnCollisionExit()
            grounded = false;
        }
        else
        {
            // limit the position of the dummy (camera target point) when jumping/falling
            Vector3 groundOffset = new Vector3(0, this.transform.position.y - groundPosition.y, 0);
            if (groundOffset.y < 0)
                groundOffset.y = 0;
            groundOffset.y = -groundOffset.y * dummyJumpOffset;
            dummy.transform.localPosition = groundOffset;

            airbound = true;
        }
    }

    // Unparent if we are no longer standing on our parent
    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == transform.parent)
            transform.parent = null;
    }

    // If there are collisions check if the character is grounded
    void OnCollisionStay(Collision col)
    {
        TrackGrounded(col);
    }

    void OnCollisionEnter(Collision col)
    {
        TrackGrounded(col);
    }

    public bool IsInLocomotion()
    {
        return speed > 0.1;
    }

    public void setClimbMode(bool climbMode)
    {
        // TODO
    }

    private void StickToWorldspace (Transform root, Transform camera, Vector2 inputStick, ref Vector3 directionOut) {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(inputStick.x, 0, inputStick.y);

        // get camera rotation (global)
        Vector3 cameraDirection = camera.forward;
        cameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, cameraDirection);
        
        // convert joystick input in Worldspace coordinates
        directionOut = referentialShift * stickDirection;
    }
    
    // From the user input calculate using the set up speeds the velocity change
    private Vector3 CalculateVelocityChange(Vector3 inputVector)
    {
        // Calculate how fast we should be moving
        Vector3 relativeVelocity = transform.TransformDirection(inputVector);
        relativeVelocity.z *= runSpeed;
        relativeVelocity.x *= runSpeed;
        float maxVelocityChange = groundMaxVelocityChange;
        if (!grounded)
        {
            relativeVelocity.z *= airControl;
            relativeVelocity.x *= airControl;
            relativeVelocity += jumpMomentum;
            maxVelocityChange = airMaxVelocityChange;
        }

        // Calcualte the delta velocity
        Vector3 currRelativeVelocity = rigidbody.velocity - groundVelocity;
        Vector3 velocityChange = relativeVelocity - currRelativeVelocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    // Check if the base of the capsule is colliding to track if it's grounded
    private void TrackGrounded(Collision collision)
    {
        float maxHeight = capsule.bounds.min.y + capsule.radius * .9f; // magic number?
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.point.y < maxHeight)
            {
                if (isKinematic(collision))
                {
                    // Get the ground velocity and we parent to it
                    groundVelocity = collision.rigidbody.velocity;
                    transform.parent = collision.transform;
                }
                else if (isStatic(collision))
                {
                    // Just parent to it since it's static
                    transform.parent = collision.transform;
                }
                else
                {
                    // We are standing over a dinamic object,
                    // set the groundVelocity to Zero to avoid jiggers and extreme accelerations
                    groundVelocity = Vector3.zero;
                }

                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

                // Esta en el suelo
                grounded = true;
            }

            break;
        }
    }

    private bool isKinematic(Collision collision)
    {
        return isKinematic(collider.transform);
    }

    private bool isKinematic(Transform transform)
    {
        return transform.rigidbody && transform.rigidbody.isKinematic;
    }

    private bool isStatic(Collision collision)
    {
        return isStatic(collision.transform);
    }

    private bool isStatic(Transform transform)
    {
        return transform.gameObject.isStatic;
    }
}
