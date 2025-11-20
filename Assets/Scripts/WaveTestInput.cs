using UnityEngine;

public class WaveTestInput : MonoBehaviour
{
    public PlayerStorage storage;
    public PlayerStorageUI ui;
    public WaveManager wave;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Try removing first trash item
            if (storage.RemoveFirstTrash() != null)
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
