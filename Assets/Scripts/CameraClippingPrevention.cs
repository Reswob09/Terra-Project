using UnityEngine;

public class CameraClippingPrevention : MonoBehaviour
{

    [SerializeField] float  cameraMinDistance = 5f;
    [SerializeField] float cameraDistance = 10f;
    private float  cameraCurrentDistance;
    [SerializeField] float cameraRadius = 3f;

    [SerializeField] float cameraLerpSpeed = 10f;
    [SerializeField] CameraZoom zoom;
    private float cameraPitch;
    public LayerMask collisionMask;
    public GameObject playerCamera;
    private Controller controller;

    void Start()
    {
        controller = GetComponent<Controller>();
        cameraCurrentDistance = cameraDistance;
    }

    void Update()
    {
        
    }
    void LateUpdate()
    {
       float maxDistance = zoom.CurrentDistance;
       float minDistance = Mathf.Min(cameraMinDistance, maxDistance);

        Vector3 pivot = transform.position + Vector3.up * 1.5f;
        Quaternion rotation = Quaternion.Euler(controller.CameraPitch, transform.eulerAngles.y, 0f);
        Vector3 direction = rotation * Vector3.back;

        float wanted = maxDistance;
        if (Physics.SphereCast(pivot, cameraRadius, direction, out RaycastHit hit,
        maxDistance,collisionMask, QueryTriggerInteraction.Ignore))
        {
            wanted = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }

        cameraCurrentDistance = Mathf.Lerp(cameraCurrentDistance, wanted,
        Mathf.Clamp01(Time.deltaTime * cameraLerpSpeed));

        playerCamera.transform.SetPositionAndRotation(pivot + direction * cameraCurrentDistance, rotation);
    }
}
