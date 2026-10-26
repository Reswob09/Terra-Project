using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
public class Controller : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    private Camera cam;
    public GameObject playerCamera;
    [SerializeField] float mouseSensitivity = 200f;
    [SerializeField] float lerpSpeed = 10f;
    [SerializeField] float shootDistance = 3f;
    [SerializeField] float interactRange = 5f;
    private bool shootHit;
    private float cameraPitch;
   [SerializeField] float fovNormal = 60f;
    [SerializeField] float fovZoomed = 30f;
    [SerializeField] float targetFOV;
    [SerializeField] float fovSpeed = 5f;
    [SerializeField] float  minDistance = 5f;
    [SerializeField] float cameraDistance = 10f;
    [SerializeField] float cameraRadius = 3f;
    [SerializeField] float acceleration = 30f;
    [SerializeField] float deceleration = 15f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float slowMaxSpeed = 4f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float jumpForce = 5f;
    public float currentHeight => transform.position.y;
    public float currentSpeed => new Vector3(rb.linearVelocity.x,0, rb.linearVelocity.z).magnitude;

    // MISC VARS
    public LayerMask collisionMask;
    private float  currentDistance;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = gameObject.GetComponent<Rigidbody>();
        cam = playerCamera.GetComponent<Camera>();

        cam.fieldOfView = fovNormal;
        currentDistance = cameraDistance;

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

        if(desiredDirection.sqrMagnitude > 0.01f)
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, desiredDirection * speedLimit, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
        }
    }

    private void Update()
    {
        RaycastHit hitInfo;


        // CAMERA CONTROLS
        float mouseYaw = Input.GetAxis("Mouse X");
        float mousePitch = Input.GetAxis("Mouse Y");

        transform.Rotate(
            Vector3.up * mouseYaw * mouseSensitivity * Time.deltaTime);
            
            cameraPitch -= mousePitch * mouseSensitivity * Time.deltaTime;

            cameraPitch = Mathf.Clamp( cameraPitch, -90.0f, 90.0f);
            
        if(Input.GetMouseButton(1))
        {
            targetFOV = fovZoomed;
        }
        else
        {
            targetFOV = fovNormal;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime *  fovSpeed);

        
        if(Input.GetMouseButton(0))
        {
            Physics.Raycast(transform.position, Vector3.forward, shootDistance);
            shootHit = true;    
        }
        if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo, interactRange))
        {
            Debug.Log("I am looking at object: " + hitInfo.collider.name);
        }

        bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, jumpHeight);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.aquamarine;
        Gizmos.DrawRay(transform.position, Vector3.forward * shootDistance);
    }

    void LateUpdate()
    {
        // CAMERA CLIPPING FUNCTION

        // currently broken please fix this :(

        Vector3 pivot = transform.position + Vector3.up * 1.5f;
        Quaternion rotation = Quaternion.Euler(cameraPitch, transform.eulerAngles.y, 0f);
        Vector3 direction = rotation * Vector3.back;

        float wanted = cameraDistance;
        if (Physics.SphereCast(pivot,
        cameraRadius,
        direction,
        out RaycastHit hit,
        cameraDistance,
        collisionMask, QueryTriggerInteraction.Ignore))

        {
            wanted = Mathf.Clamp(hit.distance, minDistance, cameraDistance);
        }

        currentDistance = Mathf.Lerp
        (currentDistance,
        wanted,
        Mathf.Clamp01(Time.deltaTime * lerpSpeed));

        playerCamera.transform.SetPositionAndRotation(pivot + direction * currentDistance, rotation);
    }
}
