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
    public Dictionary<TrashType, int> recycleCounts = new Dictionary<TrashType, int>();
    public int requiredAmountToRecyclePerType = 5;

    public int currentWaveProgress = 0;
    public int currentWaveLimit = 0;
    public int currentWave = 0;

    public TMP_Text waveText;
    public Slider waveProgressBar;

    public event Action<int> OnWaveStarted;

    private void Start()
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
        UpdateWaveBar();
    }

    public void NextWave()
    {
        if (currentWave >= 6) return; // whats this for

        // change variables for new wave here
        currentWave++;
        updateAllowedTypes();
        resetWaveProgress();

        UpdateWaveBar(forceSnap: true);
        UpdateWaveUI();

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

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateWaveText(text);
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
