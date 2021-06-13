using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    //[SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {      

        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }


    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(lastRay.origin, lastRay.direction * 100); // just a visualization
        RaycastHit hit; // << informatuion from hit event stored here
        bool hashit = Physics.Raycast(ray, out hit); // storing in hit, information about where the ray hit, which can be one of multiple ptoperties such as point
        if (hashit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
        //if() //if something happens move player to a fixed target
        //{
        //    GetComponent<NavMeshAgent>().destination = target.position;
        //}
    }




}
