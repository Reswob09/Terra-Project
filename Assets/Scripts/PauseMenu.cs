using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   
   public static bool IsPaused { get; private set; }

   [SerializeField] private GameObject pausePanel;
   [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
   [SerializeField] private bool gameLocksCursor = false;
    void Start()
    {
        SetPaused(false);
    }


    void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            SetPaused(!IsPaused);
        }
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void Restart()
    {
        SetPaused(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        {
            Application.Quit();
        }
#endif
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
        AudioListener.pause = paused;

        if(gameLocksCursor)
        {
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;
        }
    }

    private void Oestroy()
    {
        IsPaused = false;
        Time.timeScale =1f;        
    }
}
