using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type; // our enum displayed to inspector
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjecttionDist = 1f;
        [SerializeField] float sphereCastRadius = 0.2f;

        bool isDraggingUI = false;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        void Update() // all below are bools to prevent each from happening simultaneously
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None); // from our enum
                return;
            }

            if (InteractWithComponent()) return; // replaced InteractWithCombat()
            if (InteractWithMovement()) return;
            //print("edge of world");
            SetCursor(CursorType.None); // from our enum
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }
            // careful, this includes Fader vv, so in FaderCanvas prefab, deselect Interactable and Blocks Raycasts
            if (EventSystem.current.IsPointerOverGameObject()) // is over UI Gameobject
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true; // can only initiate drag when mouse down and over obj
                }
                SetCursor(CursorType.UI); // from our enum
                return true;
            }
            if (isDraggingUI) // if is true
            {
                return true; // can interact with UI if isDragging
            }
            return false;
        }


        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted(); // returns list of hits
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] iRaycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in iRaycastables)
                {
                    if (raycastable.HandleRaycast(this)) // 'this' is the PlayerController which is called for
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false; // if no objects have HandleRaycast()
        }

        RaycastHit[] RaycastAllSorted()
        {
            // sorting from 2 , hits and distances
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), sphereCastRadius); // our unsorted array of hits
            float[] distances = new float[hits.Length]; // array of floats with same length as our hits
            for(var i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance; // setting each distance value in array to actual distance of each hit
            }
            Array.Sort(distances, hits); // arranges using distances as the keys, and hits as items
            return hits; // returns the now ordered list of hits
        }

        private bool InteractWithMovement()
        {
            // ! navmesh does not have a component that we can make IRaycastable
            //Debug.DrawRay(GetMouseRay().origin, GetMouseRay().direction * 100); // visualization           
            Vector3 target;
            bool hashit = RaycastNavMash(out target);
            if (hashit) // if hashit bool is true..
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false; // if cannot move to target, cannot interact

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f); // // move to declared Vector3, 1f is full speed               
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

        // all this is to not have move cursor over things that aren't navmesh
        private bool RaycastNavMash(out Vector3 target) // requesting out as (out ReturnType name), need a bool and a Vector3
        {
            target = new Vector3();

            RaycastHit hit; // << informatuion from hit event stored here
            // storing in hit, information about where the ray hit, which can be one of multiple ptoperties such as point
            bool hashit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hashit) return false;
            // find nearest navmeth point
            NavMeshHit navMeshHit; // always has a value, so assignment isn't required
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point,
                out navMeshHit,
                maxNavMeshProjecttionDist,
                NavMesh.AllAreas); // AllAreas is default
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position; // update target to position on navmesh we raycast to

            //NavMeshPath path = new NavMeshPath(); // may have a null value, so we need to assign an abject
            //bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            //if (!hasPath) return false;
            //if (path.status != NavMeshPathStatus.PathComplete) return false; // will not move to location if not complete
            //if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
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