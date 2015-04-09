using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public enum State{
		Idle,
		Loading,
		Loaded,
		Firing,
		Fired
	}	

	public float velocityLoading;
	private float timeLoading;
	public float coefShooting;

	public float minAngle;
	public float maxAngle;

	public string Information = "Don't modify next values... !";
	public Quaternion Rotation;
	
	public State currentState;
	private Transform clubTransf;
		
	void Start () {
		clubTransf = GetComponent<Transform> ();
		currentState = State.Idle;
		timeLoading = 0;
	}


	void FixedUpdate () {
		Rotation = clubTransf.rotation;

		if (currentState == State.Loading && clubTransf.rotation.z < maxAngle ) { 		//Loading
			timeLoading += Time.deltaTime;
			clubTransf.Rotate (Vector3.down * Time.deltaTime * velocityLoading);
		}else if (currentState == State.Firing && clubTransf.rotation.z > minAngle ) { 	//Shooting
			clubTransf.Rotate (-Vector3.down * Time.deltaTime * coefShooting * timeLoading);
		}else if (currentState == State.Loading) {										//Loaded
			currentState = State.Loaded;
		} else if (currentState == State.Firing) {										//Fired
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


	public int debugEnum_currentState {get {return (int) currentState; }}
}
