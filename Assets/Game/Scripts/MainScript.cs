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
	MoveToTheBall,
	TurnLeft,
	TurnRight,
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

	// Global Clubs properties
	public float velocityLoading;
	public float velocityShooting;	
	public float minAngle;
	public float midAngle;
	public float maxAngle;

	//GUI
	public GameObject buttonLeft;
	public GameObject buttonRight;
	public float moveToBallVelocity;

	/*
	 * Private properties
	 */	
	//Player
	private Transform playerTransf;
	private Vector3 playerOffsetWithBall;

	//Club
	private ClubProperties clubProperties;
	private Transform clubTransf;
	private Quaternion clubDefaultRotation;
	private float timeLoading;

	//Ball
	private Rigidbody ballRigidBody;
	private MeshRenderer ballRenderer;
	private AudioSource ballAudioSource;
	private Color ballOriginalColor;
	private Color ballCurrentColor;
	private bool ballIsWatched;
	private bool ballIsShooted;


	//GUI
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

		/*
		 * 	Club
		 */
		clubProperties = Club.GetComponent<ClubProperties> ();
		clubTransf = Club.transform;
		clubDefaultRotation = new Quaternion(clubTransf.localRotation.x, clubTransf.localRotation.y, clubTransf.localRotation.z, clubTransf.localRotation.w);

		/*
		 * Ball
		 */
		ballRigidBody = Ball.GetComponent<Rigidbody> ();
		ballRenderer = Ball.GetComponent<MeshRenderer> ();
		ballAudioSource = Ball.GetComponent<AudioSource> ();
		ballOriginalColor = ballCurrentColor = ballRenderer.material.color;
		ballIsWatched = false;
		ballIsShooted = false;


		/*
		 * GUI
		 */
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
				if(!isGuiVisible)
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
				ballAudioSource.Play();
					clubTransf.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * timeLoading);
					if(!ballIsShooted && clubTransf.localRotation.z < midAngle)							//Shoot now
					{
						ShootBall(timeLoading, playerTransf.eulerAngles.y);
						ballIsShooted = true;
					}
					else if (clubTransf.localRotation.z <= minAngle)
					{
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
				if(ballRigidBody.velocity.magnitude < 0.1f)
				{
					clubTransf.localRotation = Quaternion.RotateTowards(clubTransf.localRotation, clubDefaultRotation, 10f);
					if(currentMovement != MovementState.MoveToTheBall)
					{
						currentMovement = MovementState.MoveToTheBall;
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
				playerTransf.RotateAround (Ball.transform.position, Vector3.up, -1);
			break;

			/*
			 * Turn right
			 */
			case MovementState.TurnRight:
				playerTransf.RotateAround (Ball.transform.position, Vector3.up, 1);
				buttonRightBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f); 
			break;

			/*
			 * Move toward the ball
			 */
			case MovementState.MoveToTheBall:
				float step = moveToBallVelocity * Time.deltaTime;
				Player.transform.position = Vector3.MoveTowards (Player.transform.position, Ball.transform.position + playerOffsetWithBall, step);
				if(transform.position == Ball.transform.position + playerOffsetWithBall)
					currentMovement = MovementState.MoveToTheBall;
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
