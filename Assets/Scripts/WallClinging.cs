using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WallClinging : MonoBehaviour
{
    [Header ("Detection")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float wallCheckDistance = 0.8f;
    [SerializeField] private float minHeightAboveGround = 1f;
    [SerializeField] private float groundCheckDistance = 3.0f;

    [Header("WallRun")]
    [SerializeField] private float wallGravityScale = 0.1f; // 0 = no gravity 1 = normal gravity
    [SerializeField] private float maxWallFallSpeed = 2f;
    [SerializeField] private float wallRunForce = 20f;
    [SerializeField] private float stickForce = 5f;
    [SerializeField] private float maxWallRunTime = 3f;
    [SerializeField] private float wallLeaveResetDelay = 0.5f;

    [Header ("Wall Jump")]
    [SerializeField] private float wallJumpForce = 7f;
    [SerializeField] private float wallJumpSideForce = 8f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float wallJumpCooldown = 0.25f;
    [SerializeField] float lastWallJumpTime = -10f;

    private Rigidbody rb;
    private bool wallLeft, wallRight, isWallRunning;
    private RaycastHit leftHit, rightHit;
    private float wallRunTimer;
    private float timeSinceWall;

    public bool IsWallRunning => isWallRunning;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnDisable()
    {
        isWallRunning = false;
        
        if(rb != null)
        {
            rb.useGravity = true;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        bool grounded = IsGrounded();
        if(grounded)
        {
            wallRunTimer = 0f;
        }

        CheckForWall();

        bool touchingWall = wallLeft || wallRight;

        timeSinceWall = touchingWall ? 0f : timeSinceWall + Time.deltaTime;
        if (timeSinceWall > wallLeaveResetDelay)
        {
            wallRunTimer = 0f;
        }

        bool cooldownOver = Time.time > lastWallJumpTime + wallJumpCooldown;

        bool wantsToWallRun = touchingWall
        && !grounded
        && wallRunTimer < maxWallRunTime
        && cooldownOver;

        if (wantsToWallRun && !isWallRunning)
        {
            StartWallRun();
        }
        else if(!wantsToWallRun && isWallRunning)
        {
            StopWallRun();
        }

        if (isWallRunning)
        {
            wallRunTimer += Time.deltaTime;
        }


        if (touchingWall && !grounded && cooldownOver && Input.GetKeyDown(jumpKey))
        {
            wallJump();
        }

        // Debug.Log($"grounded: {grounded} L:{wallLeft} R:{wallRight} running:{isWallRunning} timer:{wallRunTimer:F2}");

        
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
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float distance = groundCheckDistance + 0.1f;

        bool hit = Physics.Raycast(origin, Vector3.down, distance, groundMask, QueryTriggerInteraction.Ignore);

       // Debug.DrawRay(origin, Vector3.down * distance, hit ? Color.antiqueWhite : Color.skyBlue);
        return hit;
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

      if (Vector3.Dot(transform.forward, wallForward) < 0f) wallForward = -wallForward;

      if (Input.GetAxisRaw("Vertical") > 0)
        {
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        }

        rb.AddForce(-wallNormal * stickForce, ForceMode.Force);
    }

    void wallJump()
    {
        wallRunTimer = 0f;
        lastWallJumpTime = Time.time;

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
