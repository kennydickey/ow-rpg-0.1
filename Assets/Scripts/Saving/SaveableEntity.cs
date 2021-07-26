using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Saving
{
    [ExecuteAlways] // allows execution of fle when editing, so executes when we click or whatever in editor window
    public class SaveableEntity : MonoBehaviour
    {
        // SerializeField persists in scene / scene file across reloads, but not across scenes
        // uid needs to be saved in scene/ scene file, not in prefab file as it will create duplicate instance uids
        [SerializeField] string uniqueIdentifier = "";

        // following code only needed to initially generate uids while creating
#if UNITY_EDITOR // to avoid error, as this code is not available to package, code is removed entirely when packaging
        private void Update()
        {
            // don't allow execution if..
            if (Application.IsPlaying(gameObject)) return; // checking to see if scene that this.gameObject is in is playing
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // empty path returned means we are in a prefab, so don't add uid

            SerializedObject serializedObject = new SerializedObject(this); // gives the serialization of this monobehaviour
            SerializedProperty idProperty = serializedObject.FindProperty("uniqueIdentifier"); // now we can access it's properties
            if(string.IsNullOrEmpty(idProperty.stringValue))
            {
                idProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties(); // to update or "tell" Unity about the change
            }
        }
#endif
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        // lives on player and enemies, so captures all of them
        public object EntityCaptureState() // object is the root for everything so anything can be returned here
        {
            return new SerializableVector3(transform.position); // needs to be serializeable
        }

        public void EntityRestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false; // to prevent glitches when warging
            transform.position = position.ToVector(); // needed to convert to an actoual Vector3 from Serializeable
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction(); // keeps player from moving to a clicked point
        }

        
    }

}
