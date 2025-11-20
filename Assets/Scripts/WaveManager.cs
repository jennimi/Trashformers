using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq; // needed for OrderBy


public class WaveManager : MonoBehaviour
{
    public List<TrashType> allTypes;
    public List<TrashType> allowedTypes = new List<TrashType>();

    public Dictionary<TrashType, int> chosenTrash = new Dictionary<TrashType, int>();

    public Dictionary<TrashType, int> recycleCounts = new Dictionary<TrashType, int>();
    public int requiredAmountToRecyclePerType = 5;

    public int currentWaveProgress = 0;
    public int currentWaveLimit = 0;
    public int currentWave = 0;

    public TMP_Text waveText;
    public Slider waveProgressBar;

    public event Action<int> OnWaveStarted;

    [Header("Tilesets to hide per wave clear")]
    public GameObject tileset1;
    public GameObject tileset2;
    public GameObject tileset3;
    public GameObject tileset4;
    public GameObject tileset5;

    [Header("Winning UI / Scene Objects")]
    public GameObject winningContainer; 

    private void Awake()
    {
        Debug.Log("STARTING");
        requiredAmountToRecyclePerType = 2;

        // instantiating data for first wave
        currentWave = 1;
        updateAllowedTypes();
        resetWaveProgress();

        Debug.Log("WAVE STARTING");
        foreach (TrashType type in allowedTypes)
        {
            Debug.Log("Allowed trash type: " + type.name);
        }

        
        UpdateWaveUI();
    }

    private void Start()
    {
        SendWaveUIUpdate(true);
    }

    public void NextWave()
    {
        if (currentWave >= 6)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ShowWinningUI();

            return;
        }

        // change variables for new wave here
        currentWave++;
        updateAllowedTypes();
        resetWaveProgress();

        UpdateWaveUI();

        // Hide dirty palette for THIS wave
        HideTilesetForWave(currentWave);
        
        // Snap instantly when changing waves
        SendWaveUIUpdate(forceSnap: true);

        OnWaveStarted?.Invoke(currentWave);
    }

    private void resetWaveProgress()
    {
        Debug.Log("BEFORE required amount per type: " + requiredAmountToRecyclePerType);
        currentWaveProgress = 0;
        currentWaveLimit = allowedTypes.Count * requiredAmountToRecyclePerType;
        Debug.Log("allowed types: " + allowedTypes.Count);
        Debug.Log("AFTER required amount per type: " + requiredAmountToRecyclePerType);
    }

    private void updateAllowedTypes()
    {
        recycleCounts = new Dictionary<TrashType, int>();

        // IF MAU 2/ 3 RANDOM TYPES PER WEAVE
        requiredAmountToRecyclePerType++;
        allowedTypes.Clear();
        List<TrashType> shuffled = allTypes.OrderBy(x => UnityEngine.Random.value).ToList();

        for (int i = 0; i < 3 && i < shuffled.Count; i++)
        {
            allowedTypes.Add(shuffled[i]);
            recycleCounts[shuffled[i]] = 0;
        }

        foreach (var type in allowedTypes)
        {
            chosenTrash[type] = UnityEngine.Random.Range(0, 6);
        }

        // IF MAU ADD NEW TYPE EVERY WAVE
        // requiredAmountToRecyclePerType = 10;
        // foreach (TrashType type in allTypes)
        // {
        //     if (!allowedTypes.Contains(type))
        //     {
        //         allowedTypes.Add(type);
        //         break; // only add one per wave
        //     }
        // }
    }


    private void UpdateWaveUI()
    {
        string text = $"Wave: {currentWave} | Progress: {currentWaveProgress}/{currentWaveLimit}\n";

        foreach (var type in allowedTypes)
        {
            int collected = recycleCounts.ContainsKey(type) ? recycleCounts[type] : 0;
            text += $"{type.name}: {collected}/{requiredAmountToRecyclePerType}\n";
        }

        if (waveText != null) waveText.text = text; // keep if you want local TMP

        //idk what to do here
    }

    public void AcceptTrash(TrashType type)
    {
        if (!allowedTypes.Contains(type)) return;

        if (!recycleCounts.ContainsKey(type)) return;

        recycleCounts[type]++;

        // Reached per-type limit?
        if (recycleCounts[type] >= requiredAmountToRecyclePerType)
        {
            // remove all trash of this type from the map
            FindObjectOfType<TrashSpawner>()?.DestroyTrashOfType(type);
        }

        ProgressWave();
    }

    public void ProgressWave()
    {
        currentWaveProgress++;

        if (currentWaveProgress >= currentWaveLimit)
            NextWave();

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