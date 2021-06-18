using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        MonoBehaviour currentAction;

        public void StartAction(MonoBehaviour action)
        {
            if (currentAction == action) return; //if action already in progress
            if (currentAction != null)
            {
                print("cancelled" + currentAction);

            }
            currentAction = action; // so like setting currentAction to Mover.cs or Fighter.cs
        }
    }
}