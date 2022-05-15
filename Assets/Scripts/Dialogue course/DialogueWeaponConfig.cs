using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueWeaponConfig", menuName = "Scriptable Object Demo/DialogueWeaponConfig", order = 0)]
public class DialogueWeaponConfig : ScriptableObject
{
    public float maxAmmo;
    public float damage;
    public bool areaDamage;
}