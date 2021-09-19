using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        enum CursorType
        {
            None,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type; // our enum displayed to inspector
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return; // actually calls each method I think
            if (InteractWithMovement()) return;
            //print("edge of world");
            SetCursor(CursorType.None); // from our enum
        }

        private bool InteractWithCombat() // just making a bool for if statement above
        {

                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); // returns list of hits
                foreach (RaycastHit hit in hits)
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if (target == null) continue;
                    if (!GetComponent<Fighter>().CanAttack(target.gameObject)) // if can't attack..
                    {
                    continue; // continue within foreach on to next hit object
                    }
                    if (Input.GetMouseButton(0)) // consider GetMouseButtonDown()
                    {
                        GetComponent<Fighter>().Attack(target.gameObject);
                    }
                SetCursor(CursorType.Combat); // from our enum
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
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f); // 1f is full speed                
                }
                SetCursor(CursorType.Movement); // from our enum
                return true;
            }
            //if() //if something happens move player to a fixed target
            //{
            //    GetComponent<NavMeshAgent>().destination = target.position;
            //}
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto); // SetCursor is a unity function, hotspot is location of the cursor basically
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings) // for each one in array of mappings
            {
                if(mapping.type == type)
                {
                    return mapping; // finds first matching CursorMapping in array
                }
            }
            return cursorMappings[0]; // if none found, return first item in CursorMapping[]
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}