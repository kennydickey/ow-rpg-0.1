using NUnit.Framework;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "MakeWeapon/NewWeapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equipPrefab = null;
        [SerializeField] AnimatorOverrideController weaponAnimOverride = null;

        [SerializeField] float weaponRange = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null; // keep as null if weapon has no projectile


        public void Spawn(Transform rightHandTr, Transform leftHandTr, Animator animator)
        {
            if (equipPrefab != null) // if there is a prefab able to equip
            {
                if (isRightHanded)
                {
                    Instantiate(equipPrefab, rightHandTr);
                }
                else
                {
                    Instantiate(equipPrefab, leftHandTr);
                }

                // otherwise don't create weapon for unarmed case
            }
            if (weaponAnimOverride != null)
            {
                // when spawning weapon, change attack animation to override anim
                animator.runtimeAnimatorController = weaponAnimOverride;
            }
                
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHandTr, Transform leftHandTr, Health target)
        {
            if (isRightHanded)
            {
                Projectile projectileInstance = Instantiate(projectile, rightHandTr.position, Quaternion.identity);
                projectileInstance.SetTarget(target, weaponDamage);
            }
            else
            {
                Projectile projectileInstance = Instantiate(projectile, leftHandTr.position, Quaternion.identity);
                projectileInstance.SetTarget(target, weaponDamage);
            }
            
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
    }
}