using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways] // allows execution of fle when editing, so executes when we click or whatever in editor window
    public class SaveableEntity : MonoBehaviour
    {
        // SerializeField persists across reloads
        // uid needs to be saved in scene file, not in prefab file as it will create duplicate instance uids
        [SerializeField] string uniqueIdentifier = "";

        private void Update()
        {
            // don't allow execution if..
            if (Application.IsPlaying(gameObject)) return; // checking to see if scene that this.gameObject is in is playing

        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
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
