using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foregroundRectTransform = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            // true if approx zero or full with 1
            if (Mathf.Approximately(healthComponent.GetFraction(), 0) || Mathf.Approximately(healthComponent.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foregroundRectTransform.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);
        }
    }
}
