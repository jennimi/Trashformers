using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Fade Overlay")]
    public Image fadeOverlay;            // Black screen
    public float fadeDuration = 1f;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    private bool isPaused = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Hide panels
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        fadeOverlay.gameObject.SetActive(true);
        Color c = fadeOverlay.color;
        c.a = 0f;
        fadeOverlay.color = c;
    }

    // ------------------------------------------------
    // GAME OVER
    // ------------------------------------------------
    public void TriggerGameOver()
    {
        isGameOver = true;

        foreach (EnemyStats e in EnemyStats.AllEnemies)
        {
            e.enabled = false;
        }

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // 1. Wait for death animation (unscaled, so pause won’t interrupt)
        yield return new WaitForSecondsRealtime(0.7f);

        // 2. Make sure overlay is active and invisible
        fadeOverlay.gameObject.SetActive(true);
        Color c = fadeOverlay.color;
        c.a = 0f;
        fadeOverlay.color = c;

        float t = 0f;
        float targetAlpha = 0.95f;

        // 3. Fade using unscaled time
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // << THE FIX

            float a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);

            c.a = a;
            fadeOverlay.color = c;

            yield return null;
        }

        // 4. Make sure final alpha is applied
        c.a = targetAlpha;
        fadeOverlay.color = c;

        // 5. Show Game Over UI
        gameOverPanel.SetActive(true);

        // 6. Pause AFTER fade (if you want)
        // Time.timeScale = 0f;
    }

    // ------------------------------------------------
    // PAUSE MENU
    // ------------------------------------------------
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        // Fade screen slightly (optional)
        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0.4f);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Remove fade tint
        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0f);
    }

    // ------------------------------------------------
    // BUTTON FUNCTIONS
    // ------------------------------------------------
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
