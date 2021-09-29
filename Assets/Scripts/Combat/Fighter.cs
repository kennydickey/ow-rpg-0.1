using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;
using RPG.Combat;
using RPG.Saving;
using RPG.CharaStats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;

public class Fighter : MonoBehaviour, Iaction, ISaveable, IModifierProvider
{
    [SerializeField] float timeSinceAttack = Mathf.Infinity; // to be ready only at start
    [SerializeField] float attackBuffer = 1f; // !Must be set in inspector

    [SerializeField] Transform rightHand = null;
    [SerializeField] Transform leftHand = null;
    [SerializeField] WeaponConfig defaultWeaponSO = null; // unity will be looking for our Weapon ScriptableObject
    LazyValue<WeaponConfig> currentWeaponSO;
    Health target; // will always be of health type so we don't have to GetComponent


    private void Awake()
    {
        currentWeaponSO = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
    }

    private WeaponConfig SetupDefaultWeapon()
    {
        AttachWeapon(defaultWeaponSO);
        return defaultWeaponSO;
    }

    private void Start()
    {
        currentWeaponSO.ForceInit();
        //EquipWeapon(defaultWeaponSO); // instead of defaultWeaponSO // careful, this will set default when changing scenes

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

    public Health GetTarget()
    {
        return target;
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
        float damage = GetComponent<BaseCharaStats>().GetStatFromProg(UpCharaStats.Damage);
        if (currentWeaponSO.value.HasProjectile())
        {
            // // Instigator -1- our fighter gameObject is the instigator who attacked, so starts here
            currentWeaponSO.value.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
        }
        else
        {
            target.TakeDamage(this.gameObject, damage);//<- create
        }

    }
    // ! Save This
    //void Shoot() // in a case where the evnt is called something else
    //{       
    //    Hit();       
    //}

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < currentWeaponSO.value.GetRange();
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

    public IEnumerable<float> GetAdditiveModsI(UpCharaStats stat)
    {
        if(stat == UpCharaStats.Damage) // if stat in question is Damage stat..
        {
            yield return currentWeaponSO.value.GetDamage(); // additive modifier on top of base damage
            // can have multiple yield returns for future reference
        }
    }

    public IEnumerable<float> GetPercentageModsI(UpCharaStats stat) // for use in BaseCharaStats
    {
        if (stat == UpCharaStats.Damage)
        {
            yield return currentWeaponSO.value.GetWeaponPercentageBonus();

        }
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponSO.value = weapon; // currentWeapon becomes whatever is specified when EquipWeapon called
        AttachWeapon(weapon);
    }

    private void AttachWeapon(WeaponConfig weapon)
    {
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHand, leftHand, animator);
    }

    public object CaptureState()
    {
        return currentWeaponSO.value.name;
    }

    public void RestoreState(object state)
    {
        string savedWeaponName = (string)state; //state cast as a string here
        WeaponConfig savedWeapon = UnityEngine.Resources.Load<WeaponConfig>(savedWeaponName); // looks for Objects with Weapon in Resources
        EquipWeapon(savedWeapon);
    }

}
