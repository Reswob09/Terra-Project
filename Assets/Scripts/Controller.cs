using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
public class Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;
   [SerializeField] float jumpHeight = 5f;
    [SerializeField] float jumpForce = 5f;
    private Rigidbody rb;
    public float mouseSensitivity = 200f;
    private float cameraPitch;
    private float randomTime = 6.28f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = gameObject.GetComponent<Rigidbody>();
    }
    

    private void Update()
    {

        float mousePitch = Input.GetAxis("mouse X");
        float mouseYaw = Input.GetAxis("mouse Y");

        transform.Rotate(
            Vector3.up * mousePitch * mouseSensitivity * Time.deltaTime);
            
            cameraPitch -= mouseYaw * mouseSensitivity * Time.deltaTime;

            cameraPitch = Mathf.Clamp( cameraPitch, -90.0f, 90.0f);

        if (Input.GetMouseButtonDown(0))
        {
        
          Debug.Log("everything is working");
          
        }  

    }

    private void FixedUpdate()
    {
        float cameraHorizontalRotation = Input.GetAxis("Horizontal");
        float cameraVerticalRotation = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * cameraHorizontalRotation + transform.forward * cameraVerticalRotation;

        rb.AddForce(movement * moveSpeed);
    }
}
