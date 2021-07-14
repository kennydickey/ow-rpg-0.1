using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int toScene = -1;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                
                StartCoroutine(Transition());
            }


        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            yield return SceneManager.LoadSceneAsync(toScene);
            
            print($"scene {toScene} loaded");
            yield return new WaitForSeconds(2); // just to see destroy happening for now
            Destroy(gameObject);
        }

        
    }
}