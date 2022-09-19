using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField]
        //DialogueNode[] nodes;
        //must initialize list..
        List<DialogueNode> nodes = new List<DialogueNode>(); // easier to add/remove to list vs array
#if UNITY_EDITOR
        private void Awake()
        {
            Debug.Log("awake from" + name);
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode()); // adds to list
            }
        }
#endif
        // IEnumerable is any obj that allows a for loop over it
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
    }

}
