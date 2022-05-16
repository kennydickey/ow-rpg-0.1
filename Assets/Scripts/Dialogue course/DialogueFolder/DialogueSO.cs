using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        DialogueNode[] nodes;
    }
}
