using UnityEngine;
using System.Collections;

//The wind gameObject is just here for the tree to move
public class WindScript : MonoBehaviour {

	public float Velocity;
	public float Angle;

	void Start(){
		transform.rotation = Quaternion.AngleAxis(Angle, Vector3.up);
	}

	public void SetOrientation(float angle){
		this.Angle = angle;
		transform.rotation = Quaternion.AngleAxis(Angle, Vector3.up);
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
