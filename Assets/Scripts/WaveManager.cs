using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; 

public class WaveManager : MonoBehaviour
{
    public int currentWaveProgress = 0;
    public int currentWaveLimit = 10;

    public int currentWave = 0;
    public TMP_Text waveText;

    public Slider waveProgressBar;

    public event Action<int> OnWaveStarted;

    private void Start()
    {
        currentWave = 1;
        UpdateWaveUI();
        UpdateWaveBar();
    }

    public void NextWave()
    {
        if (currentWave >= 6) return;
        currentWave++;
        UpdateWaveBar(forceSnap: true);
        UpdateWaveUI();
        OnWaveStarted?.Invoke(currentWave);
    }


    private void UpdateWaveUI()
    {
        string text = "Wave: " + currentWave + " | Progress: " + currentWaveProgress + "/" + currentWaveLimit;
        if (waveText != null) waveText.text = text; // keep if you want local TMP

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateWaveText(text);
    }

    public void ProgressWave()
    {
        currentWaveProgress++;

        if (currentWaveProgress >= currentWaveLimit)
        {
            currentWaveProgress = 0;
            currentWaveLimit += 5;
            NextWave();
        }

        UpdateWaveUI();
        UpdateWaveBar();
    }

    private void UpdateWaveBar(bool forceSnap = false)
    {

        if (UIManager.Instance != null && UIManager.Instance.waveBar != null)
        {
            // hard cap full at wave 6
            if (currentWave >= 6)
            {
                UIManager.Instance.UpdateWaveBar(UIManager.Instance.waveBar.maxValue);
                return;
            }

            if (forceSnap)
            {
                UIManager.Instance.UpdateWaveBar(currentWave - 1);
                return;
            }

            float normalized = (float)currentWaveProgress / currentWaveLimit;
            UIManager.Instance.UpdateWaveBar((currentWave - 1) + normalized);
            return;
        }

        if (waveProgressBar == null) return;

        // 🔒 HARD CAP — Wave 6 = full bar, no more progress
        if (currentWave >= 6)
        {
            waveProgressBar.value = waveProgressBar.maxValue;
            return; 
        }

        // Snap exactly to whole wave if needed
        if (forceSnap)
        {
            waveProgressBar.value = currentWave - 1;
            return;
        }

        // Smooth progress inside the wave (Wave 1 = 0→1, Wave 2 = 1→2, etc.)
        float normalizedWaveProgress = (float)currentWaveProgress / currentWaveLimit;
        waveProgressBar.value = (currentWave - 1) + normalizedWaveProgress;
    }

}
