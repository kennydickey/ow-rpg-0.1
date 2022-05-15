using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
public class DialogueConfig : ScriptableObject
{
    [SerializeField]
    DialogueNode[] nodes;
}