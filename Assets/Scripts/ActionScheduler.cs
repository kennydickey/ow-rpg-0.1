using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        Iaction currentAction;

        public void StartAction(Iaction action)
        {
            if (currentAction == action) return; //if action already in progress
            if (currentAction != null)
            {
                print("cancelled" + currentAction);
                currentAction.Cancel(); // since Fighter and Mover implement their own IAction.Cancel
                

            }
            currentAction = action; // so like setting currentAction to Mover.cs or Fighter.cs
        }
    }
}