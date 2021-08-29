using UnityEngine;
using RPG.Attributes;

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

        const string weaponName = "Weapon"; // just an easier way to access Weapon as a string

        public void Spawn(Transform rightHandTr, Transform leftHandTr, Animator animator)
        {
            DestroyOldWeapon(rightHandTr, leftHandTr); // destroys whichever weapon it finds before we equip

            if (equipPrefab != null) // if there is a prefab able to equip
            {
                if (isRightHanded)
                {
                    GameObject weaponEquip = Instantiate(equipPrefab, rightHandTr);
                    weaponEquip.name = weaponName;
                }
                else
                {
                    GameObject weaponEquip = Instantiate(equipPrefab, leftHandTr);
                    weaponEquip.name = weaponName;
                }

                // otherwise don't create weapon for unarmed case
            }

            // ! Review this 
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponAnimOverride != null) // If it's already an override..
            {
                // when spawning weapon, change attack animation to override anim
                animator.runtimeAnimatorController = weaponAnimOverride;
            }
            else if (overrideController != null)// ! Review this also
                                                // returns null if this is the RuntimeAC vv rather than the high level class AnimatorOverrideController              
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }


        }

        private void DestroyOldWeapon(Transform rightHandLook, Transform leftHandLook)
        {
            Transform oldWeapon = rightHandLook.Find(weaponName); // look for "Weapon" in right hand
            if(oldWeapon == null) // if no weapon found in right hand..
            {
                oldWeapon = leftHandLook.Find(weaponName);
            }
            if (oldWeapon == null) return;
            // if "Weapon" is found..
            oldWeapon.name = "DestroyProcessing"; // to explicitly destroy oldWeapon and not destroy new "Weapon"
            Destroy(oldWeapon.gameObject);
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