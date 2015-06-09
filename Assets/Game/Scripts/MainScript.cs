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
	Won
}

public enum MovementState{
	TurnLeft,
	TurnRight,
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
	public GameObject Player;	
	public GameObject Club;
	public GameObject Ball;
	public Terrain Terrain;
	public List<HoleScript> Holes;

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
	public Image PowerBar;
	public float rotateAroundBallVelocity;
	public float moveToBallVelocity;

	/*
	 * Private properties
	 */	
	//Player
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
	private Rigidbody ballRigidBody;
	private TrailRenderer ballTrail;
	private float ballOldTrailTime;
	private AudioSource ballAudioSource;
	private bool ballIsShooted;
	private bool ballIsOnGround;
	private HoleScript currentHole;
	
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
		ballRigidBody = Ball.GetComponent<Rigidbody> ();
		ballTrail = Ball.GetComponent<TrailRenderer> ();
		ballOldTrailTime = ballTrail.time;
		ballAudioSource = Ball.GetComponent<AudioSource> ();
		ballIsShooted = false;
		ballIsOnGround = true;
		currentHole = Holes [0];

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
					ballTrail.enabled = false;
					ballTrail.time = 0;
					currentAction = ActionState.Fired;
					Ball.transform.position = currentHole.BeginPosition.transform.position;
					ballRigidBody.velocity = new Vector3(0f, 0f, 0f);
					ballRigidBody.angularDrag = 20f;
					ballIsOnGround = false;
				return;
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
			/*	
			case MovementState.None:
				buttonRightBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
				buttonLeftBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
			break;
			*/
		
			/*
			 * Turn left
			 */
			/*
			case MovementState.TurnLeft:
			//	buttonLeftBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f); 
				playerTransf.RotateAround (Ball.transform.position, Vector3.up, -rotateAroundBallVelocity * Time.deltaTime);
				angleRotationAroundBall -= rotateAroundBallVelocity;
				break;
			*/

			/*
			 * Turn right
			 */
			/*
			case MovementState.TurnRight:
			//	buttonRightBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f);
				playerTransf.RotateAround (Ball.transform.position, Vector3.up, rotateAroundBallVelocity * Time.deltaTime);
				angleRotationAroundBall += rotateAroundBallVelocity;
			break;
			*/

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
				Player.transform.position = initialPosition;
				Player.transform.rotation = initialRotation;
				Player.transform.position = Ball.transform.position + playerOffsetWithBall;
				playerTransf.RotateAround(Ball.transform.position, Vector3.up, angleRotationAroundBall);
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
				}
				if(!ballTrail.enabled)
				{
					ballTrail.enabled = true;
					ballTrail.time = ballOldTrailTime;
				}
			break;
		}

		/*
		 * 	New rotation system (Every value here is in degree°)
		 */
		var headRotation = Cardboard.SDK.HeadRotation.eulerAngles;		// Head rotation
		var horizontalNeckRotation = headRotation.y;					// Neck vector
		var forwardNeckRotation = headRotation.x;						// x rotation of the neck (forward) 

		var horizontalRotationThreshold = 20; 							// The left right scope of the player

		var forwardRotationThresholdMin = 20; 							// Player look in direction of the ground/ball
		var forwardRotationThresholdMax = 90; 	

		var playerRot = playerTransf.localRotation.eulerAngles.y - initialRotation.eulerAngles.y; //The player rotation without the initial rotation


		Debug.Log ("Head: " + headRotation + "; PlayerRot: " + playerRot);

		// Player look at the horizon (we follow his neck rotation)
		if (forwardNeckRotation < forwardRotationThresholdMin || forwardNeckRotation > forwardRotationThresholdMax) 
		{
			playerTransf.rotation = Quaternion.Euler(0, horizontalNeckRotation, 0);
			Debug.Log ("Look at the Horizon");
		}
		// Player look at the ground we follow his neck
		else
		{
			Debug.Log ("Look at the Ground");
		}
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

	public void EnterHole(int holeNumber)
	{
		foreach (HoleScript hole in Holes) 
		{
			if(hole.HoleNumber==holeNumber+1)
			{
				currentHole = hole;
				currentAction = ActionState.Won;
			}
		}
	}

	/*
	 * Button Left/Right
	 */
	public void WatchButtonLeft(){
		currentMovement = MovementState.TurnLeft;
		/*
		 * TODO: Sounds
		 */
	}

	public void UnWatchButtonLeft(){
		currentMovement = MovementState.None;
		/*
		 * TODO: Sounds
		 */
	}

	public void WatchButtonRight(){
		currentMovement = MovementState.TurnRight;
		/*
		 * TODO: Sounds
		 */
	}
	
	public void UnWatchButtonRight(){
		currentMovement = MovementState.None;
		/*
		 * TODO: Sounds
		 */
	}
}
