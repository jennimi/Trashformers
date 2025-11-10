using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // the object the camera follows
    public float smoothSpeed = 5f;  // how smoothly it moves
    public Vector3 offset;  // optional offset from target

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}