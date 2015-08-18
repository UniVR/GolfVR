using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionState{
	Idle,
	Loading,
	Loaded,
	Firing,
	Fired, 
	Won,
	OutOfBound
}

public enum MovementState{
	FadeIn,
	FadeOut,
	MoveToTheBall,
	None
}

public class MainScript : MonoBehaviour {

	[HideInInspector]
	public ActionState currentAction;

	[HideInInspector]
	public MovementState currentMovement;

	/*
	 * 	Public Properties
	 */
	public GameObject CardboardGameObject;
	public GameObject Player;	
	public GameObject Club;
	public GameObject Ball;
	public HolesListScript Holes;

	// Global Clubs properties
	public float velocityLoading;
	public float velocityShooting;	
	public float minAngle;
	public float midAngle;
	public float maxAngle;

	//GUI	
	public GameObject FadePlane;
	public float FadeSpeed;
	public Text ScoreHUD;
	public Text InformationsHUD;
	public Image PowerBar;
	public float rotateAroundBallVelocity;
	public float moveToBallVelocity;

	/*
	 * Private properties
	 */	
	//Player
	private CardboardHead cardBoardHead;
	private Transform playerTransf;
	private Vector3 playerOffsetWithBall;
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private float angleRotationAroundBall;

	//Club
	private ClubProperties clubProperties;
	private Transform clubTransf;
	private Quaternion clubDefaultRotation;
	private float timeLoading;

	//Ball
	private DetectTerrainType detectTerrainType;
	private Transform ballTransf;
	private Vector3 ballOldPos;
	private Rigidbody ballRigidBody;
	private TrailRenderer ballTrail;
	private float ballOldTrailTime;
	private AudioSource ballAudioSource;
	private bool ballIsShooted;
	private bool ballIsOnGround;
	
	//GUI
	private Material fadePlaneMaterial;
	private float fadeAlphaValue;
	private int score;
	private Color buttonsColor;
	private bool isGuiVisible;
	

	/*
	 * Initialisation
	 */
	void Start () {
		currentAction = ActionState.Idle;
		currentMovement = MovementState.None;

		/*
		 * Player
		 */		
		cardBoardHead = CardboardGameObject.GetComponentInChildren<CardboardHead> ();
		playerTransf = Player.transform;
		playerOffsetWithBall = Player.transform.position - Ball.transform.position;
		initialPosition = playerTransf.position;
		initialRotation = playerTransf.localRotation;
		angleRotationAroundBall = 0;

		/*
		 * 	Club
		 */
		clubProperties = Club.GetComponent<ClubProperties> ();
		clubTransf = Club.transform;
		clubDefaultRotation = new Quaternion(clubTransf.localRotation.x, clubTransf.localRotation.y, clubTransf.localRotation.z, clubTransf.localRotation.w);

		/*
		 * Ball
		 */
		detectTerrainType = GetComponent<DetectTerrainType> ();
		ballTransf = Ball.transform;
		ballRigidBody = Ball.GetComponent<Rigidbody> ();
		ballTrail = Ball.GetComponent<TrailRenderer> ();
		ballOldTrailTime = ballTrail.time;
		ballAudioSource = Ball.GetComponent<AudioSource> ();
		ballIsShooted = false;
		ballIsOnGround = true;

		/*
		 * GUI
		 */
		FadePlane.SetActive (false);
		fadePlaneMaterial = FadePlane.GetComponent<Renderer>().material;
		PowerBar.enabled = false;
		isGuiVisible = true;
		score = 0;
		ScoreHUD.text = "Score: " + score;	
	}


