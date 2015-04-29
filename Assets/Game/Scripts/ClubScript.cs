using UnityEngine;
using System.Collections;

/// <summary>
/// *** Shoot ***
/// Control the club. Load/Release the shoot
/// </summary>
public class ClubScript : MonoBehaviour {
	public enum State{
		Idle,
		Loading,
		Loaded,
		Firing,
		Fired
	}	

	/*
	 * 	Ball and player
	 */
	public GameObject ball;
	public GameObject player;

	private Rigidbody ballRigidBody;
	private MovePlayer movePlayerScript;

	/*
	 * 	Club properties
	 */
	public float clubForceCoef;
	public float clubAngle;

	public float velocityLoading;
	public float velocityShooting;

	public float minAngle;
	public float midAngle;
	public float maxAngle;

	/*
	 * 	Visual informations (debug purpose)
	 */
	public string Information = "Don't modify next values... !";
	public Quaternion Rotation;
	
	public State currentState;

	/*
	 * 	Runtime variables
	 */
	private float timeLoading;
	private Transform clubTransf;
	private Quaternion clubDefaultRotation;

	private bool ballShooted;

	void Start () {
		clubTransf = GetComponent<Transform> ();
		clubDefaultRotation = new Quaternion(clubTransf.localRotation.x, clubTransf.localRotation.y, clubTransf.localRotation.z, clubTransf.localRotation.w);
		ballRigidBody = ball.GetComponent<Rigidbody> ();
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
			clubTransf.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * timeLoading);
			if(!ballShooted && clubTransf.localRotation.z < midAngle)							//Shoot now
			{
				ShootBall(timeLoading, player.transform.eulerAngles.y);
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
				timeLoading = 0;
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

	public void ShootBall (float magnitude, float orientation)
	{
		Vector3 direction = Quaternion.Euler(0, orientation, -clubAngle) * new Vector3 (-1, 0, 0);
		ballRigidBody.AddForce(direction * magnitude * clubForceCoef, ForceMode.Impulse);	
	}


	public int debugEnum_currentState {get {return (int) currentState; }}
}
