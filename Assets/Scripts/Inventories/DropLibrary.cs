using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/DropLibrary"))]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField]
        DropConfig[] potentialDrops;
        // percentages corresponding to different levels vv, same for all arrays here
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minSingleItem;
            public int[] maxSingleItem;

            public int GetRandomNumber(int level) // get random for particular enemy level
            {
                if (!item.IsStackable())
                {
                    return 1; // if not stackable, just return 1 and exit function
                }
                int min = GetByLevel(minSingleItem, level);
                int max = GetByLevel(maxSingleItem, level);
                // if it is stackable, randomize how many are in the stack
                return UnityEngine.Random.Range(min, max + 1); // +1 bc excludes last otherwise
            }
        }

        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level) // dropdown list of Dropped struct
        {
            if (!ShouldRandomDrop(level))
            {
                yield break; // return from ienum without doing anything
            }
            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        bool ShouldRandomDrop(int level)
        {
            return Random.Range(0, 100) < GetByLevel(dropChancePercentage, level); // is value of dice less than percentage
        }

        int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrops, level);
            int max = GetByLevel(maxDrops, level);
            return Random.Range(min, max);
        }

        Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }

        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = Random.Range(0, totalChance);
            float chanceTotal = 0;
            foreach(var drop in potentialDrops) // for each drop item we specified in inspector
            {
                chanceTotal += GetByLevel(drop.relativeChance, level);
                if(chanceTotal > randomRoll)
                {
                    return drop;
                }
            }
            return null;
        }

        float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }
            return total;
        }

        // takes in an array and a level, returns value from array at given level
        static T GetByLevel<T>(T[] values, int level) // generic type, look it up
        {
            if (values.Length == 0)
            {
                return default;
            }
            if (level > values.Length) // if level is greater than length of array, just returns last element of array
            {
                return values[values.Length - 1];
            }
            if (level <= 0)
            {
                return default;
            }
            return values[level - 1];
        }
    }
}