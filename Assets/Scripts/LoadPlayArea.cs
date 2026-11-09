using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayArea : MonoBehaviour
{
    [SerializeField] private Collider coll;
    private void Start()
    {
        if (coll == null)
        {
            coll = GetComponent<Collider>();
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("PlayArea"))
            {
                SceneManager.LoadSceneAsync("PlayAreaScene");
            }
    }
}