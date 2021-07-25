using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SaveableEntity : MonoBehaviour
    {
        public string GetUniqueIdentifier()
        {
            return "";
        }

        public object EntityCaptureState()
        {
            print("Capturing state for uid " +  GetUniqueIdentifier());
            return null; // temp to avoid value error
        }

        public void EntityRestoreState(object state)
        {
            print("Restoring state for uid" + GetUniqueIdentifier());
        }
    }
}
