using System;
using UnityEngine;

// use for uneven terrain and stuff
public class KillZone : MonoBehaviour
{
[SerializeField] Transform respawnPosition;
private CharacterController characterController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (cc != null) cc.enabled = false;
            if(rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity =Vector3.zero;
            }
            other.transform.position = respawnPosition.position;

            if (cc !=null) cc.enabled = true;
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
