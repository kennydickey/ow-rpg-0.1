using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;        
        [SerializeField] bool isHoming;
        [SerializeField] int projectileDwell = 12;
        // Effects
        [SerializeField] GameObject impactEffect = null;

        Health projectileTarget = null; // uses RPG.Core
        float projectileDamage = 0; // no damage from projectile as of now, weapon controls amount

        private void Start()
        {
            transform.LookAt(AimLocation());
            StartCoroutine(ProjectileDwell());
        }

        // Update is called once per frame
        void Update()
        {
            if (projectileTarget == null) return;

            if (isHoming && !projectileTarget.IsDead())
            {
                transform.LookAt(AimLocation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        IEnumerator ProjectileDwell()
        {
            yield return new WaitForSeconds(projectileDwell);
            Destroy(gameObject);
        }

        public void SetTarget(Health target, float damage)
        {   
            this.projectileTarget = target; // assigns whatever we specify when calling to this.projectileTarget
            this.projectileDamage = damage; // damage input coming from weapon when SetTarget is called there
        }

        private Vector3 AimLocation()
        {
            CapsuleCollider targetCapsule = projectileTarget.GetComponent<CapsuleCollider>();
            if(targetCapsule == null)
            {
                return projectileTarget.transform.position;
            }
            return projectileTarget.transform.position + Vector3.up * targetCapsule.height / 2; // just a vector 3 with modified y pos
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != projectileTarget) return;
            if (projectileTarget.IsDead()) return;    // arrow continues without giving damage or being destroyed       
            projectileTarget.TakeDamage(projectileDamage); // Takedamage method is on Health component of our target
            if(impactEffect != null)
            {
                Instantiate(impactEffect, AimLocation(), transform.rotation);
            }
            Destroy(gameObject);

        }
    }
}
