using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player UI")]
    public Image expFillImage;
    private Coroutine expRoutine;

    public TMP_Text expText;
    public TMP_Text levelText;

    public Image healthFillImage;
    public TMP_Text healthText;
    private Coroutine healthRoutine;


    [Header("Wave UI")]
    public Image waveFillImage;
    public TMP_Text waveText;
    private Coroutine waveLerpRoutine;

    [Header("Dash UI")]
    public Image dashFillImage;
    private Coroutine dashRoutine;
    public TMP_Text dashCooldownText;

    public Transform selectedTrashContainer;    // UI parent (Horizontal Layout Group)
    public TrashUIItem trashUIItemPrefab;       // UI prefab


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ---------------- PLAYER UI ----------------
    public void UpdateHealth(float current, float max)
    {
        if (!healthFillImage) return;

        float targetFill = Mathf.Clamp01(current / max);

        // Smooth fill (optional)
        if (healthRoutine != null)
            StopCoroutine(healthRoutine);

        healthRoutine = StartCoroutine(LerpHealthFill(targetFill));

        // Update text
        if (healthText != null)
            healthText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
    }

    private IEnumerator LerpHealthFill(float target)
    {
        float duration = 0.25f;
        float start = healthFillImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            healthFillImage.fillAmount = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        healthFillImage.fillAmount = target;
    }

    public void UpdateEXP(int exp, int cap)
    {
        if (!expFillImage) return;

        float targetFill = Mathf.Clamp01((float)exp / cap);

        if (expRoutine != null)
            StopCoroutine(expRoutine);

        expRoutine = StartCoroutine(LerpExpFill(targetFill));

        if (expText != null)
            expText.text = $"{exp} / {cap}";
    }

    private IEnumerator LerpExpFill(float target)
    {
        float duration = 0.25f;
        float start = expFillImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            expFillImage.fillAmount = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        expFillImage.fillAmount = target;
    }

    public void UpdateLevel(int lvl)
    {
        if (levelText) levelText.text = "Level " + lvl;
    }

    // ---------------- WAVE UI ----------------
    public void UpdateWaveUI(int wave, int progress, int limit, bool forceSnap = false)
    {
        if (waveText)
            // waveText.text = $"Wave: {wave} | Progress: {progress}/{limit}";
            waveText.text = $"Wave: {wave}";

        if (!waveFillImage) return;

        float targetFill = CalculateWaveFill(wave, progress, limit);

        if (forceSnap)
            waveFillImage.fillAmount = targetFill;
        else
            SmoothWaveFill(targetFill);
    }

    private float CalculateWaveFill(int wave, int progress, int limit)
    {
        if (wave >= 6)
            return 1f;

        float normalized = (float)progress / limit;
        return Mathf.Clamp01(((wave - 1) + normalized) / 5f);
    }

    private void SmoothWaveFill(float target)
    {
        if (waveLerpRoutine != null)
            StopCoroutine(waveLerpRoutine);

        waveLerpRoutine = StartCoroutine(LerpWaveFill(target));
    }

    private IEnumerator LerpWaveFill(float target)
    {
        float duration = 0.3f;
        float start = waveFillImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            waveFillImage.fillAmount = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        waveFillImage.fillAmount = target;
    }

    // ---------------- DASH UI
    public void StartDashCooldown(float duration)
    {
        if (!dashFillImage) return;

        if (dashRoutine != null)
            StopCoroutine(dashRoutine);

        dashRoutine = StartCoroutine(DashCooldownRoutine(duration));
    }

    public void ResetDashUI()
    {
        if (!dashFillImage) return;

        if (dashRoutine != null)
            StopCoroutine(dashRoutine);

        dashFillImage.fillAmount = 1f;

        if (dashCooldownText != null)
            dashCooldownText.text = "";
    }

    private IEnumerator DashCooldownRoutine(float duration)
    {
        // Immediate full bar
        dashFillImage.fillAmount = 1f;

        if (dashCooldownText != null)
            dashCooldownText.text = duration.ToString("F1");

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float remaining = duration - elapsed;

            // FILL STARTS AT 1 → GOES TO 0
            dashFillImage.fillAmount = Mathf.Clamp01(remaining / duration);

            // update text (e.g. "5.3")
            if (dashCooldownText != null)
            {
                if (remaining > 0)
                    dashCooldownText.text = remaining.ToString("F1");
                else
                    dashCooldownText.text = "";
            }

            yield return null;
        }

        // Cooldown finished
        dashFillImage.fillAmount = 0f;

        if (dashCooldownText != null)
            dashCooldownText.text = "";
    }


public void DisplaySelectedTrashUI(Dictionary<TrashType, int> chosenTrashIndex)
{
    if (selectedTrashContainer == null)
    {
        Debug.LogError("UIManager: selectedTrashContainer is NOT assigned!");
        return;
    }

    // Clear previous UI icons
    foreach (Transform child in selectedTrashContainer)
        Destroy(child.gameObject);

    // Loop through each selected trash type
    foreach (var kvp in chosenTrashIndex)
    {
        TrashType type = kvp.Key;
        int index = kvp.Value;

        // Safety check
        if (type.prefabs == null || type.prefabs.Count <= index)
        {
            Debug.LogError("TrashType doesn't have prefab index " + index);
            continue;
        }

        GameObject prefab = type.prefabs[index];

        // 🔥 EXACT LOGIC YOU USED IN STORAGE
        Sprite icon = prefab.GetComponentInChildren<SpriteRenderer>()?.sprite;

        if (icon == null)
        {
            Debug.LogError("UIManager: SpriteRenderer missing on prefab: " + prefab.name);
            continue;
        }

        // // Create UI element
        TrashUIItem uiItem = Instantiate(trashUIItemPrefab, selectedTrashContainer);

        // // Assign sprite + text
        uiItem.Setup(icon, prefab.name);
    }
}


}