	void FixedUpdate () {
		/*
		 * 	Action
		 */
		switch (currentAction) 
		{
			/*
			 * Idle
			 */
			case ActionState.Idle:
				if(!isGuiVisible 
			   		&& currentMovement!=MovementState.FadeIn
			   		&& currentMovement!=MovementState.FadeOut
			   		&& currentMovement!=MovementState.MoveToTheBall)
				{
					isGuiVisible = true;
				}
			break;

			/*
			 * Loading
			 */
			case ActionState.Loading:
				if(clubTransf.localRotation.z < maxAngle)
				{
					timeLoading += Time.deltaTime;
					clubTransf.Rotate (Vector3.down * Time.deltaTime * velocityLoading);
					
					PowerBar.enabled = true;
					PowerBar.fillAmount = Mathf.InverseLerp(midAngle, maxAngle, clubTransf.localRotation.z);
					//PowerBarMaterial.SetFloat("_Cutoff", 1f - Mathf.InverseLerp(maxAngle, midAngle, clubTransf.localRotation.z)); 
				}
				else
				{
					currentAction = ActionState.Loaded;
				}
				
				if(isGuiVisible)
				{
					isGuiVisible = false;
					currentMovement = MovementState.None;
				}
			break;

			/*
			 * Loaded
			 */
			case ActionState.Loaded:
			break;

			/*
			 * Firing
			 */
			case ActionState.Firing:
				if(PowerBar.enabled)
					PowerBar.enabled = false;
				
				if(clubTransf.localRotation.z > minAngle)
				{
					clubTransf.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * timeLoading);
					if(!ballIsShooted && clubTransf.localRotation.z < midAngle)							//Shoot now
					{
						ballOldPos = ballTransf.position;
						ShootBall(timeLoading, playerTransf.eulerAngles.y);
						ballIsShooted = true;
						score++;
						ScoreHUD.text = "Score: " + score;	
					}
					else if (clubTransf.localRotation.z <= minAngle)
					{					
						if(ballIsOnGround)
							detectTerrainType.SetBallDrag();
						currentAction = ActionState.Fired;
						ballIsShooted = false;
						timeLoading = 0;
					}
				}
			break;

			/*
			 * Fired
			 */
			case ActionState.Fired:					
				if(ballIsOnGround)
					detectTerrainType.SetBallDrag();

				if(detectTerrainType.IsOutOfBound()){
					currentAction = ActionState.OutOfBound;
					break;
				}		   		
				if(ballRigidBody.velocity.magnitude < 0.1f)
				{
					clubTransf.localRotation = Quaternion.RotateTowards(clubTransf.localRotation, clubDefaultRotation, 10f);
					if(currentMovement != MovementState.FadeOut)
					{
						fadeAlphaValue = 0;
						FadePlane.SetActive (true);
						currentMovement = MovementState.FadeOut;
						ballRigidBody.velocity = new Vector3(0f, 0f, 0f);
					}
					if(clubTransf.localRotation.z > midAngle)
					{
						currentAction = ActionState.Idle;
					}
				}				
			break;

			case ActionState.Won:	
				currentAction = ActionState.Fired;
				Ball.transform.position = Holes.CurrentHole.BeginPosition.transform.position;
				ballRigidBody.velocity = new Vector3(0f, 0f, 0f);
				ballRigidBody.angularDrag = 20f;		
				ballTrail.enabled = false;
				ballTrail.time = 0;
				ballIsOnGround = true;
			break;

			case ActionState.OutOfBound:	
				InformationsHUD.enabled = true;
				score++;
				ScoreHUD.text = "Score: " + score;	
				Ball.transform.position = ballOldPos;
				ballRigidBody.velocity = new Vector3(0f, 0f, 0f);
				ballRigidBody.angularDrag = 20f;		
				ballTrail.enabled = false;
				ballTrail.time = 0;
				ballIsOnGround = true;

				if(currentMovement != MovementState.FadeOut){
					fadeAlphaValue = 0;
					FadePlane.SetActive (true);
					currentMovement = MovementState.FadeOut;
				}
				else{
					currentAction = ActionState.Fired;
				}
			break;
		}

