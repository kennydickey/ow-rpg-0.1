using UnityEngine;
using System.Collections;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        public bool isDead = false;
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && isDead == false)
            {
                Die();
            }
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("death");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
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
