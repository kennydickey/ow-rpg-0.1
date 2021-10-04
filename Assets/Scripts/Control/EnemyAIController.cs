using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 2f;
        [SerializeField] float suspicionTime = 4f;
        [SerializeField] float aggroDuration = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f; // 1m
        [SerializeField] float wayPointDwell = 1f; // 1m
        [Range(0,1)] // sets slider for patrolSpeedFraction vv
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance;

        Fighter fighter;
        Mover mover;
        GameObject player;
        Health health;
        LazyValue<Vector3> guardPosition;

        float timeSinceSawPlayer = Mathf.Infinity; // only the starting value
        float timeSinceLastWaypoint = Mathf.Infinity;
        float timeSinceAggrivated = Mathf.Infinity; // time since last aggo starts as never

        int waypointIndex = 0;


        private void Awake()
        {
            // references should be setup in time for a public method here that could be called in Start() elsewhere
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            // starts at our position
            guardPosition.ForceInit(); // transform uses a different Monobehaviour, so we should not access on Awake()
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (IsAggrivated() && fighter.CanAttack(player))
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

        public void Aggrevate()
        {
            timeSinceAggrivated = 0; // resets aggro timer
        }

        private void UpdateTimers()
        {
            timeSinceSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
            timeSinceAggrivated += Time.deltaTime; // timer is constantly growing
        }

        private void AttackBehaviour()
        {
            timeSinceSawPlayer = 0; // stays at zero through updates
            fighter.Attack(player);

            AggrevateNearby();
        }

        private void AggrevateNearby()
        {
            // finds all enemies within a certain radius, no direction or max dist necessary, so just up and 0
                               // from our Enemy.transform.position vv
            RaycastHit[] hits =  Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                EnemyAIController enemyAi = hit.collider.GetComponent<EnemyAIController>(); // hit.anyComponentOnCollidedEnemy
                if (enemyAi == null) continue; // skip this one iteration of for loop
                enemyAi.Aggrevate(); // so in summar, any enemy within shouting distance specified is aggroed
            }
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //WaitAfterChase(suspicionTime);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

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

        private bool IsAggrivated() // returns a float to compare with chaseDistance 
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            // check aggro timer
            return distanceToPlayer < chaseDistance || timeSinceAggrivated < aggroDuration; // return bool in range or not         
        }

        // Called by Unity when it wants us to draw gizmos on obj selected
        private void OnDrawGizmosSelected() // also try OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
