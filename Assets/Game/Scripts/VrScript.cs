using UnityEngine;
using System.Collections;

public class VrScript : MonoBehaviour {

	public float ForwardRotationThresholdMin;
	public float ForwardRotationThresholdMax; 

	private PlayerScript player;
	private GameObject parent;

	private GameObject DebugLog;

	void Start () {
		player = MainScript.Get ().Player;
		parent = gameObject.transform.parent.gameObject;
		DebugLog = GameObject.Find ("DebugLog");
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

		Vector3 forwardVector;
		if (forwardAngle < ForwardRotationThresholdMin) 
		{
			forwardVector = faceForward;
		}
		else if (forwardAngle > ForwardRotationThresholdMax) 
		{
			forwardVector = neckForward;
		} 
		else
		{
			var factor = (forwardAngle - ForwardRotationThresholdMin) / (ForwardRotationThresholdMax - ForwardRotationThresholdMin);
			forwardVector = Vector3.Lerp (faceForward, neckForward, 0);
			//Debug.Log ("factor: " + factor);
			Debug.Log ("forward: " + forwardVector);
		}
			
		var forwardRotation = Quaternion.LookRotation (forwardVector).eulerAngles;
		var rotatedBy = forwardRotation.y - player.transform.eulerAngles.y;
		player.transform.Rotate(0, rotatedBy, 0);
		parent.transform.Rotate(0, - rotatedBy, 0);
	}
}
