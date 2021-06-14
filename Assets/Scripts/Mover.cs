using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;


    public class Mover : MonoBehaviour
    {
        //[SerializeField] Transform target;

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(RaycastHit hit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point; // hit.point is a Vector3
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); //world space direction and speed to local values for animator v^
            float forwardSpeed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forward", forwardSpeed);
        }




    }

