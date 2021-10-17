using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.CharaStats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper // overriding from ItemDropper script rather than monbehaviour
    {
        // CONFIG DATA
        [Tooltip("How far pickups can be fall from dropper")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] DropLibrary dropLibrary;

        // CONSTANTS
        const int ATTEMPTS = 30; // max navmesh sample attempts

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseCharaStats>();

            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel()); // GetRandomDrops uses our current level
            foreach(var drop in drops)
            {
                DropItem(drop.item, drop.number); // second number is number to drob and stackabilty

            }

        }

        // Start is called before the first frame update
        protected override Vector3 GetDropLocation() // overrides ItemDropper.GetDropLocation
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
               Vector3 randomPoint = transform.position + (Random.insideUnitSphere * scatterDistance);
                NavMeshHit hit;
                // bool which samples navmesh within these parameters vv
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position; // a hit here returns, therefore exits for loop
                }
            }
            return transform.position; // if navmesh sample unsuccessful, just drop here
        }
    }
}