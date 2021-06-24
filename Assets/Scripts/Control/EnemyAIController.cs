using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 2f;
        Fighter fighter;
        GameObject player;

        private void Start()
        {          
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            if (InAttackRange() && fighter.CanAttack(player))
            {
                
                // if(gameObject.name/tag == "enemy") to select individual items or debug
                //print(gameObject.name + " chase");
                GetComponent<Fighter>().Attack(player);
            }
            else
            {
                fighter.Cancel();
            }
        }


        private bool InAttackRange() // returns a float to compare with chaseDistance 
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance; // return bool in range or not         
        }
    }
}
