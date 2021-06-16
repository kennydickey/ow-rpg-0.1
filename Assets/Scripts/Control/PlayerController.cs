using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        void Start()
        {

        }

        void Update()
        {
            InteractWithMovement();
            InteractWithCombat();
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); // returns list of hits
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target = null) continue;
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
            }

        }

        public void MoveToCursor()
        {
            //Debug.DrawRay(GetMouseRay().origin, GetMouseRay().direction * 100); // visualization
            RaycastHit hit; // << informatuion from hit event stored here
            bool hashit = Physics.Raycast(GetMouseRay(), out hit); // storing in hit, information about where the ray hit, which can be one of multiple ptoperties such as point
            if (hashit)
            {
                GetComponent<Mover>().MoveTo(hit);
            }
            //if() //if something happens move player to a fixed target
            //{
            //    GetComponent<NavMeshAgent>().destination = target.position;
            //}
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}