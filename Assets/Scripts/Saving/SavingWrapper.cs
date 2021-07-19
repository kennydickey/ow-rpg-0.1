using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for saving based on key bindings
// wrapper is also for the case where we want different save slots

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<SavingSystem>().Save(defaultSaveFile);             
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GetComponent<SavingSystem>().Load(defaultSaveFile);             
            }
        }
    }
  
}