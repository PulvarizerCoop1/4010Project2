using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulation : MonoBehaviour  {
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;


    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;
  
    //private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
  
    private GameObject pickup;

	private LineRenderer lrrrr;
	private Vector3[] lineRendererVertices;

	private SteamVR_TrackedController controller;

  	// Use this for initialization
  	void Start () {

		controller = GetComponent<SteamVR_TrackedController>();

		if(controller == null){
			controller = gameObject.AddComponent<SteamVR_TrackedController>();
			//Debug.Log("Controller not initialiezed");
			//return;
		}

        trackedObj = GetComponent<SteamVR_TrackedObject>();

		lrrrr = gameObject.AddComponent<LineRenderer> ();
		lrrrr.SetWidth (.05f, .05f);
		lrrrr.SetVertexCount (2);
		lineRendererVertices = new Vector3[2];
    }
  	void Update() {
			
		RaycastHit hit;
		// Vector3.down points the direction vector downward
		Vector3 startPos = transform.position;

		Ray myRay = new Ray(startPos, transform.forward);
		//lineRendererVertices [0] = transform.position;

		// View the ray for debugging purposes
		//Debug.DrawRay(transform.position, Vector3.forward);

		// Raycast returns true if it intersects with anything
		if (Physics.Raycast (myRay, out hit)) {
			lineRendererVertices [1] = hit.point;
			lrrrr.SetColors (Color.green, Color.green);

			// Do the collision stuff here
			// example
			// if(hit.collider.tag == "whatever")
			//{
			//do stuff
			//}
		} else {
			lineRendererVertices [1] = startPos + transform.forward * 100.0f;
			lrrrr.SetColors (Color.blue, Color.blue);
		}

		/*
		if (controller.GetPressDown (triggerButton)) {
			


		}

        if (controller.GetPressDown(gripButton) && pickup != null) {
            pickup.transform.parent = this.transform;
            pickup.GetComponent<Rigidbody>().isKinematic = true;
          }
        /*if (controller.GetPressUp(gripButton) && pickup != null) {
           pickup.transform.parent = null;
            pickup.GetComponent<Rigidbody>().isKinematic = true;
          }*/
        // need a buttonpress,maybe trackpad
  		//if (controller.GetPressDown(null) && pickup != null) {       	
        	//pickup.transform.localScale += new Vector3(0.1F, 0.1F, 0.1F);
  		//}

  	}

    private void OnTriggerEnter(Collider collider) {
        pickup = collider.gameObject;

    }

    private void OnTriggerExit(Collider collider) {
        pickup = null;
    }
  }
