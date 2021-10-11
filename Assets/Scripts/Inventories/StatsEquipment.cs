using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.CharaStats;
using UnityEngine;

namespace RPG.Inventories
{
    // inheriting from equipment, so overriding and changing Equipment
    public class StatsEquipment : Equipment, IModifierProvider
    {
        IEnumerable<float> IModifierProvider.GetAdditiveModsI(UpCharaStats stat)
        {
            foreach(var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider; // should be cast bc not all are ImodifierProviders
                if (item == null) continue;
                foreach(float modifier in item.GetAdditiveModsI(stat))
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<float> GetPercentageModsI(UpCharaStats stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider; // should be cast bc not all are ImodifierProviders
                if (item == null) continue;
                foreach (float modifier in item.GetPercentageModsI(stat))
                {
                    yield return modifier;
                }
            }
        }
    }
}