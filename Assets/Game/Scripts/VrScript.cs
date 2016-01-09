using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VrScript : MonoBehaviour {

	private PlayerScript player;
	private GameObject parent;

	void Start () {
		player = MainScript.Get ().Player;
		parent = gameObject.transform.parent.gameObject;
	}
	
	/*
	 * 	Rotation system
	 */
	void Update () {

		// Neck vector
		var neckVector = gameObject.transform.rotation * Vector3.up;
		var neckForward = new Vector3 (neckVector.x, 0, neckVector.z);

		// Look direction
		var faceVector = gameObject.transform.forward;
		var faceForward = new Vector3 (faceVector.x, 0, faceVector.z);			

		//Angle forward (between 0 and 180)
		var forwardAngle = (gameObject.transform.rotation.eulerAngles.x + 90) % 360; 

		var headForward = gameObject.transform.rotation * Vector3.forward;
		var size = Mathf.Abs(neckForward.x) + Mathf.Abs(neckForward.z);
		Vector3 forwardVector;

		if (size > 1) {
			forwardVector = neckForward;
		}
		else if (size > 0 && headForward.y < 0.35) {
			forwardVector = Vector3.Lerp (neckForward, faceForward, 1-size);
		} 
		else {
			forwardVector = faceForward;
		}

		var forwardRotation = Quaternion.LookRotation (forwardVector).eulerAngles;
		var rotatedBy = forwardRotation.y - player.transform.eulerAngles.y;
		player.transform.Rotate(0, rotatedBy, 0);
		parent.transform.Rotate(0, - rotatedBy, 0);
	}
}
