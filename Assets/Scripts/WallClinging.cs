using System;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

// AN IMPORTANT NOTE REGARDING RIGID BODIES: you can freeze certain axes, particularly X and Z, so that you player model does not tip over!

[RequireComponent(typeof(Rigidbody))]
public class WallClinging : MonoBehaviour
{
    [Header ("Detection")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float wallCheckDistance = 0.8f;
    [SerializeField] private float minHeightAboveGround = 1f;

    [Header("WallRun")]
    [SerializeField] private float wallGravityScale = 0.1f; // 0 = no gravity 1 = normal gravity
    [SerializeField] private float maxWallFallSpeed = 2f;
    [SerializeField] private float wallRunForce = 20f;
    [SerializeField] private float stickForce = 5f;
    [SerializeField] private float maxWallRunTime = 3f;

    [Header ("Wall Jump")]
    [SerializeField] private float wallJumpForce = 7f;
    [SerializeField] private float wallJumpSideForce = 8f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody rb;
    private bool wallLeft, wallRight, isWallRunning;
    private RaycastHit leftHit, rightHit;
    private float wallRunTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        CheckForWall();

        bool wantsToWallRun = (wallLeft || wallRight)
         && !IsGrounded()
         && wallRunTimer < maxWallRunTime;

         if(wantsToWallRun && !isWallRunning) StartWallRun();
         else if (!wantsToWallRun && isWallRunning) StopWallRun();

         if (isWallRunning)
        {
            wallRunTimer += Time.deltaTime;
            if(Input.GetKeyDown(jumpKey)) wallJump();
        }

        if (IsGrounded()) wallRunTimer = 0f;
    }

    void FixedUpdate()
    {
        if (isWallRunning) WallRunMovement();
    }
    void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, -transform.right, out leftHit, wallCheckDistance, wallMask);
        wallRight = Physics.Raycast(transform.position, transform.right, out rightHit, wallCheckDistance, wallMask);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, minHeightAboveGround);
    }

    void StartWallRun()
    {
        isWallRunning = true;
        rb.useGravity = false;

        //catching wall mechanic
        Vector3 v = rb.linearVelocity;
        if (v.y < 0) v.y = 0;
        rb.linearVelocity = v;
    }

    void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }

    void WallRunMovement()
    {
        // reducing gravity when wall running
        rb.AddForce(Physics.gravity * wallGravityScale, ForceMode.Acceleration);

        // clamp fall speed
        Vector3 v = rb.linearVelocity;
        if (v.y < -maxWallFallSpeed) v.y = -maxWallFallSpeed;
        rb.linearVelocity = v;

        RaycastHit hit = wallRight ? rightHit : leftHit;
        Vector3 wallNormal = hit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);

        // points in direction player if facing
        if((transform.forward - wallForward).sqrMagnitude > (transform.forward + wallForward).sqrMagnitude)wallForward = -wallForward;

        // push player forward while playing is moving forward, also make sure player sticks to wall
        if(Input.GetAxisRaw("vertical") >0)
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        rb.AddForce(-wallNormal * stickForce, ForceMode.Force);
    }

    void wallJump()
    {
        RaycastHit hit = wallRight ? rightHit : leftHit;
        Vector3 jumpDir = Vector3.up * wallJumpForce + hit.normal * wallJumpSideForce;

        StopWallRun();

        //Reset vertical velocity 
        Vector3 v = rb.linearVelocity;
        v.y = 0;
        rb.linearVelocity = v;

        rb.AddForce(jumpDir, ForceMode.Impulse);
    }
}
