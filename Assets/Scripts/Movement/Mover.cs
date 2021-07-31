using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, Iaction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); // provides a bool to switch off navmesh when dead, enabled if not dead

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this); // this monobehaviour
            GetComponent<Fighter>().Cancel();
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            GetComponent<NavMeshAgent>().destination = destination; // hit.point is a Vector3
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); // multiple of clamped val between 0 and 1
            navMeshAgent.isStopped = false;        
        }


        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); //world space direction and speed to local values for animator v^
            float forwardSpeed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forward", forwardSpeed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position); // needs to be serializeable
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;

            GetComponent<NavMeshAgent>().enabled = false; // to prevent glitches when warging
            transform.position = position.ToVector(); // needed to convert to an actoual Vector3 from Serializeable
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction(); // keeps player from moving to a clicked point
        }
    }

}
