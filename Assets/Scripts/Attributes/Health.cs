using UnityEngine;
using RPG.Saving;
using RPG.CharaStats;
using RPG.Core;
using System;
using UnityEngine.Events;
using RPG.UI;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenPercentage = 70;
        [SerializeField] UnityEvent<float> takeDamageEvent; // unity events can take arguments such as <float>

        // following no longer needed, unity can now handle generics I think v
        //[SerializeField] TakeDamageEvent takeDamageEvent;
        //[System.Serializable]
        //public class TakeDamageEvent : UnityEvent<float> {} // inherit and make serializeable

        float healthPoints = -1f; // just the value uninitialized
        public bool isDead = false;

        private void Start()
        {
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health);
            }
        }

        private void OnEnable()
        {
            GetComponent<BaseCharaStats>().onLevelUp += RegenHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseCharaStats>().onLevelUp -= RegenHealth;
        }

        private void RegenHealth()
        {
            // gets the value that we set for initial health in our CharaProgSo, times a percent
            float regenPoints = GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health) * (regenPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenPoints); // returns larger of the two
        }

        public bool IsDead()
        {
            return isDead;
        }

        // Instigator -6- ? more passing in, ugh
        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && isDead == false)
            {
                Die();
                AwardExperience(instigator); // will reward instigator(Fighter.cs) who attacked after Die()
            }
            else
            {
                takeDamageEvent.Invoke(damage); // only take damage if not dead
            }
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health);
        }

        public float GetHealthPercentage()
        {
            // amount of health in relation to base health, gives us a fraction, 100 x gives us percent
            return 100 * (healthPoints / GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health));           
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("death");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints;  //this is a float, so serializeable by default
        }

        public void RestoreState(object state)
        {
                          // vv we cannot change Isaveable's parameter, but we can cast here
            healthPoints = (float)state;
            if(healthPoints <= 0)
            {
                Die();

            }
        }
    }
}
