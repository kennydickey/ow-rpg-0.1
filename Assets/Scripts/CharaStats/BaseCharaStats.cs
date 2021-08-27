using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class BaseCharaStats : MonoBehaviour
{
    [Range(1,99)]
    [SerializeField] int charaLevel = 1;
    [SerializeField] CharaClass charaClass;
}
