using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ActionState{
	Idle,
	Loading,
	Loaded,
	Firing,
	Fired
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

	// Global Clubs properties
	public float velocityLoading;
	public float velocityShooting;	
	public float minAngle;
	public float midAngle;
	public float maxAngle;

	//GUI	
	public GameObject FadePlane;
	public float FadeSpeed;	
	public GameObject buttonLeft;
	public GameObject buttonRight;
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
	private MeshRenderer ballRenderer;
	private AudioSource ballAudioSource;
	private Color ballOriginalColor;
	private Color ballCurrentColor;
	private bool ballIsWatched;
	private bool ballIsShooted;
	private bool ballIsOnGround;
	
	//GUI
	private Material fadePlaneMaterial;
	private float fadeAlphaValue;
	private Button buttonLeftBtn;
	private Button buttonRightBtn;
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
		initialRotation = playerTransf.rotation;
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
		ballRenderer = Ball.GetComponent<MeshRenderer> ();
		ballAudioSource = Ball.GetComponent<AudioSource> ();
		ballOriginalColor = ballCurrentColor = ballRenderer.material.color;
		ballIsWatched = false;
		ballIsShooted = false;
		ballIsOnGround = true;

		/*
		 * GUI
		 */
		FadePlane.SetActive (false);
		fadePlaneMaterial = FadePlane.GetComponent<Renderer>().material;
		buttonLeftBtn = buttonLeft.GetComponent<Button> ();
		buttonRightBtn = buttonRight.GetComponent<Button> ();
		buttonsColor = buttonLeftBtn.image.color;
		isGuiVisible = true;
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
					buttonLeft.SetActive(true);
					buttonRight.SetActive(true);
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
				}
				else
				{
					currentAction = ActionState.Loaded;
				}
				
				if(isGuiVisible)
				{
					isGuiVisible = false;
					buttonLeft.SetActive(false);
					buttonRight.SetActive(false);
					currentMovement = MovementState.None;
				}
				
				ballCurrentColor = new Color(ballCurrentColor.r += 0.05f, ballCurrentColor.g, ballCurrentColor.b);
				ballRenderer.material.color = ballCurrentColor;
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
				if(clubTransf.localRotation.z > minAngle)
				{
					clubTransf.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * timeLoading);
					if(!ballIsShooted && clubTransf.localRotation.z < midAngle)							//Shoot now
					{
						ShootBall(timeLoading, playerTransf.eulerAngles.y);
						ballIsShooted = true;
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
				
				ballCurrentColor = ballOriginalColor;
				ballRenderer.material.color = ballCurrentColor;
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
					}
					if(clubTransf.localRotation.z > midAngle)
					{
						currentAction = ActionState.Idle;
					}
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
				buttonRightBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
				buttonLeftBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
			break;

			/*
			 * Turn left
			 */
			case MovementState.TurnLeft:
				buttonLeftBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f); 
				playerTransf.RotateAround (Ball.transform.position, Vector3.up, -rotateAroundBallVelocity);
				angleRotationAroundBall -= rotateAroundBallVelocity;
				break;

			/*
			 * Turn right
			 */
			case MovementState.TurnRight:
				buttonRightBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f);
				playerTransf.RotateAround (Ball.transform.position, Vector3.up, rotateAroundBallVelocity);
				angleRotationAroundBall += rotateAroundBallVelocity;
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
			break;
		}
	}




	/*
	 * Watching ball (shoot/release)
	 */
	public void LoadShoot(){
		ballIsWatched = true;
		if (currentAction == ActionState.Idle)
			currentAction = ActionState.Loading;
		/*
		 * TODO: Sounds
		 */
	}

	public void ReleaseShoot(){
		ballIsWatched = false;
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
