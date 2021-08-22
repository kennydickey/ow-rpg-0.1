using NUnit.Framework;
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


        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equipPrefab != null) // if there is a prefab able to equip
            {
                Instantiate(equipPrefab, handTransform);
                // otherwise don't create weapon for unarmed case
            }
            if (weaponAnimOverride != null)
            {
                // when spawning weapon, change attack animation to override anim
                animator.runtimeAnimatorController = weaponAnimOverride;
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