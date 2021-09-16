using UnityEngine;

namespace RPG.Combat
{
    public class DestroyEffects : MonoBehaviour
    {
        [SerializeField] GameObject parentToDestroy = null;

        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) // if not alive !
            {
                if(parentToDestroy != null)
                {
                    Destroy(parentToDestroy); // to destroy the whole prefab if there is one
                }
                Destroy(gameObject);
            }
        }
    }
}
