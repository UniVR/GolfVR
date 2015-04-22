using UnityEngine;
using System.Collections;

/// <summary>
/// *** Shoot ***
/// Control the club. Load/Release the shoot
/// </summary>
public class Shoot : MonoBehaviour {
	public enum State{
		Idle,
		Loading,
		Loaded,
		Firing,
		Fired
	}	

	public GameObject ball;
	private BallScript ballScript;
	private Rigidbody ballRigidBody;
	public GameObject player;
	private MovePlayer movePlayerScript;

	public float velocityLoading;
	private float timeLoading;
	public float coefShooting;

	public float minAngle;
	public float midAngle;
	public float maxAngle;

	public float yAngleCorrection;

	public string Information = "Don't modify next values... !";
	public Quaternion Rotation;
	
	public State currentState;
	private Transform clubTransf;
	private Quaternion clubDefaultRotation;

	private bool ballShooted;
	private ClubProperties prop;	

	void Start () {
		clubTransf = GetComponent<Transform> ();
		clubDefaultRotation = new Quaternion(clubTransf.localRotation.x, clubTransf.localRotation.y, clubTransf.localRotation.z, clubTransf.localRotation.w);
		prop = GetComponent<ClubProperties> ();
		ballRigidBody = ball.GetComponent<Rigidbody> ();
		ballScript = ball.GetComponent<BallScript> ();
		movePlayerScript = player.GetComponent<MovePlayer> ();
		currentState = State.Idle;
		timeLoading = 0;
		ballShooted = false;
	}


	void FixedUpdate () {
		Rotation = player.transform.rotation; //Informations

		/*
		 * 	Shooting process
		 */
		if (currentState == State.Loading &&  clubTransf.localRotation.z < maxAngle ) 			//Loading
		{ 		
			timeLoading += Time.deltaTime;
			clubTransf.Rotate (Vector3.down * Time.deltaTime * velocityLoading);
		}
		else if (currentState == State.Loading) 												//Loaded
		{										
			currentState = State.Loaded;
		} 
		else if (currentState == State.Firing && clubTransf.localRotation.z > minAngle )		//Shooting
		{ 	
			clubTransf.Rotate (-Vector3.down * Time.deltaTime * coefShooting * timeLoading);
			if(!ballShooted && clubTransf.localRotation.z < midAngle)							//Shoot now
			{
				ballScript.Shoot(timeLoading, player.transform.eulerAngles.y, prop);
				ballShooted = true;
			}
		}
		else if (currentState == State.Firing) {												//Fired									
			currentState = State.Fired;
		}
		else if(currentState == State.Fired && ballRigidBody.velocity.magnitude < 0.1f){		//Fired and velocity small
			movePlayerScript.MovePlayerToBall();
			clubTransf.localRotation = Quaternion.RotateTowards(clubTransf.localRotation, clubDefaultRotation, 10f);
			if(clubTransf.localRotation.z > midAngle){
				this.currentState = State.Idle;
				ballShooted = false;
			}
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
