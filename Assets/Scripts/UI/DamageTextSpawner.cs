using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;

        void Start()
        {
            Spawn(111);
        }

        private void Spawn(float damageAmount) // can use this function to spawn text when damage is caused
        {
            DamageText damageInstance = Instantiate<DamageText>(damageTextPrefab, transform);
        }
    }
}
