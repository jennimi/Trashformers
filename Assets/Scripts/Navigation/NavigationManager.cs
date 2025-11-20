using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    // Loads any scene by name
    public void LoadScene(string sceneName)
    {
        Debug.Log("🔄 Navigating to scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Reloads the current scene
    public void ReloadScene()
    {
        string current = SceneManager.GetActiveScene().name;
        Debug.Log("🔁 Reloading scene: " + current);
        SceneManager.LoadScene(current);
    }

    // Quits the application (works in build)
    public void QuitGame()
    {
        Debug.Log("❌ Quitting game");
        Application.Quit();
    }
}