using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PlayerController : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        public void MoveToCursor()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100); // just a visualization
            RaycastHit hit; // << informatuion from hit event stored here
            bool hashit = Physics.Raycast(ray, out hit); // storing in hit, information about where the ray hit, which can be one of multiple ptoperties such as point
            if (hashit)
            {
                GetComponent<Mover>().MoveTo(hit);
            }
            //if() //if something happens move player to a fixed target
            //{
            //    GetComponent<NavMeshAgent>().destination = target.position;
            //}
        }
    }