using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform target;


        void LateUpdate() // to prevent camera jitter
        {
            transform.position = target.position;
        }
    }
}
