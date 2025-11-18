using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player UI")]
    public Slider healthBar;
    public Slider expBar;
    public TMP_Text levelText;

    [Header("Wave UI")]
    public Slider waveBar;
    public TMP_Text waveText;

    [Header("Dash UI")]
    public DashUI dashUI; // reference to DashUI component (in scene or child)

    void Awake()
    {
        // Singleton pattern (simple)
        if (Instance == null)
        {
            Instance = this;
            // Optionally: DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------- PLAYER UI ----------------
    public void UpdateHealth(float current, float max)
    {
        if (healthBar == null) return;
        healthBar.maxValue = Mathf.Max(1, max);
        healthBar.value = Mathf.Clamp(current, 0, max);
    }

    public void UpdateEXP(int exp, int cap)
    {
        if (expBar == null) return;
        expBar.maxValue = Mathf.Max(1, cap);
        expBar.value = Mathf.Clamp(exp, 0, cap);
    }

    public void UpdateLevel(int lvl)
    {
        if (levelText == null) return;
        levelText.text = "Lv " + lvl;
    }

    // ---------------- WAVE UI ----------------
    public void UpdateWaveBar(float value)
    {
        if (waveBar == null) return;
        waveBar.value = Mathf.Clamp(value, waveBar.minValue, waveBar.maxValue);
    }

    public void UpdateWaveText(string text)
    {
        if (waveText == null) return;
        waveText.text = text;
    }

    // ---------------- DASH UI ----------------
    public void StartDashCooldown(float duration)
    {
        if (dashUI == null) return;
        dashUI.StartCooldown(duration);
    }

    public void ResetDashUI()
    {
        if (dashUI == null) return;
        dashUI.ResetUI();
    }
}
