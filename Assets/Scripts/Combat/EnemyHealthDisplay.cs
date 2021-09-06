using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter; // fighter script of the player, not the enemy

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>(); // to get access to player's target

        }

        private void Update()
        {
            if(fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "Not Available";
                return;
            }
            Health health = fighter.GetTarget(); // this will be the Health component on the player's target
            // second param, if we want decimal places will be 0.0, or 0.00 etc
            GetComponent<Text>().text = string.Format("{0:0}%", health.GetHealthPercentage());
        }
    }
}