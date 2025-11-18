using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "TrashType", menuName = "ScriptableObjects/TrashType")]
public class TrashType : ScriptableObject
{
    public string name;

    public List<GameObject> prefabs;

}


