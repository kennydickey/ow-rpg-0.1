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

        private void Start()
        {
            Load();
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save() // made this it's own method and public so it can be called from other scripts
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }


        public void Load() // made this it's own method and public so it can be called from other scripts
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
  
}