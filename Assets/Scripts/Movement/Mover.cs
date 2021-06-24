using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{

    public class Mover : MonoBehaviour, Iaction
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this); // this monobehaviour
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination; // hit.point is a Vector3
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

    }

}