		/*
		 * 	Movement
		 */
		switch (currentMovement)
		{
			/*
			 * 	None
			 */	
			case MovementState.None:
				/*
				 * 	New rotation system (Every value here is in degree°)
				 */
				var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;		// Head rotation
				var horizontalNeckRotation = headRotation.y;					// y rotation of the neck (horizontally)
				var forwardNeckRotation = headRotation.x;						// x rotation of the neck (forward) 
				var neckVector = Cardboard.SDK.HeadPose.Orientation  * Vector3.up;		// Neck vector
				
				var forwardRotationThresholdMin = 10; 							// Player look in direction of the ground/ball
				var forwardRotationThresholdMax = 90; 	

				// Player look at the horizon (normal rotation around the ball)
				var lookHorizontally = forwardNeckRotation < forwardRotationThresholdMin || forwardNeckRotation > forwardRotationThresholdMax;
				if (lookHorizontally) 
				{
					playerTransf.eulerAngles = new Vector3(0, horizontalNeckRotation, 0);
				}
				else // Player look at the ground we follow his neck direction (Horizontal projection of the neck vector)
				{
					var direction = new Vector3(neckVector.x, 0, neckVector.z);
					playerTransf.rotation = Quaternion.LookRotation(direction); // TODO: add a little threshold?
				}

				//Cardboard top/bottom rotation
				var cardBoardVect = CardboardGameObject.transform.eulerAngles;
				CardboardGameObject.transform.eulerAngles = new Vector3(forwardNeckRotation, cardBoardVect.y, cardBoardVect.z);
			break;
		
			/*
			 * 	Fade Out
			 */
			case MovementState.FadeOut:
				fadeAlphaValue += Time.deltaTime / FadeSpeed;
				fadePlaneMaterial.color = new Color(fadePlaneMaterial.color.r, fadePlaneMaterial.color.g, fadePlaneMaterial.color.b, fadeAlphaValue);
				if(fadeAlphaValue>=1f)
					currentMovement = MovementState.MoveToTheBall;
			break;

			/*
			 * Move toward the ball
			 */
			case MovementState.MoveToTheBall:
				Player.transform.position = Ball.transform.position;
		//TODO: find how rotate the player to front the ball... !
				//Player.transform.LookAt(Holes.CurrentHole.transform);
				//cardBoardHead.transform.LookAt(Holes.CurrentHole.transform.position);
				//Cardboard.SDK.HeadPose.Orientation.SetLookRotation(Holes.CurrentHole.transform.position);
				//var vectorHeadToHole = Holes.CurrentHole.transform.position - Cardboard.SDK.HeadPose.Position;
				//Cardboard.SDK.HeadPose.Orientation.SetLookRotation(vectorHeadToHole);
				//var vectorHeadToHole = Holes.CurrentHole.transform.position - Cardboard.SDK.HeadPose.Position;
				//var orientation = Quaternion.LookRotation(vectorHeadToHole, Vector3.up);
				//((MutablePose3D) Cardboard.SDK.HeadPose).Set(Cardboard.SDK.HeadPose.Position, orientation);	
				//Cardboard.SDK.HeadPose.Orientation.Set(orientation.x, orientation.y, orientation.z, orientation.w);
				//Player.transform.LookAt(Holes.CurrentHole.transform);
				//Debug.Log("########### MOVE !");
				currentMovement = MovementState.FadeIn;
			break;

			/*
			 * 	Fade In
			 */
			case MovementState.FadeIn:
				fadeAlphaValue -= Time.deltaTime / FadeSpeed;
				fadePlaneMaterial.color = new Color(fadePlaneMaterial.color.r, fadePlaneMaterial.color.g, fadePlaneMaterial.color.b, fadeAlphaValue);
				if(fadeAlphaValue<=0f){
					FadePlane.SetActive (false);
					currentMovement = MovementState.None;
					InformationsHUD.enabled = false;	
				}
				if(!ballTrail.enabled)
				{
					ballTrail.enabled = true;
					ballTrail.time = ballOldTrailTime;
				}
			break;
		}

		//Debug.Log("NECK: " + Cardboard.SDK.HeadPose.Orientation  * Vector3.up);
	}




	public HoleScript GetCurrentHole(){
		return Holes.CurrentHole;
	}

	/*
	 * Watching ball (shoot/release)
	 */
	public void LoadShoot(){
		//ballIsWatched = true;
		if (currentAction == ActionState.Idle)
			currentAction = ActionState.Loading;
		/*
		 * TODO: Sounds
		 */
	}

	public void ReleaseShoot(){
		//ballIsWatched = false;
		if (currentAction == ActionState.Loading || currentAction == ActionState.Loaded)
			currentAction = ActionState.Firing;
		/*
		 * TODO: Sounds
		 */
	}

	public void ShootBall (float magnitude, float orientation)
	{
		Vector3 direction = Quaternion.Euler(0, orientation, -clubProperties.clubAngle) * new Vector3 (-1, 0, 0);
		ballRigidBody.AddForce(direction * magnitude * clubProperties.clubForceCoef, ForceMode.Impulse);

		ballAudioSource.Play();
	}


	/*
	 * 	Events
	 */
	public void CollisionEnter(GameObject source, Collision collision)
	{
		ballIsOnGround = true;
	}

	public void CollisionExit (GameObject source, Collision collision)
	{
		ballIsOnGround = false;
	}

	public void EnterHole()
	{
		currentAction = ActionState.Won;
	}

	public void Win(){
		// TODO
	}
}
