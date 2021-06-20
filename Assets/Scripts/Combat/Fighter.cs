using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

public class Fighter : MonoBehaviour, Iaction
{
    [SerializeField] float weaponRange = 1f;

    Transform target;

    private void Update()
    {
        // !if target is null && isinrange, result is nullreferror
        if (target == null) return; // so if target is null, we exit
        if (!IsInRange())
        {
            GetComponent<Mover>().MoveTo(target.position);
        }
        else
        {
            GetComponent<Mover>().Cancel();
        }
    }

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.position) < weaponRange;
    }

    public void Attack(CombatTarget combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this); // htis monobehaviour
        print("attacked!");
        target = combatTarget.transform;
        GetComponent<Animator>().SetBool("attack", true);
        
    }

    public void Cancel() // different Cancel() than mover
    {
        target = null;
    }

   
}
