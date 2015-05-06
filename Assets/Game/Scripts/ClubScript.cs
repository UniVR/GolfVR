using UnityEngine;
using System.Collections;

/// <summary>
/// *** Shoot ***
/// Control the club. Load/Release the shoot
/// </summary>
public class ClubScript : MonoBehaviour {
	MainScript mainScript;

	/*
	 * 	Ball
	 */
	private GameObject ball;
	private Rigidbody ballRigidBody;

	/*
	 * 	Player
	 */
	private GameObject player;

	/*
	 * 	Club properties
	 */
	public float velocityLoading;
	public float velocityShooting;

	public float minAngle;
	public float midAngle;
	public float maxAngle;

	private float clubForceCoef;
	private float clubAngle;


	/*
	 * 	Visual informations (debug purpose)
	 */
	public string Information = "Don't modify next values... !";
	public Quaternion Rotation;	

	/*
	 * 	Runtime variables
	 */
	private float timeLoading;
	private Transform clubTransf;
	private Quaternion clubDefaultRotation;
	private bool ballShooted;

	void Start () {
		mainScript = GetComponent<MainScript> ();
		ball = mainScript.Ball;
		player = mainScript.Player;

		mainScript = GetComponent<MainScript> ();
		var clubProperties = mainScript.Club.GetComponent<ClubProperties> ();
		clubForceCoef = clubProperties.clubForceCoef;
		clubAngle = clubProperties.clubAngle;

		clubTransf = mainScript.Club.GetComponent<Transform> ();
		clubDefaultRotation = new Quaternion(clubTransf.localRotation.x, clubTransf.localRotation.y, clubTransf.localRotation.z, clubTransf.localRotation.w);

		ballRigidBody = mainScript.Ball.GetComponent<Rigidbody> ();

		ballShooted = false;
		timeLoading = 0;
	}


	void FixedUpdate () {
		ActionState currentState = mainScript.currentAction;
		Rotation = player.transform.rotation; //Informations


		/*
		 * 	Shooting process
		 */
		if (currentState == ActionState.Loading &&  clubTransf.localRotation.z < maxAngle ) 			//Loading
		{ 		
			timeLoading += Time.deltaTime;
			clubTransf.Rotate (Vector3.down * Time.deltaTime * velocityLoading);
		}
		else if (currentState == ActionState.Loading) 												//Loaded
		{										
			mainScript.currentAction = ActionState.Loaded;
		} 
		else if (currentState == ActionState.Firing && clubTransf.localRotation.z > minAngle )		//Shooting
		{ 	
			clubTransf.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * timeLoading);
			if(!ballShooted && clubTransf.localRotation.z < midAngle)							//Shoot now
			{
				ShootBall(timeLoading, player.transform.eulerAngles.y);
				ballShooted = true;
			}
		}
		else if (currentState == ActionState.Firing) {												//Fired									
			mainScript.currentAction = ActionState.Fired;
		}
		else if(currentState == ActionState.Fired && ballRigidBody.velocity.magnitude < 0.1f){		//Fired and velocity small
			clubTransf.localRotation = Quaternion.RotateTowards(clubTransf.localRotation, clubDefaultRotation, 10f);
			if(mainScript.currentMovement != MovementState.MoveToTheBall){
				mainScript.currentMovement = MovementState.MoveToTheBall;
			}
			if(clubTransf.localRotation.z > midAngle){
				mainScript.currentAction = ActionState.Idle;
				ballShooted = false;
				timeLoading = 0;
			}
		}
	}

	public void ShootBall (float magnitude, float orientation)
	{
		Vector3 direction = Quaternion.Euler(0, orientation, -clubAngle) * new Vector3 (-1, 0, 0);
		ballRigidBody.AddForce(direction * magnitude * clubForceCoef, ForceMode.Impulse);	
	}


	public int debugEnum_currentState {get {return (int) mainScript.currentAction; }}
}
