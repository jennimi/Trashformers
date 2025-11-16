using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class PlayerStorageController : MonoBehaviour
{

    public PlayerStorage storage;
    public List<Image> slotImages; // Drag your Image1..Image5 here

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Clear all slots
        foreach (var img in slotImages)
        {
            img.sprite = null;
            img.color = Color.clear;   // <-- MUST HAVE
        }

        // Add icons for items player has
        for (int i = 0; i < storage.storage.Count && i < slotImages.Count; i++)
        {
            TrashType type = storage.storage[i];
            Sprite icon = type.prefab.GetComponentInChildren<SpriteRenderer>().sprite;

            slotImages[i].sprite = icon;
            slotImages[i].color = Color.white;
        }
    }



}
