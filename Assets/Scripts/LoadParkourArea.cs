using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadParkourArea : MonoBehaviour
{
    [SerializeField] private Collider coll;
    void Start()
    {
        if (coll == null)
        {
            coll = GetComponent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("ParkourArea"))
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }
    }
}
