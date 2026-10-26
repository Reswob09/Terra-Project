using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement variables

    [SerializeField] float moveSpeed = 15f; // moveSpeed is acceleration
    [SerializeField] float movebackSpeed = 5f; // can also be referenced for acceleration
    [SerializeField] float deceleration = 1.0f;
    [SerializeField] float strafeSpeed = 7.5f;
    [SerializeField] float maxSpeed = 500.0f;
    public float currentSpeed = 0.0f;
    public float currentVelocity => new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpHeight = 1.5f;
    public float currentHeight => transform.position.y;

    public Controller player;
    [SerializeField] Rigidbody rb;
    void Start()
    {
        rb.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * strafeSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * movebackSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * strafeSpeed * Time.deltaTime);
        }
    }
}
