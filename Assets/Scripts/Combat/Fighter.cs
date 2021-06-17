using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

public class Fighter : MonoBehaviour
{
    [SerializeField] float weaponRange = 1f;

    Transform target;

    private void Update()
    {
        //if target is null and isinrange, result is nullreferror
        if (target == null) return;
        if (!IsInRange())
        {
            GetComponent<Mover>().MoveTo(target.position);
        }
        else
        {
            GetComponent<Mover>().Stop();
        }
    }

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.position) < weaponRange;
    }

    public void Attack(CombatTarget combatTarget)
    {
        print("attacked!");
        target = combatTarget.transform;
        
    }

    public void Cancel()
    {
        target = null;
    }
}
