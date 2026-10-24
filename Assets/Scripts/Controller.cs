using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
public class Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;
   //[SerializeField] float jumpHeight = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = 7.5f;
    private Rigidbody rb;
    private Camera cam;
    public GameObject playerCamera;
    [SerializeField] float mouseSensitivity = 200f;
    [SerializeField] float lerpSpeed = 10f;
    private float cameraPitch;
   [SerializeField] float fovNormal = 30f;
    [SerializeField] float fovZoomed = 60f;
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
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(
                currentPosition.x, 
                currentPosition.y, 
                currentPosition.z += moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(
                currentPosition.x -= moveSpeed * Time.deltaTime,
                currentPosition.y,
                currentPosition.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(
                currentPosition.x,
                currentPosition.y,
                currentPosition.z -= moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(
                currentPosition.x += moveSpeed * Time.deltaTime,
                currentPosition.y,
                currentPosition.z);
        }
        if(Input.GetKey(KeyCode.Space))
        {
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(
                currentPosition.x,
                currentPosition.y += jumpForce * Time.deltaTime,
                currentPosition.z);
        }
        else
        {
            Vector3 currentPosition = this.gameObject.transform.localPosition;
            this.gameObject.transform.localPosition = new Vector3(
                currentPosition.x,
                currentPosition.y -= gravity * Time.deltaTime,
                currentPosition.z);
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
