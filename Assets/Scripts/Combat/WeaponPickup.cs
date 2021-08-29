using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon pickupSO = null;
        [SerializeField] float pickupRespawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(pickupSO);
                //Destroy(gameObject); // replace with coroutine
                StartCoroutine(HideForSeconds(pickupRespawnTime));
            }
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

        
    }
}
