using UnityEngine;

public class CameraClippingPrevention : MonoBehaviour
{

    [SerializeField] float  cameraMinDistance = 5f;
    [SerializeField] float cameraDistance = 10f;
    private float  cameraCurrentDistance;
    [SerializeField] float cameraRadius = 3f;

    [SerializeField] float cameraLerpSpeed = 10f;
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
        // CAMERA CLIPPING FUNCTION

        Vector3 pivot = transform.position + Vector3.up * 1.5f;
        Quaternion rotation = Quaternion.Euler(controller.CameraPitch, transform.eulerAngles.y, 0f);
        Vector3 direction = rotation * Vector3.back;

        float wanted = cameraDistance;
        if (Physics.SphereCast(pivot,
        cameraRadius,
        direction,
        out RaycastHit hit,
        cameraDistance,
        collisionMask, QueryTriggerInteraction.Ignore))

        {
            wanted = Mathf.Clamp(hit.distance, cameraMinDistance, cameraDistance);
        }

        cameraCurrentDistance = Mathf.Lerp
        (cameraCurrentDistance,
        wanted,
        Mathf.Clamp01(Time.deltaTime * cameraLerpSpeed));

        playerCamera.transform.SetPositionAndRotation(pivot + direction * cameraCurrentDistance, rotation);
    }
}
