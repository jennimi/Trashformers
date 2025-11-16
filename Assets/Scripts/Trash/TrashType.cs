using UnityEngine;

[CreateAssetMenu(fileName = "TrashType", menuName = "Scriptable Objects/TrashType")]
public class TrashType : ScriptableObject
{
    public string trashName;

    public GameObject prefab;

    public Sprite Icon
    {
        get
        {
            if (prefab == null) return null;

            var sr = prefab.GetComponent<SpriteRenderer>();
            return sr != null ? sr.sprite : null;
        }
    }
}
