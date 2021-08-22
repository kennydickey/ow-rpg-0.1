using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Control;
using RPG.Combat;

public class Fighter : MonoBehaviour, Iaction
{
    [SerializeField] float timeSinceAttack = Mathf.Infinity; // to be ready only at start
    [SerializeField] float attackBuffer = 1f; // !Must be set in inspector

    [SerializeField] Transform rightHand = null;
    [SerializeField] Transform leftHand = null;
    [SerializeField] Weapon defaultWeaponSO = null; // unity will be looking for our Weapon ScriptableObject
    Weapon currentWeaponSO = null; // just to initialize as null


    Health target; // will always be of health type so we don't have to GetComponent

    private void Start()
    {
        EquipWeapon(defaultWeaponSO);

    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime; // we will reset after each update

        // !if target is null && isinrange, result is nullreferror
        if (target == null) return; // so if target is null, we exit
        if (target.IsDead()) return;
        if (!IsInRange()) // if not in range to attack
        {
            GetComponent<Mover>().MoveTo(target.transform.position, 1f); // 1f is full speed
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
            transform.LookAt(target.transform); // try Vector3.zero to test
            //this will trigger the Hit() animation event
            GetComponent<Animator>().ResetTrigger("stopAttack"); //prevent trigger bug
            GetComponent<Animator>().SetTrigger("attack"); // sets trigger and then returns to false
            timeSinceAttack = 0;
        }
      
    }

    //Animation Event we set up on Attack animation clip
    void Hit()
    {
        if (target == null) return;
        target.TakeDamage(currentWeaponSO.GetDamage());
        
    }

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < currentWeaponSO.GetRange();
    }

    public bool CanAttack(GameObject combatTarget) // for use in PlayerController
    {
        if (combatTarget == null) return false;
        Health targetToTest = combatTarget.GetComponent<Health>(); // info from combatTarget
        return targetToTest != null && !targetToTest.IsDead(); // can attack if IsDead is false
    }

    
    public void Attack(GameObject combatTarget) //called in PlayerController
    {
        GetComponent<ActionScheduler>().StartAction(this); // htis monobehaviour
        target = combatTarget.GetComponent<Health>();
    }

    public void Cancel() // different Cancel() than mover
    {
        GetComponent<Animator>().ResetTrigger("attack"); //prevent trigger bug
        GetComponent<Animator>().SetTrigger("stopAttack");
        target = null;
        GetComponent<Mover>().Cancel(); // also cancel movement
        
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeaponSO = weapon; // currentWeapon becomes whatever is specified when EquipWeapon called
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHand, leftHand, animator);

    }

    

   
}
