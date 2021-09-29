using UnityEngine;
using RPG.Saving;
using RPG.CharaStats;
using RPG.Core;
using System;
using UnityEngine.Events;
using RPG.UI;
using GameDevTV.Utils;
using UnityEditor;

// ! first LazyValue example, among also.. Fighter, EnemyAIController, BaseCharaStats
// ! LazyValue ensures that everything using it can be initialized as needed after Awake() but can be forced by Start()
namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenPercentage = 70;
        [SerializeField] UnityEvent<float> takeDamageEvent; // unity events can take arguments such as <float>
        [SerializeField] UnityEvent onDie;

        // following no longer needed, unity can now handle generics I think v
        //[SerializeField] TakeDamageEvent takeDamageEvent;
        //[System.Serializable]
        //public class TakeDamageEvent : UnityEvent<float> {} // inherit and make serializeable

        LazyValue<float> healthPoints; // uninitialized
        public bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth); // LazyValue will initialize healthpoints when we need it
        }

        private float GetInitialHealth() // needs to return a float since that is the param LazyValue expects
        {
            return GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health);
        }

        private void Start()
        {
            // if healthpoints has not been initialized, initialized now vv
            healthPoints.ForceInit(); // also a function of LazyValue
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
            // getting .value ensures that value is from a fully initialized healthpoints right at this time with LazyValue, even without health.Start() or any Start()
            healthPoints.value = Mathf.Max(healthPoints.value, regenPoints); // returns larger of the two
        }

        public bool IsDead()
        {
            return isDead;
        }

        // Instigator -6- ? more passing in, ugh
        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (healthPoints.value == 0 && isDead == false)
            {
                onDie.Invoke();
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
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health);
        }

        public float GetHealthPercentage()
        {
            // amount of health in relation to base health, gives us a fraction, 100 x gives us percent
            return 100 * GetFraction();           
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Health);
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
            return healthPoints.value;  //this is a float, so serializeable by default
        }

        public void RestoreState(object state)
        {
                          // vv we cannot change Isaveable's parameter, but we can cast here
            healthPoints.value = (float)state;
            if(healthPoints.value <= 0)
            {
                Die();

            }
        }
    }
}
