using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;

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

        public Coroutine FadeOut(float time) // total time taken
        {
            return fade(1, time);
        }

        public Coroutine FadeIn(float time) // total time taken
        {
            return fade(0, time);
        }

        public Coroutine fade(float targetFadeValue, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(targetFadeValue, time));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float targetFadeValue, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, targetFadeValue)) // while fader is not at target value
            {
                //increments by framerate divided by total time
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetFadeValue, Time.deltaTime / time);
                yield return null; // makes coroutine wait for 1 frame
            }
        }
    }
}