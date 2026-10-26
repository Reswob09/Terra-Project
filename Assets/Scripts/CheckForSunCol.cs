using Unity.VisualScripting;
using UnityEngine;

public class CheckForSunCol : MonoBehaviour
{
     public Rigidbody rb;
    public Controller controller;
    public SunBehavior sun;
    public bool shouldLerp;
    bool ifMoving; 
    void Start()
    {
        shouldLerp = false;
        rb.gameObject.GetComponent<Rigidbody>();
    }
     void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("BehaviorTriggers"))
        {
            shouldLerp = true;
        }
    }
    void Update()
    {
        
    }
}
