using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake() // to call ahead of SavingWrapper.Start(), where canvas group does not exist yet
        {
            canvasGroup = GetComponent<CanvasGroup>();
            //StartCoroutine(FadeOutIn());
        }

        // not needed as Portal is now calling these methods
        //IEnumerator FadeOutIn() // nested coroutine
        //{
        //    yield return FadeOut(1f);
        //    print("fade out");
        //    yield return FadeIn(2f);
        //    print("faded in");
        //}

        public void FadeOutImmediate() // for use in saving wrapper
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time) // total time taken
        {           
            while (canvasGroup.alpha < 1)
            {
                //increments by framerate divided by total time
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null; // makes coroutine wait for 1 frame
            }
        }

        public IEnumerator FadeIn(float time) // total time taken
        {
            while (canvasGroup.alpha > 0)
            {
                //increments by framerate divided by total time
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null; // makes coroutine wait for 1 frame
            }
        }
    }
}