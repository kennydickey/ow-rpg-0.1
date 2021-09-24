using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacingText : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
