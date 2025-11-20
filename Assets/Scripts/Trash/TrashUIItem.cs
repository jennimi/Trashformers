using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrashUIItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;

    public void Setup(Sprite sprite, string trashName)
    {
        icon.sprite = sprite;
        icon.color = Color.white;   // ensure visible
        nameText.text = trashName;
    }
}
