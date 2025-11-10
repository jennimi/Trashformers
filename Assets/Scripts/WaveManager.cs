using UnityEngine;
using TMPro;   // ✅ Import TextMeshPro namespace
using System;

public class WaveManager : MonoBehaviour
{
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

        Debug.Log("Wave " + currentWave + " started!");
        OnWaveStarted?.Invoke(currentWave);
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
    }

}
