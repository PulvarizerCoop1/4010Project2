/*
 * 
 * Code was created with help from
 * https://www.raywenderlich.com/149239/htc-vive-tutorial-unity
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{

    // Use this for initialization
    private SteamVR_TrackedObject trackedObj;

    // Store the GameObject that the trigger is currently colliding with,
    //  so we can grab the object
    private GameObject collidingObject;
    // Serves as a reference to the GameObject that the player is currently
    //  grabbing
    private GameObject objectInHand;

	private	GameObject cube;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Accepts Collider as parameter and uses its GameObject as the collidingObject
    //  for grabbing and releasing
    private void SetCollidingObject(Collider col)
    {
        // Doesn't make the GameObject a potential grab target if player is
        //  holding something or the object has no rigid body
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // Assigns the object as a potential grab target
        collidingObject = col.gameObject;
    }

    // Handle what should happen when the trigger collider enters and exits
    //  another colider
    // When the trigger collider enters another, sets up the collider as
    //  a potential grab target
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // Ensures the target is set when the player holds a controller over an object.
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // When the collider exits an object, abandoning an ugrabbed target.
    // Removes it's target by setting it to null
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        // Move the GameObject inside the player's hand and remove it from
        //  the collidingObject variable
        objectInHand = collidingObject;
		print ("Object in hand: " + objectInHand);
        collidingObject = null;
        // Add a new joint that connects the controller to the object
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    // Make a new fixed joint, add it to the controller, and then set it up
    //  so it doesn't break easily.
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    // Handles releasing the object
    private void ReleaseObject()
    {
        // Check if fixed joint is attached to the controller
        if (GetComponent<FixedJoint>())
        {
            // Remove the connection to the object held by the joint
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // Add the speed and rotation of the controller when the user
            //  releases the object
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        // Remove the reference to the formerly attached object.
        objectInHand = null;
    }

	void ScaleUp(){
		objectInHand.transform.localScale += new Vector3 (.5f, .5f, .5f);
		//objectInHand.GetComponent<BoxCollider>().size += new Vector3 (.1f, .1f, .1f);
	}

	void ScaleDown(){
		objectInHand.transform.localScale -= new Vector3 (.5f, .5f, .5f);
		//objectInHand.GetComponent<BoxCollider>().size -= new Vector3 (.1f, .1f, .1f);
	}

    // Update is called once per frame
    void Update()
	{
		if (Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
		{
			if (collidingObject)
			{
				GrabObject();
			}
		}

		if (Controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
		{
			if (objectInHand)
			{
				ReleaseObject();
			}
		}

		if (Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0) [0] > 0.8f
		   && Controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
			print ("here1");
			ScaleUp();
		}

		if (Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0) [0] < 0.8f
			&& Controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)) {
			print ("here2");
			ScaleDown();
		}

		if (Controller.GetHairTriggerDown ()) {
			Vector3 startPos = transform.position;
			Vector3 endPos = startPos + transform.forward * 4f;

			cube = GameObject.CreatePrimitive (PrimitiveType.Cube);

			cube.transform.position = new Vector3(endPos[0],.5f,endPos[2]);
			cube.transform.localScale = new Vector3 (.5f, .1f, .5f);

			Destroy (cube.GetComponent<Rigidbody> ());
			Destroy (cube.GetComponent<BoxCollider> ());

			cube.GetComponent<Renderer> ().material.color = Color.green;
		}

		if (Controller.GetHairTriggerUp ()) {
			transform.parent.position = cube.transform.position;
			Destroy (cube);
		}
    }
}
