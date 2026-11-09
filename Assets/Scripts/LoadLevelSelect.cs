using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelSelect : MonoBehaviour
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
        if (CompareTag("LevelSelect"))
        {
            SceneManager.LoadSceneAsync("Hub_LevelSelect");
        }
    }
}
