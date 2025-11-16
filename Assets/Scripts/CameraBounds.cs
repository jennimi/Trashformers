using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public static CameraBounds Instance;

    private Camera cam;
    private Vector2 min;
    private Vector2 max;

    void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void UpdateBounds()
    {
        min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        max = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
    }

    public Vector2 Min => min;
    public Vector2 Max => max;
}
