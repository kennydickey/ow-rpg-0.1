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
        [SerializeField] float suspicionTime = 4f;
        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        Vector3 guardPosition;
        float timeSinceSawPlayer = Mathf.Infinity; // only the starting value

        private void Start()
        {          
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position;
        }

        IEnumerator WaitAfterChase(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRange() && fighter.CanAttack(player))
            {
                timeSinceSawPlayer = 0; // stays at zero through updates
                // if(gameObject.name/tag == "enemy") to select individual items or debug
                AttackBehaviour();
            }
            // out of range, time now increments
            else if (timeSinceSawPlayer < suspicionTime) // so, while incrementing..
            {
                SuspicionBehaviour();                
            }
            else
            {
                GuardBehaviour();
            }
            timeSinceSawPlayer += Time.deltaTime; // 
        }


        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //WaitAfterChase(suspicionTime);
        }
        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
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
