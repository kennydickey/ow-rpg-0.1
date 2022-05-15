using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogItemInventory : MonoBehaviour
{
    [SerializeField]
    DialogueItemConfig[] contents;

    void Start()
    {
        foreach(var item in contents)
        {
            Debug.Log($"has item: {item.GetnName()}");
        }
    }

    void Update()
    {
        
    }
}
