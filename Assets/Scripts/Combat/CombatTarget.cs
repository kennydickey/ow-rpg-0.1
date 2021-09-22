using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            // ! in context that this is the target acted upon by the callingController, such as the Player
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) // bool if can't attack..
            {
                return false;
            }
            if (Input.GetMouseButton(0)) // consider GetMouseButtonDown()
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}