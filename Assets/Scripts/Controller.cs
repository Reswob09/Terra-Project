using System.Numerics;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
public class Controller : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float moveSpeed = 15f; // move speed is acceleration
    [SerializeField] float deceleration = 1.0f;
    [SerializeField] float strafeSpeed = 7.5f;
    [SerializeField] float maxSpeed = 5000.0f;
    private float currentSpeed = 0.0f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpHeight = 2.5f;
    [SerializeField] Rigidbody rb;
    private Camera cam;
    public GameObject playerCamera;
    [SerializeField] float mouseSensitivity = 200f;
    [SerializeField] float lerpSpeed = 10f;
    private float cameraPitch;
   [SerializeField] float fovNormal = 60f;
    [SerializeField] float fovZoomed = 30f;
    [SerializeField] float targetFOV;
    [SerializeField] float fovSpeed = 5f;
    [SerializeField] float  minDistance = 5f;
    [SerializeField] float cameraDistance = 10f;
    [SerializeField] float cameraRadius = 3f;

    // MISC VARS
    private readonly int collisionMask;
    private float  currentDistance;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = gameObject.GetComponent<Rigidbody>();
        cam = playerCamera.GetComponent<Camera>();

        cam.fieldOfView = fovNormal;
        currentDistance = cameraDistance;

        Debug.Log("No Compilng Errors :D");
    }
      private void FixedUpdate()
    {
        float cameraHorizontalRotation = Input.GetAxis("Horizontal");
        float cameraVerticalRotation = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * cameraHorizontalRotation + transform.forward * cameraVerticalRotation;

        rb.AddForce(movement * moveSpeed);
    }

    private void Update()
    {
        // CAMERA CONTROLS
        float mousePitch = Input.GetAxis("Mouse X");
        float mouseYaw = Input.GetAxis("Mouse Y");

        transform.Rotate(
            Vector3.up * mousePitch * mouseSensitivity * Time.deltaTime);
            
            cameraPitch -= mouseYaw * mouseSensitivity * Time.deltaTime;

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

        // MOVEMENT CONTROLS
      if (Input.GetKey(KeyCode.W))
        {
           currentSpeed = Mathf.Min(currentSpeed + moveSpeed *Time.deltaTime, maxSpeed);

           transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, 0.0f, deceleration * Time.deltaTime);
        }
       

        if (Input.GetKey(KeyCode.A))
        {
           transform.Translate(Vector3.left * strafeSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * strafeSpeed * Time.deltaTime);
        }

        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, jumpHeight);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) // please change this to not be physics based
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
          maxSpeed = 100f;
          moveSpeed = 5f;
          Debug.Log("Speed Decreased");
        }
        else
        {
            maxSpeed = 500f;
            moveSpeed = 15f;
            Debug.Log("Speed Increased");
        }

    }

     void LateUpdate()
    {
        // CAMERA CLIPPING FUNCTION

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
