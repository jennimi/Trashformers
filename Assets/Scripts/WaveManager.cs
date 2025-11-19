using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public int currentWaveProgress = 0;
    public int currentWaveLimit = 10;
    public int currentWave = 1;   // start at wave 1

    public event Action<int> OnWaveStarted;

    [Header("Tilesets to hide per wave clear")]
    public GameObject tileset1;
    public GameObject tileset2;
    public GameObject tileset3;
    public GameObject tileset4;
    public GameObject tileset5;

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

        // Hide dirty palette for THIS wave
        HideTilesetForWave(currentWave);

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

    // CLEANLINESS
    private void HideTilesetForWave(int wave)
    {
        switch (wave)
        {
            case 1:
                if (tileset1 != null) tileset1.SetActive(false);
                break;
            case 2:
                if (tileset2 != null) tileset2.SetActive(false);
                break;
            case 3:
                if (tileset3 != null) tileset3.SetActive(false);
                break;
            case 4:
                if (tileset4 != null) tileset4.SetActive(false);
                break;
            case 5:
                if (tileset5 != null) tileset5.SetActive(false);
                break;
        }
    }
}