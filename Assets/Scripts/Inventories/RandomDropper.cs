using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;
using UnityEngine.AI;

public class RandomDropper : ItemDropper // overriding from ItemDropper script rather than monbehaviour
{
    //// CONFIG DATA
    //[Tooltip("How far pickups can be fall from dropper")]
    //[SerializeField] float scatterDistance = 1;

    //// Start is called before the first frame update
    //protected override Vector3 GetDropLocation() // overrides ItemDropper.GetDropLocation
    //{
    //    Vector3 randomPoint = transform.position + (Random.insideUnitSphere * scatterDistance);
    //    NavMeshHit hit;
    //    // bool which samples navmesh within these parameters vv
    //    if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
    //    {
    //        return hit.position;
    //    }
    //}
}
