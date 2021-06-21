using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

public class Fighter : MonoBehaviour, Iaction
{
    [SerializeField] float weaponRange = 1f;
    [SerializeField] float timeSinceAttack = 0f;
    [SerializeField] float attackBuffer = 1f; // !Must be set in inspector
    [SerializeField] float weaponDamage = 5f;

    Transform target;

    private void Update()
    {
        timeSinceAttack += Time.deltaTime; // we will reset after each update

        // !if target is null && isinrange, result is nullreferror
        if (target == null) return; // so if target is null, we exit
        if (!IsInRange())
        {
            GetComponent<Mover>().MoveTo(target.position);
        }
        else
        {
            GetComponent<Mover>().Cancel();        
            AttackBehaviour();       
        }
    }

    private void AttackBehaviour()
    {
        if (timeSinceAttack > attackBuffer)
        {
            //this will trigger the Hit() animation event
            GetComponent<Animator>().SetTrigger("attack"); // sets trigger and then returns to false
            timeSinceAttack = 0;
            
        }           
       
    }

    //Animation Event
    void Hit()
    {
        Health healthComponent = target.GetComponent<Health>();
        healthComponent.TakeDamage(weaponDamage);
    }

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.position) < weaponRange;
    }

    public void Attack(CombatTarget combatTarget) //called in PlayerController
    {
        GetComponent<ActionScheduler>().StartAction(this); // htis monobehaviour
        print("attacked!");
        target = combatTarget.transform;

    }

    public void Cancel() // different Cancel() than mover
    {
        target = null;
    }

   
}
