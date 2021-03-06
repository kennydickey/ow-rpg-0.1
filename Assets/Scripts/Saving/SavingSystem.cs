using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

// core(main) saving system

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastScene"))
            {
                // just overwrite the buildIndex value
                buildIndex = (int)state["lastScene"]; // int as a value that is updated in CaptureState
            }
            // LoadSceneAsync runs after Awakes(), but before any Start()s
            yield return SceneManager.LoadSceneAsync(buildIndex); // wait and then load last scene       
            // RestoreState needs to be called after all Awake(), but before any Start()
            RestoreState(state); // restore all SaveableEntity uid's

        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {        
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromFile(saveFile)); // deletes the file found in the file path provided
        }

        // PRIVATE

        private void SaveFile(string saveFile, object capturedState)
        {
            string path = GetPathFromFile(saveFile);
            print("saving to " + path); //FromFile(defaultSaveFile)

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter advancedFormatter = new BinaryFormatter(); // Calling BinaryFormatter's constructor
                advancedFormatter.Serialize(stream, capturedState); // stream file and "graph" to serialize         
            }
            // auto closes stream on exit of using

            // must do this to close any file handles that were created vv
            // stream.Close(); no longer needed with using(Filestream)
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>(); // if no .sav file exists, create new one
            }
            using (FileStream stream = File.Open(path, FileMode.Open)) // bc .Create overwrites file
            {
                BinaryFormatter advancedFormatter = new BinaryFormatter();
                return (Dictionary<string, object>)advancedFormatter.Deserialize(stream); // cast(cast)
            }
            
        }

        // updating state?
        private void CaptureState(Dictionary<string, object> state)
        {
            // entire state of game will be a dictionary, dictionaries are serializeable by default, so ready for save file
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                //store our captured state object into the dictionary
                state[saveable.GetUniqueIdentifier()] = saveable.EntityCaptureState(); // so setting key and value for Dictionary state
            }
            // setting part of state for last scene with string key and int scene index
            state["lastScene"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> stateDictionary)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (stateDictionary.ContainsKey(id))
                {
                    saveable.EntityRestoreState(stateDictionary[id]);

                }
            }
        }

        private string GetPathFromFile(string saveFile)
        {
            // .sav just to mark it as indeed a save file
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}