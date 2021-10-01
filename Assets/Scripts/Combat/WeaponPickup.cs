using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig pickupSO = null;
        [SerializeField] float healthToRestore = 0; // for health pickups, we set this value in inspector of pickup
        [SerializeField] float pickupRespawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject) // generic pickup to be further specified below
        {
            if(pickupSO != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(pickupSO);
            }
            if(healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            //Destroy(gameObject); // replace with coroutine
            StartCoroutine(HideForSeconds(pickupRespawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }       

        private void ShowPickup(bool show)
        {
            gameObject.GetComponent<Collider>().enabled = show;
            //for (int i = 0; i < gameObject.transform.childCount; i++)
            //{
            //    this.transform.GetChild(i).gameObject.SetActive(show);
            //}
            //or
            foreach(Transform child in this.transform)
            {
                child.gameObject.SetActive(show);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController) // called from PlayerController script
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }
        
    }
}
