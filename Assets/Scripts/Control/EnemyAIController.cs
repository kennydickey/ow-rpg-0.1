using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{

    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        // Update is called once per frame
        void Update()
        {
            if(DistanceToPlayer() < chaseDistance)
            {
                // if(gameObject.name/tag == "enemy") to select individual items or debug
                print(gameObject.name + " chase");
            }
        }

        private float DistanceToPlayer() // returns a float to compare with chaseDistance 
        {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(player.transform.position, transform.position);            
        }
    }
}
