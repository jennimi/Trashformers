using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class PlayerStorageUI : MonoBehaviour
{

    public PlayerStorage storage;
    public List<Image> slotImages; // Drag your Image1..Image5 here

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        Debug.Log("REFRESHING UI");
        // Clear all slots
        foreach (var img in slotImages)
        {
            img.sprite = null;
            img.color = Color.clear;   // <-- MUST HAVE
        }

        // Add icons for items player has
        for (int i = 0; i < storage.items.Count && i < slotImages.Count; i++)
        {
            GameObject prefab = storage.items[i].prefab;
            Sprite icon = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
            if (icon == null)
            {
                Debug.Log("Yo somethings wrong w the sprite");
            }

            slotImages[i].sprite = icon;
            slotImages[i].color = Color.white;
        }
    }



}
