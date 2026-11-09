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

    public void ReturnToMainMenu()
    {
        SetPaused(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        IsPaused = false;
        Time.timeScale =1f;    
        AudioListener.pause = false;    
    }

    public void ReturnToHubArea()
    {
        SetPaused(false);
        SceneManager.LoadScene("Hub_LevelSelect");
    }
}
