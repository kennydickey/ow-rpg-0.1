using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueItemConfig", menuName = "Scriptable Object Demo/DialogueItemConfig", order = 0)]
public class DialogueItemConfig : ScriptableObject
{
    [SerializeField]
    string itemName;
    [SerializeField]
    string description;
    [SerializeField]
    float itemWeight;

    public string GetnName()
    {
        return itemName;
    }
}
