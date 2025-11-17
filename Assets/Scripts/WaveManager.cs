using UnityEngine;
using TMPro;   // ✅ Import TextMeshPro namespace
using System;

public class WaveManager : MonoBehaviour
{
    public int currentWaveProgress = 0;
    public int currentWaveLimit = 10;

    public int currentWave = 0;
    public TMP_Text waveText;
    public event Action<int> OnWaveStarted;

    private void Start()
    {
        UpdateWaveUI();
    }

    public void NextWave()
    {
        currentWave++;
        UpdateWaveUI();

        OnWaveStarted?.Invoke(currentWave);
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave + "| Progress: " + currentWaveProgress + "/" +currentWaveLimit;
        }
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
    }

}
