using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable] // makes this class serializeable(savable in unity's eyes)
    public class DialogueNode
    {
        public string uniqueID; //public here is automatically serializeable
        public string text;
        public string[] children;
    }
}


