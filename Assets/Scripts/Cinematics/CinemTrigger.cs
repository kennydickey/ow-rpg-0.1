using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinemTrigger : MonoBehaviour
    {
        private bool hasTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!hasTriggered && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                hasTriggered = true;
            }
            
        }
    }
}