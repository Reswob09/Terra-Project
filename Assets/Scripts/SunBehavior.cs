using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class SunBehavior : MonoBehaviour
{

    [SerializeField] Light directionalLight; 
    bool ifMoving; // set to true upon triggering 
    [SerializeField] float sunlerpSpeed = 1f;
    [SerializeField] Quaternion positionA;
    [SerializeField] Quaternion positionB;
    

    private bool moveToB = false;
    private float t = 0f;
    void Start()
    {
       ifMoving = false;
       GetComponent<DirectionalLight>();
    }

    void Update()
    {

        // Setup trigger system to lerp between light intensity, color, and position or rotation
        
        if(ifMoving == true)
        {
            moveToB = !moveToB;
            t = 0f;

           directionalLight.intensity = 100f;

            Debug.Log("The Sun is Moving!");
        }

        t += Time.deltaTime * sunlerpSpeed;
        t = Mathf.Clamp01(t);

       Vector3 startPos = moveToB ? positionA.eulerAngles : positionB.eulerAngles;
        Vector3 targetPos = moveToB ? positionB.eulerAngles : positionA.eulerAngles;

        transform.position = Vector3.Lerp(startPos, targetPos, t);
        
        // positionA and positionB are set through empties, not coordinates, i think???
        // probably need to fix this method
    }

    void ModifyLightSettings()
    {
        directionalLight.intensity = 1.0f;

        directionalLight.color = Color.aquamarine;

        directionalLight.shadows = LightShadows.Soft;
    }

  
}
