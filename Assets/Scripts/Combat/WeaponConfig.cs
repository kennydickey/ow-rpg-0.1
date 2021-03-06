using UnityEngine;
using RPG.Attributes;
using GameDevTV.Inventories;
using RPG.CharaStats;
using System.Collections.Generic;

namespace RPG.Combat
{
    // This allows us to create our weapon SO's and is part of each SO created
    [CreateAssetMenu(fileName = "Weapon", menuName = "MakeWeapon/NewWeapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] Weapon equipPrefab = null;
        [SerializeField] AnimatorOverrideController weaponAnimOverride = null;

        [SerializeField] float weaponRange = 1f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponPercentageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null; // keep as null if weapon has no projectile

        const string weaponName = "Weapon"; // just an easier way to access Weapon as a string

        public Weapon Spawn(Transform rightHandTr, Transform leftHandTr, Animator animator)
        {
            DestroyOldWeapon(rightHandTr, leftHandTr); // destroys whichever weapon it finds before we equip

            Weapon weaponEquip = null; // already defaults at null, but just to be explicit
            if (equipPrefab != null) // if there is a prefab able to equip
            {
                if (isRightHanded)
                {
                    weaponEquip = Instantiate(equipPrefab, rightHandTr);
                    weaponEquip.gameObject.name = weaponName;
                }
                else
                {
                    weaponEquip = Instantiate(equipPrefab, leftHandTr);
                    weaponEquip.gameObject.name = weaponName;
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
            return weaponEquip;
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

                                // Instigator -2- gameObject from fighter who attacked then becomes the value of vv
        public void LaunchProjectile(Transform rightHandTr, Transform leftHandTr, Health target, GameObject instigator, float calculatedDamage)
        {
            if (isRightHanded)
            {
                Projectile projectileInstance = Instantiate(projectile, rightHandTr.position, Quaternion.identity);
            // Instigator -3- Which then is the value of vv
                projectileInstance.SetTarget(target, instigator, calculatedDamage); // instigator set here
            }
            else
            {
                Projectile projectileInstance = Instantiate(projectile, leftHandTr.position, Quaternion.identity);
                projectileInstance.SetTarget(target, instigator, calculatedDamage); // previously weaponDamage
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

        public float GetWeaponPercentageBonus() // percentage so that weapon percent stays relevant in relation to base dmg 
        {
            return weaponPercentageBonus;
        }

        // Special case similar to StatsEquipableItem
        public IEnumerable<float> GetAdditiveModsI(UpCharaStats stat)
        {
            if(stat == UpCharaStats.Damage) // if name of stat called in function is Damage..
            {
                yield return weaponDamage;
            }
        }
        // Special case similar to StatsEquipableItem
        public IEnumerable<float> GetPercentageModsI(UpCharaStats stat)
        {
            if (stat == UpCharaStats.Damage) // if name of stat called in function is Damage..
            {
                yield return weaponPercentageBonus;
            }
        }
    }
}