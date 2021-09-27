using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Core;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentity
        {
            A, B, C, D
        }
        [SerializeField] int toScene = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentity destinationId;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 0.6f;
        [SerializeField] float fadeWait = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {              
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if(toScene < 0)
            {
                Debug.LogError("toScene not set");
                yield return null; // or yield break
            }
            
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            //remove control from player
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            // leave last scene behind and save state
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(toScene);
            //yield return new WaitForSeconds(2); // just to see destroy happening for now

            // Have to find player GameObject again bc it's a new player in a new scene
            PlayerController nextScenePlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            nextScenePlayerController.enabled = false;

            // loads necessary state for new world
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            // save again after player's location etc has been updated, before fadein
            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWait);
            fader.FadeIn(fadeInTime);

            // yield return are done, restore control
            nextScenePlayerController.enabled = true;

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destinationId != destinationId) continue;

                return portal; // <- other portal

            }
            return null; //if no portals found
        }


        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            //player.GetComponent<NavMeshAgent>().Warp(otherPortal.transform.position);
            // or we can disable and reenable navmesh
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.transform.position;
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;


        }

        public void Cancel()
        {
            
        }
    }
}