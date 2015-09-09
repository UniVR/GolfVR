using UnityEngine;
using System.Collections;

public class CardboardScript : MonoBehaviour {

	public float WatchDownAngle;
	public float angle;

	private PlayerScript player;

	private bool lookDown;
	private float lastRotation;
	private bool locked;

	void Start () {
		player = MainScript.Get ().Player;
		lookDown = false;
		locked = false;
	}
	
	/*
	 * 	Rotation system
	 */
	void Update () {
		var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;				// Head rotation
			
		if (locked) {
			transform.eulerAngles = new Vector3 (headRotation.x, headRotation.y, headRotation.z);
			return;
		}

		var forwardRotationThresholdMin = 10; 											// Player look in direction of the ground/ball
		var forwardRotationThresholdMax = 90; 	

		// Player look at the horizon (normal rotation around the ball)
		var lookHorizontally = headRotation.x < forwardRotationThresholdMin || headRotation.x > forwardRotationThresholdMax;
		if (lookHorizontally) {
			player.transform.eulerAngles = new Vector3 (0, headRotation.y, 0);
			if(lookDown){ //Fix the jump glitch
				lookDown = false;
				transform.eulerAngles = new Vector3 (headRotation.x, lastRotation, headRotation.z);
			}

			transform.eulerAngles = new Vector3 (headRotation.x, transform.eulerAngles.y, headRotation.z);
		}  																				// Player look at the ground we follow his neck direction (Horizontal projection of the neck vector)
		else { 
			lookDown= true;
			var neckVector = Cardboard.SDK.HeadPose.Orientation * Vector3.up;			// Neck vector
			var direction = new Vector3 (neckVector.x, 0, neckVector.z);

			player.transform.rotation = Quaternion.LookRotation (direction); 			// TODO: add a little threshold?
			transform.eulerAngles = new Vector3 (headRotation.x, headRotation.y, headRotation.z);
			lastRotation = headRotation.y;
		}
	}

	public bool WatchDown ()
	{
		var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;
		angle = headRotation.x;
		return headRotation.x > WatchDownAngle && headRotation.x<180;
	}

	public void Lock(bool locked){
		this.locked = locked;
	}
}
