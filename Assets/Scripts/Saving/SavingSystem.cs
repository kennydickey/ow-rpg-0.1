using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

// core(main) saving system

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            print("saving to " + path); //FromFile(defaultSaveFile)

            using(FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter advancedFormatter = new BinaryFormatter(); // Calling BinaryFormatter's constructor
                advancedFormatter.Serialize(stream, CaptureState()); // stream file and "graph" to serialize         
            }
            // auto closes stream on exit of using

            // must do this to close any file handles that were created vv
            // stream.Close(); no longer needed with using(Filestream)
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            print("loading " + path);
            using (FileStream stream = File.Open(path, FileMode.Open)) // bc .Create overwrites file
            {
                BinaryFormatter advancedFormatter = new BinaryFormatter();
                RestoreState(advancedFormatter.Deserialize(stream)); // (cast)cast
            }
        }       

        private object CaptureState()
        {
            // entire state of game will be a dictionary, dictionaries are serializeable by default, so ready for save file
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                //store our captured state object into the dictionary
                state[saveable.GetUniqueIdentifier()] = saveable.EntityCaptureState(); // so setting key and value for Dictionary state
            }

            return state;
        }

        private void RestoreState(object restoredState)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)restoredState;
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.EntityRestoreState(stateDictionary[saveable.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromFile(string saveFile)
        {
            // .sav just to mark it as indeed a save file
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}