using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinemRemover : MonoBehaviour
    {
        [SerializeField] PlayableDirector playableDirector;
        [SerializeField] PlayerController playerEnabled;
        GameObject player;

        void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl; // to disable movement
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;

        }

        void EnableControl(PlayableDirector playableDirector)
        {
            player.GetComponent<PlayerController>().enabled = true;

        }
    }
}
