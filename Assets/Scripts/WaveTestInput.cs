using UnityEngine;

public class WaveTestInput : MonoBehaviour
{
    public PlayerStorage storage;
    public PlayerStorageController ui;
    public WaveManager wave;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Try removing first trash item
            if (storage.RemoveFirstTrash())
            {
                ui.RefreshUI();  // update UI
                wave.ProgressWave(); // test wave system
            }
            else
            {
                Debug.Log("No items to deliver!");
            }
        }
    }
}
