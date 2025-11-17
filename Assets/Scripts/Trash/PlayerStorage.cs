using System.Collections.Generic;
using UnityEngine;

public class PlayerStorage : MonoBehaviour
{
    public int maxCapacity = 6;

    // This is your queue of Trashs
    public List<TrashType> storage = new List<TrashType>();

    public PlayerStorageController storageController;


    // Called when player tries to pick up an Trash
    public bool AddTrash(TrashType type)
    {
        if (storage.Count >= maxCapacity)
        {
            Debug.Log("Storage full! Can't pick up " + type.trashName);
            return false;
        }

        storage.Add(type);
        Debug.Log("Picked up " + type.trashName + " | Storage count: " + storage.Count);
        PrintStorage();

        storageController?.RefreshUI();

        return true;
    }

    // Removes the first Trash (slot 1)
    public TrashType RemoveFirstTrash()
    {
        if (storage.Count == 0)
        {
            Debug.Log("Storage empty!");
            return null;
        }

        TrashType removed = storage[0];
        storage.RemoveAt(0);   // shifts everything up automatically

        Debug.Log("Removed " + removed.trashName + " | Storage count: " + storage.Count);
        PrintStorage();

        storageController?.RefreshUI();

        return removed;
    }

    // Check how many Trashs currently held
    public int Count()
    {
        return storage.Count;
    }

    // (optional) See what's inside the queue
    public void PrintStorage()
    {
        string content = "Storage: [ ";
        foreach (var Trash in storage)
        {
            content += Trash.trashName + " ";
        }
        content += "]";
        Debug.Log(content);
    }

    // TEMPORARY

}
