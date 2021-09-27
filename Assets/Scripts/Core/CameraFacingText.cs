using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacingText : MonoBehaviour
    {
        void LateUpdate() // to update healthbar position after enemy movement
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
