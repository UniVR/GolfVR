using UnityEngine;
using System.Collections;

public class AnemoScript : MonoBehaviour {

	public GameObject Rotor;
	private float rotationSpeed;

	void Start(){
		rotationSpeed = 0f;
	}

	public void SetRotationSpeed(float speed){
		rotationSpeed = speed;
	}

	public void SetOrientation(float angle){
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
	}

	void FixedUpdate(){
		Rotor.transform.Rotate (0f, rotationSpeed, 0f);
	}
}
