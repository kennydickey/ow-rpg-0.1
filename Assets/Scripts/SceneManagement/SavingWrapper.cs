using System.Collections;
using RPG.Saving;
using UnityEngine;

// for saving based on key bindings
// wrapper is also for the case where we want different save slots

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 1f;

        private void Awake()
        {
            // restore needs to happen before any Start()s so we can rely on the state having been restored
            StartCoroutine(LoadLastSceneSW());
        }

        private IEnumerator LoadLastSceneSW() // calls start as a coroutine
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            // or without start as a coroutine..
            //StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
            //Load();

            // we can yield return rather than start coroutine since it's a nested coroutine
            yield return fader.FadeIn(fadeInTime);
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
            if (Input.GetKeyDown(KeyCode.Delete)) // actual delete key
            {
                Delete(); // must be in play mode to delete
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

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);

        }
    }
  
}