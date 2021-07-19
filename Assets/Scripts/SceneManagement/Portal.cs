using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
            yield return fader.FadeOut(fadeOutTime);         

            yield return SceneManager.LoadSceneAsync(toScene);
            //yield return new WaitForSeconds(2); // just to see destroy happening for now

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWait);
            fader.StartCoroutine(fader.FadeIn(fadeInTime));

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
    }
}