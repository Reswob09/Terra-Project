using Unity.VisualScripting;
using UnityEngine;

public class CheckForSunCol : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;
    [SerializeField] GameObject playerCamera;
    public Controller controller;
    public SunBehavior sunBehavior;
    bool ifMoving; 
    void Start()
    {
        ifMoving = false;
        rb.gameObject.GetComponent<Rigidbody>();
    }
     void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("BehaviorTriggers"))
        {
            ifMoving = true;
        }
    }
    void Update()
    {
        
    }
}
