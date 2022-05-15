using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueWeapon : MonoBehaviour
{
    [SerializeField]
    DialogueWeaponConfig config;

    void Start()
    {
        Shoot();
    }

    public void Shoot()
    {
        Debug.Log($"Did {config.damage} damage, with {config.name} and {config.maxAmmo} remaining");
        
    }
}
