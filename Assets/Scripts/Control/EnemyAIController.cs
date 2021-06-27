using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 2f;
        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        Vector3 guardPosition;

        private void Start()
        {          
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position;
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRange() && fighter.CanAttack(player))
            {
                
                // if(gameObject.name/tag == "enemy") to select individual items or debug
                //print(gameObject.name + " chase");
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
                mover.StartMoveAction(guardPosition);
            }
        }


        private bool InAttackRange() // returns a float to compare with chaseDistance 
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance; // return bool in range or not         
        }

        // Called by Unity when it wants us to draw gizmos on obj selected
        private void OnDrawGizmosSelected() // also try OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
