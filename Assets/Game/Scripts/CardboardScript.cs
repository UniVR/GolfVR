using UnityEngine;
using System.Collections;

public class CardboardScript : MonoBehaviour {

	private PlayerScript player;

	private bool lookDown;
	private float lastRotation;

	void Start () {
		player = MainScript.Get ().Player;
		lookDown = false;
	}
	
	/*
	 * 	Rotation system
	 */
	void Update () {
		var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;				// Head rotation
		
		var forwardRotationThresholdMin = 10; 											// Player look in direction of the ground/ball
		var forwardRotationThresholdMax = 90; 	
		
		// Player look at the horizon (normal rotation around the ball)
		var lookHorizontally = headRotation.x < forwardRotationThresholdMin || headRotation.x > forwardRotationThresholdMax;
		if (lookHorizontally) {
			player.transform.eulerAngles = new Vector3 (0, headRotation.y, 0);
			if(lookDown){
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
}
