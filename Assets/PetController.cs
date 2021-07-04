using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Control;

public class PetController : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(player.transform.position, transform.position) > 2)
        {
            GetComponent<Mover>().MoveTo(player.transform.position, 1f);
            GetComponent<Animator>().SetInteger("animation", 1);
        }
        else
        {
            GetComponent<Mover>().Cancel();
            GetComponent<Animator>().SetInteger("animation", 0);
        }
    }

}
