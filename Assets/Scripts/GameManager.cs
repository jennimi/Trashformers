using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Fade Overlay")]
    public Image fadeOverlay;
    public float fadeDuration = 1f;
    public float winningFadeDuration = 1f;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject winningPanel;

    [Header("BGM Clips")]
    public AudioClip normalMusic;
    public AudioClip gameOverMusic;
    public AudioClip levelClearedMusic;

    private AudioSource audioSource;

    private bool isPaused = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // --- AUDIO SOURCE SETUP ---
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // Start playing normal music
        PlayBGM(normalMusic, 0.4f);

        // Hide panels
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (winningPanel != null) winningPanel.SetActive(false);

        fadeOverlay.gameObject.SetActive(true);
        Color c = fadeOverlay.color;
        c.a = 0f;
        fadeOverlay.color = c;
    }

    // ------------------------------------------------
    // BGM SWITCHER
    // ------------------------------------------------
    public void PlayBGM(AudioClip clip, float volume = 0.5f)
    {
        if (clip == null || audioSource == null) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    // ------------------------------------------------
    // LEVEL CLEARED
    // ------------------------------------------------
    public void ShowWinningUI()
    {
        PlayBGM(levelClearedMusic, 0.5f);   // <--- MUSIC CHANGE HERE

        if (winningPanel != null)
            StartCoroutine(WinningRoutine());
    }

    private IEnumerator WinningRoutine()
    {
        fadeOverlay.gameObject.SetActive(true);
        Color c = fadeOverlay.color;
        c.a = 0f;
        fadeOverlay.color = c;

        float t = 0f;
        float targetAlpha = 0.95f;

        while (t < winningFadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, targetAlpha, t / winningFadeDuration);
            fadeOverlay.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        fadeOverlay.color = c;

        winningPanel.SetActive(true);
    }

    // ------------------------------------------------
    // GAME OVER
    // ------------------------------------------------
    public void TriggerGameOver()
    {
        isGameOver = true;

        // Change music
        PlayBGM(gameOverMusic, 0.5f);

        foreach (EnemyStats e in EnemyStats.AllEnemies)
        {
            e.enabled = false;
        }

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(0.7f);

        fadeOverlay.gameObject.SetActive(true);
        Color c = fadeOverlay.color;
        c.a = 0f;
        fadeOverlay.color = c;

        float t = 0f;
        float targetAlpha = 0.95f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;

            c.a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
            fadeOverlay.color = c;

            yield return null;
        }

        c.a = targetAlpha;
        fadeOverlay.color = c;

        gameOverPanel.SetActive(true);
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

        audioSource.Pause();

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0.4f);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        audioSource.UnPause();

        if (pausePanel != null)
            pausePanel.SetActive(false);

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
