using UnityEngine;
using System.Collections;

//The wind gameObject is just here for the tree to move
public class WindScript : MonoBehaviour {

	public float Velocity;
	public float Angle;

	void Start(){
		transform.eulerAngles = Quaternion.AngleAxis(Angle, Vector3.up) * Vector3.forward;
	}

	public void SetOrientation(float angle){
		this.Angle = angle;
		transform.eulerAngles = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
	}

	public float GetOrientation(){
		return Angle;
	}

	public void SetVelocity(float velocity){
		Velocity = velocity;
	}

	public Vector3 GetForce(){
		var direction = Quaternion.AngleAxis(Angle, Vector3.up) * Vector3.forward;
		return direction * Velocity;
	}
}
