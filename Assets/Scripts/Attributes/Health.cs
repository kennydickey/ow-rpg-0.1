﻿using UnityEngine;
using RPG.Saving;
using RPG.CharaStats;
using RPG.Core;
using UnityEngine.UI;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        public bool isDead = false;

        private void Start()
        {
            healthPoints = GetComponent<BaseCharaStats>().GetHealth();
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
                AwardExperience(instigator); // will reward instigator(Fighter.cs) who attacked
            }
        }       

        public float GetHealthPercentage()
        {
            // amount of health in relation to base health, gives us a fraction, 100 x gives us percent
            return 100 * (healthPoints / GetComponent<BaseCharaStats>().GetHealth());           
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
            experience.GainExperience(GetComponent<BaseCharaStats>().GetExperienceReward());
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