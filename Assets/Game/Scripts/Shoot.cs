using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject Pivot;
	public float velocity;

	public float minAngle;
	public float maxAngle;

	public enum State{
		Idle,
		Loading,
		Loaded,
		Firing,
		Fired
	}
	public State currentState;
	private Transform pivot;
	private Transform clubTransf;
	private Rigidbody clubBody;

	// Use this for initialization
	void Start () {
		clubTransf = GetComponent<Transform> ();
		pivot = Pivot.GetComponent<Transform> ();
		currentState = State.Idle;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (currentState == State.Loading && isRotationInRange (pivot.eulerAngles)) {
			pivot.RotateAround (pivot.position, pivot.eulerAngles, Time.deltaTime * velocity);
		} else if (currentState == State.Loading) {
			currentState = State.Loaded;
		} else if (currentState == State.Firing && isRotationInRange (pivot.eulerAngles)) {
			pivot.RotateAround (pivot.position, pivot.eulerAngles, Time.deltaTime * velocity);
		} else if (currentState == State.Firing) {
			currentState = State.Fired;
		}
	}

	public void LoadShot(){
		if (currentState == State.Idle)
			currentState = State.Loading;
	}

	public void ReleaseShot(){
		if (currentState == State.Loading || currentState == State.Loaded)
			currentState = State.Firing;
	}


	private bool isRotationInRange(Vector3 quat){
		return quat.z > minAngle && quat.z < maxAngle;
			//&& quat.y > minAngle && quat.y < maxAngle
			//&& quat.z > minAngle && quat.z < maxAngle
			//&& quat.w > minAngle && quat.w < maxAngle;
	}

	public int debugView_enumName {get {return (int) currentState; }}
}
