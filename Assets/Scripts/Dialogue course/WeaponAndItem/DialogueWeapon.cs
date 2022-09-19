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
        // config.name gets filename, others we specified and take from game input
        Debug.Log($"Did {config.damage} damage, with {config.name} and {config.maxAmmo} ammo remaining");
        
    }
}
