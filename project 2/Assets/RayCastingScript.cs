using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RayCastingScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Console.WriteLine("Here");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Vector3.down points the direction vector downward
        Ray myRay = new Ray(transform.position, Vector3.down);

        // View the ray for debugging purposes
        // Debug.DrawRay(transform.position, Vector3.direction);

        // Raycast returns true if it intersects with anything
        if (Physics.Raycast(myRay, out hit, 50))
        {
            Console.WriteLine("Raycast collision successful");
            // Do the collision stuff here
            // example
            // if(hit.collider.tag == "whatever")
            //{
                //do stuff
            //}
        }
    }
}
