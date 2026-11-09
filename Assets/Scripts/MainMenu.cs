using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
[SerializeField] private string gameplaySceneName = "Tutorial";
    
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(gameplaySceneName);
    }

    public void SettingsMenu()
    {
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
