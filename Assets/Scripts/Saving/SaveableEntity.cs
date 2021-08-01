using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using RPG.Core;
using System;

namespace RPG.Saving
{
    [ExecuteAlways] // allows execution of fle when editing, so executes when we click or whatever in editor window
    public class SaveableEntity : MonoBehaviour
    {
        // SerializeField persists in scene / scene file across reloads, but not across scenes
        // uid needs to be saved in scene/ scene file, not in prefab file as it will create duplicate instance uids
        [SerializeField] string uniqueIdentifier = "";
        // dictionary with unique id as string, and SaveableEntity as object with id
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        // following code only needed to initially generate uids while creating
#if UNITY_EDITOR // to avoid error, as this code is not available to package, code is removed entirely when packaging
        private void Update()
        {
            // don't allow execution if..
            if (Application.IsPlaying(gameObject)) return; // checking to see if scene that this.gameObject is in is playing
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // empty path returned means we are in a prefab, so don't add uid

            SerializedObject serializedObject = new SerializedObject(this); // gives the serialization of this monobehaviour
            SerializedProperty idProperty = serializedObject.FindProperty("uniqueIdentifier"); // now we can access it's properties
            // if empty or not unique..
            if(string.IsNullOrEmpty(idProperty.stringValue) || !IsUnique(idProperty.stringValue))
            {
                idProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties(); // to update or "tell" Unity about the change
            }

            globalLookup[idProperty.stringValue] = this; // 'this' is the current SaveableEntity
        }

#endif

        private bool IsUnique(string uniqueCandidate) // check for positive value when called
        {
            // all return trues mean that there is no need to update unique id
            if (!globalLookup.ContainsKey(uniqueCandidate)) return true; // if key is not in global already
            if (globalLookup[uniqueCandidate] == this) return true; // if uniqueCandidate is the same as ours

            if(globalLookup[uniqueCandidate] == null)
            {
                globalLookup.Remove(uniqueCandidate); // remove in global only
                return true;
            }
            // if lookup is not up to date, pointing to correct value, then..
            if (globalLookup[uniqueCandidate].GetUniqueIdentifier() != uniqueCandidate)
            {
                globalLookup.Remove(uniqueCandidate); // remove in global only
                return true;
            }
            // otherwise..
            return false;
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        // lives on player and enemies, so captures all of them
        public object EntityCaptureState() // object is the root for everything so anything can be returned here, captureState doesn't mind what type of objit the saveableentity returns
        {
            // State to return  vv
            Dictionary<string, object> stateDictCapture = new Dictionary<string, object>();

            // gets list of components implementing ISaveable at runtime, such as Mover
            foreach(ISaveable isaveable in GetComponents<ISaveable>())
            {
                // create key for each saveable, for ex. Mover.cs would be key of "Mover"
                stateDictCapture[isaveable.GetType().ToString()] = isaveable.CaptureState();
            }
            return stateDictCapture;
            //return new SerializableVector3(transform.position); // needs to be serializeable
        }

        public void EntityRestoreState(object state)
        {
            // setting new dictionary to be state that we recieved, and as a cast to be sure
            Dictionary<string, object> stateDictRestore = (Dictionary<string, object>)state;
            foreach (ISaveable isaveable in GetComponents<ISaveable>())
            {
                string typeString = isaveable.GetType().ToString();
                if (stateDictRestore.ContainsKey(typeString))
                {
                    isaveable.RestoreState(stateDictRestore[typeString]);
                }
            }
            //SerializableVector3 position = (SerializableVector3)state;
            //GetComponent<NavMeshAgent>().enabled = false; // to prevent glitches when warging
            //transform.position = position.ToVector(); // needed to convert to an actoual Vector3 from Serializeable
            //GetComponent<NavMeshAgent>().enabled = true;
            //GetComponent<ActionScheduler>().CancelCurrentAction(); // keeps player from moving to a clicked point
        }

        
    }

}
