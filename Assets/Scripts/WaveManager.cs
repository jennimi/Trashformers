using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public int currentWaveProgress = 0;
    public int currentWaveLimit = 10;
    public int currentWave = 1;   // start at wave 1

    public event Action<int> OnWaveStarted;

    private void Awake()
    {
        currentWave = 1;
    }

    private void Start()
    {
        SendWaveUIUpdate(true);
    }

    public void NextWave()
    {
        if (currentWave >= 6)
            return;

        currentWave++;

        // Snap instantly when changing waves
        SendWaveUIUpdate(forceSnap: true);

        OnWaveStarted?.Invoke(currentWave);
    }

    public void ProgressWave()
    {
        currentWaveProgress++;

        if (currentWaveProgress >= currentWaveLimit)
        {
            currentWaveProgress = 0;
            currentWaveLimit += 5;
            NextWave();
            return;
        }

        SendWaveUIUpdate();
    }

    private void SendWaveUIUpdate(bool forceSnap = false)
    {
        if (UIManager.Instance == null)
            return;

        UIManager.Instance.UpdateWaveUI(
            currentWave,
            currentWaveProgress,
            currentWaveLimit,
            forceSnap
        );
    }
}
