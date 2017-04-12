using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulation : MonoBehaviour {
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;


    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;
  
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
  
    private GameObject pickup;

  	// Use this for initialization
  	void Start () {
          trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
  	void Update() {
  		if(controller == null){
  			Debug.Log("Controller not initialiezed");
  			return;
  		}

        if (controller.GetPressDown(gripButton) && pickup != null) {
            pickup.transform.parent = this.transform;
            pickup.GetComponent<Rigidbody>().isKinematic = true;
          }
        if (controller.GetPressUp(gripButton) && pickup != null) {
           pickup.transform.parent = null;
            pickup.GetComponent<Rigidbody>().isKinematic = true;
          }
  	}

    private void OnTriggerEnter(Collider collider) {
        pickup = collider.gameObject;
    }

    private void OnTriggerExit(Collider collider) {
        pickup = null;
    }
  }
