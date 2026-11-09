using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
   [SerializeField] float minDistance = 3.5f;
   [SerializeField] float maxDistance = 20f;
   [SerializeField] float startDistance = 10f;
   [SerializeField] float zoomSpeed = 2f;
   [SerializeField] float smoothing = 10f;

   float targetDistance;
   float currentDistance;
   public float CurrentDistance => currentDistance;
    void Start()
    {
        targetDistance = currentDistance = Mathf.Clamp(startDistance, minDistance, maxDistance);
    }

   
    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            targetDistance = Mathf.Clamp(targetDistance - scroll * zoomSpeed, minDistance, maxDistance);
        }
    }

    void LateUpdate()
    {
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, smoothing * Time.deltaTime);
    }
}
