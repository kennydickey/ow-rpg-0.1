using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointNodeSize = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++) // for children pf Patrol Path
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointNodeSize);
            }
        }
    }
}