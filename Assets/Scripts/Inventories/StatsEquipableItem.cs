using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.CharaStats;
using UnityEngine;

namespace RPG.Inventories
{
    // inheriting from equipment, so able to create a different EquipableItem
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        // each of these fields is an array of our Modifier struct below, which is just a stat and float value
        [SerializeField] 
        Modifier[] additiveModifiers;
        [SerializeField]
        Modifier[] percentageModifiers;        

        [System.Serializable]
        struct Modifier
        {
            public UpCharaStats stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModsI(UpCharaStats stat)
        {
            foreach (var aModifier in additiveModifiers)
            {
                if(aModifier.stat == stat)
                {
                    yield return aModifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModsI(UpCharaStats stat)
        {
            foreach (var pModifier in percentageModifiers)
            {
                if (pModifier.stat == stat)
                {
                    yield return pModifier.value;
                }
            }
        }
    }
}
