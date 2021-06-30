using System;
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
            for (int i = 0; i < transform.childCount; i++) // for children of Patrol Path
            {
                
                int j = GetNextIndex(i);
                
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointNodeSize);

                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j)); // from 0 to 1, from 3 to 0 etc
                

            }
        }

        private int GetNextIndex(int i)
        {
            if (i+1 == transform.childCount) // next waypoint is the last, index[3] + 1 is 4th
            {
                return 0; // draw line from waypoint to 0
            } // else..
            return i + 1; // draw line from i to i + 1
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}