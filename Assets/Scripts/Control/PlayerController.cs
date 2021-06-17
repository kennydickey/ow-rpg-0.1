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
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
            print("edge of world");
        }

        private bool InteractWithCombat() // just making a bool for if statement above
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); // returns list of hits
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target);
                }
                return true;
            }
            return false; // no combat targets

        }

        private bool InteractWithMovement()
        {
            //Debug.DrawRay(GetMouseRay().origin, GetMouseRay().direction * 100); // visualization
            RaycastHit hit; // << informatuion from hit event stored here
            bool hashit = Physics.Raycast(GetMouseRay(), out hit); // storing in hit, information about where the ray hit, which can be one of multiple ptoperties such as point
            if (hashit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);                
                }
                return true;
            }
            //if() //if something happens move player to a fixed target
            //{
            //    GetComponent<NavMeshAgent>().destination = target.position;
            //}
            return false;
        }

        //public void MoveToCursor()
        //{
            
        //}

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}