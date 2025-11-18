using System.Collections.Generic;
using UnityEngine;

public class StoredTrash
{
    public TrashType type;
    public GameObject prefab;
}

public class PlayerStorage : MonoBehaviour
{
    public int maxCapacity = 6;

    // This is your queue of Trashs
    public List<StoredTrash> items = new List<StoredTrash>();

    public PlayerStorageUI storageUI;


    // Called when player tries to pick up an Trash
    public bool AddTrash(TrashInstance instance)
    {
        if (items.Count >= maxCapacity)
        {
            Debug.Log("Storage full! Can't pick up " + instance.type.name);
            return false;
        }

        items.Add(new StoredTrash
        {
            type = instance.type,
            prefab = instance.prefab
        });

        Debug.Log("Picked up " + instance.type.name + " | Storage count: " + items.Count);
        PrintStorage();

        storageUI?.RefreshUI();
        if (storageUI == null)
        {
            Debug.Log("YOU GOT NO STORAGE UI");
        }
        else
        {
            Debug.Log("YOU GOT A STORAGE UI HUZZAH");
        }

        return true;
    }

    // Removes the first Trash (slot 1)
    public TrashType RemoveFirstTrash()
    {
        if (items.Count == 0)
        {
            Debug.Log("Storage empty!");
            return null;
        }

        TrashType removed = items[0].type;
        items.RemoveAt(0);   // shifts everything up automatically

        Debug.Log("Removed " + removed.name + " | Storage count: " + items.Count);
        PrintStorage();

        storageUI?.RefreshUI();

        return removed;
    }

    // Check how many Trashs currently held
    public int Count()
    {
        return items.Count;
    }

    // (optional) See what's inside the queue
    public void PrintStorage()
    {
        string content = "Storage: [ ";
        foreach (var trash in items)
        {
            content += trash.type + " ";
        }
        content += "]";
        Debug.Log(content);
    }

    // TEMPORARY

}
