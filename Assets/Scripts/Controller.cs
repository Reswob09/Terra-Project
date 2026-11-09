using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
// please transfer respawn point code to a new script :(
public class Controller : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    private CharacterController cc;
    public Transform player;
    private Camera cam;
    public GameObject playerCamera;
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float shootDistance = 3f;
    [SerializeField] float interactRange = 5f;
    public bool shootHit;
    [SerializeField] float fovNormal = 60f;
    [SerializeField] float fovZoomed = 30f;
    [SerializeField] float targetFOV;
    [SerializeField] float fovSpeed = 5f;
    [SerializeField] float zoomSpeed = 2f;
    private float cameraPitch;
    public float CameraPitch => cameraPitch;
    [SerializeField] float acceleration = 30f;
    [SerializeField] float deceleration = 15f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float slowMaxSpeed = 4f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpForce = 5f;

    // MISC VARS
    public LayerMask collisionMask;
    [SerializeField] Transform respawnPoint;
    [SerializeField] float fallTreshold = -35f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = gameObject.GetComponent<Rigidbody>();
        cam = playerCamera.GetComponentInChildren<Camera>();
        cc = GetComponent<CharacterController>();

        cam.fieldOfView = fovNormal;

        shootHit = false;

        Debug.Log("No Compilng Errors :D");
    }
      private void FixedUpdate()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        Vector3 desiredDirection = (transform.right * inputHorizontal + transform.forward * inputVertical).normalized;

        float speedLimit = Input.GetKey(KeyCode.LeftControl) ? slowMaxSpeed : maxSpeed;

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

        if (desiredDirection.sqrMagnitude > 0.01f)
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, desiredDirection * speedLimit, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
    }

    private void Update()
    {
        RaycastHit hitInfo;

        // CAMERA CONTROLS
        float mouseYaw = Input.GetAxis("Mouse X");
        float mousePitch = Input.GetAxis("Mouse Y");

        transform.Rotate(
            Vector3.up * mouseYaw * mouseSensitivity);
            
            cameraPitch -= mousePitch * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
            
        if(Input.GetMouseButton(1))
        {
            targetFOV = fovZoomed;
        }
        else
        {
            targetFOV = fovNormal;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime *  fovSpeed);

        
        // for this system make sure to setup a collision system

        Transform camT = playerCamera.transform;

        if(Input.GetMouseButton(0))
        {
            shootHit = Physics.Raycast(camT.position, camT.forward, shootDistance);
        }
        if (Physics.Raycast(transform.position,camT.forward, out hitInfo, interactRange))
        {
            Debug.Log("I am looking at object: " + hitInfo.collider.name);
        }

        bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, jumpHeight);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        void Respawn()
        {
            if (cc != null)
            {
            cc.enabled = false;
            transform.position = respawnPoint.position;
            cc.enabled = true;
            return;
            }

             if (rb != null)
            {
              rb.linearVelocity = Vector3.zero;
               rb.angularVelocity = Vector3.zero;
            }
             transform.position = respawnPoint.position;

             if (transform.position.y < fallTreshold)
            {
               Respawn();
            }
        }
        Debug.Log($"mouseYaw: {mouseYaw}   playerYaw: {transform.eulerAngles.y}");
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.aquamarine;
        Gizmos.DrawRay(transform.position, transform.forward * shootDistance);
    }
}
