using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper // overriding from ItemDropper script rather than monbehaviour
    {
        // CONFIG DATA
        [Tooltip("How far pickups can be fall from dropper")]
        [SerializeField] float scatterDistance = 1;
        [SerializeField] InventoryItem[] dropLibrary;
        [SerializeField] int numberOfDrops = 2;

        // CONSTANTS
        const int attempts = 30; // max navmesh sample attempts

        public void RandomDrop()
        {
            for (int i = 0; i < attempts; i++)
            {
                InventoryItem randomItem = dropLibrary[Random.Range(0, dropLibrary.Length)];
                DropItem(randomItem, 1); // second number is number to drob and stackabilty
            }             
        }

        // Start is called before the first frame update
        protected override Vector3 GetDropLocation() // overrides ItemDropper.GetDropLocation
        {
            for (int i = 0; i < attempts; i++)
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