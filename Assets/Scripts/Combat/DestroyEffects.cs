using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class DestroyEffects : MonoBehaviour
    {
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) // if not alive !
            {
                Destroy(gameObject);
            }
        }
    }
}