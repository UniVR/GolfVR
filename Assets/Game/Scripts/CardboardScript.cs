using UnityEngine;
using System.Collections;

public class CardboardScript : MonoBehaviour {

	public float SmoothFactor;
	public float ForwardRotationThresholdMin;
	public float ForwardRotationThresholdMax; 

	public bool OldSystem;

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

		if (!OldSystem) {
			var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;				// Head rotation			
			var neckVector = Cardboard.SDK.HeadPose.Orientation * Vector3.up;			// Neck vector

			var direction = new Vector3 (neckVector.x, 0, neckVector.z);
			//Debug.Log("direction" + direction + "; Headrotation: " + headRotation);
		
			var lookHorizontally = headRotation.x < ForwardRotationThresholdMin || headRotation.x > ForwardRotationThresholdMax;
			if (!lookHorizontally) {
				lookDown = true;
				var playerTarget = Quaternion.LookRotation (direction);
				player.transform.rotation = Quaternion.Slerp (player.transform.rotation, playerTarget, Time.deltaTime * SmoothFactor);
			} else {
				if (lookDown) { //Fix the jump glitch
					lookDown = false;
					transform.eulerAngles = new Vector3 (headRotation.x, lastRotation, headRotation.z);
				}
				player.transform.rotation = Quaternion.Slerp (player.transform.rotation, Quaternion.Euler (0, headRotation.y, 0), Time.deltaTime * SmoothFactor);
			}

			var target = Quaternion.Euler (headRotation.x, headRotation.y, headRotation.z);
			transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * SmoothFactor);
			lastRotation = transform.rotation.y;
			//transform.eulerAngles = new Vector3 (headRotation.x, headRotation.y, headRotation.z);

		}else{
			/*
			 * 	OLD SYSTEM
			 */
			var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;				// Head rotation
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
	}
}
