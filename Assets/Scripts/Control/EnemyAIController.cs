using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Control
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 2f;
        [SerializeField] float suspicionTime = 4f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f; // 1m
        [SerializeField] float wayPointDwell = 1f; // 1m
        [Range(0,1)] // sets slider for patrolSpeedFraction vv
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        Vector3 guardPosition;
        float timeSinceSawPlayer = Mathf.Infinity; // only the starting value
        float timeSinceLastWaypoint = Mathf.Infinity;

        int waypointIndex = 0;

        private void Start()
        {          
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position; // starts at our position
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRange() && fighter.CanAttack(player))
            {              
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
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
            
        }

        private void AttackBehaviour()
        {
            timeSinceSawPlayer = 0; // stays at zero through updates
            fighter.Attack(player);
        }
        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //WaitAfterChase(suspicionTime);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null) // if exists
            {
                if (AtWaypoint()) // bool, if approching waypoint within 1m
                {
                    timeSinceLastWaypoint = 0;
                    CycleWaypoint(); // waypointIndex increments
                }
                nextPosition = GetCurrentWaypoint(); // nextPosition is incremented waypoint
            }
            if(timeSinceLastWaypoint > wayPointDwell)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
            
        }

        private bool AtWaypoint() // if within a certain distance tolerance
        {
            
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < wayPointTolerance;
        }     

        private void CycleWaypoint()
        {
            // using patrolPath knowledge of waypoints
            waypointIndex = patrolPath.GetNextIndex(waypointIndex); // returns next increment or 0 to return to first
        }

        private Vector3 GetCurrentWaypoint()
        {
            // using patrolPath knowledge of waypoints
            return patrolPath.GetWaypoint(waypointIndex);
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
