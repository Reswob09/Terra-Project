using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class SunBehavior : MonoBehaviour
{

    [SerializeField] Light directionalLight; 

  //  [SerializeField] float sunlerpSpeed = 1f;
  //  [SerializeField] Quaternion positionA;
   // [SerializeField] Quaternion positionB;
    [SerializeField] float startIntensity = 1f;
    [SerializeField] float endIntensity = 1000f;
    [SerializeField] float duration = 5f;

    private float elaspedTime = 0f;
    public bool shouldLerp = false;
    

    private bool moveToB = false;
    private float t = 0f;
    void Start()
    {
       GetComponent<DirectionalLight>();
    }

    void Update()
    {

        // Setup trigger system to lerp between light intensity, color, and position or rotation
        
        if(shouldLerp == true)
        {
            Debug.Log("The Sun is changing");
            elaspedTime +=  Time.deltaTime;
            float time = elaspedTime / duration;

            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, time);

            if(elaspedTime >= duration)
            {
                shouldLerp = false;
            }
        }

      // Vector3 startPos = moveToB ? positionA.eulerAngles : positionB.eulerAngles;
       // Vector3 targetPos = moveToB ? positionB.eulerAngles : positionA.eulerAngles;

       // transform.position = Vector3.Lerp(startPos, targetPos, t);
    }

    public void StartIntensityChange(float newTargetIntensity)
    {
        startIntensity = directionalLight.intensity;
        endIntensity = newTargetIntensity;
        elaspedTime = 0f;
        shouldLerp = false;
    }
    void ModifyLightSettings()
    {
        directionalLight.intensity = 1.0f;

        directionalLight.color = Color.aquamarine;

        directionalLight.shadows = LightShadows.Soft;
    }

  
}
