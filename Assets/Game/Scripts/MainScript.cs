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
	 * 	Public Player/Club/Ball
	 */
	public GameObject Player;
	
	public GameObject Club;

	public GameObject Ball;

	/*
	 * public GUI
	 */
	public GameObject buttonLeft;
	public GameObject buttonRight;
	public float moveVelocity;

	/*
	 * Private properties of the player
	 */	
	private Transform playerPos;
	private Vector3 playerOffsetWithBall;

	/*
	 * 	Private properties of the ball
	 */
	private Rigidbody ballRigidBody;
	private MeshRenderer ballRenderer;
	private Color ballOriginalColor;
	private Color ballCurrentColor;
	private bool ballIsWatched;

	/*
	 * 	Private properties of the GUI
	 */
	private Button buttonLeftBtn;
	private Button buttonRightBtn;
	private Color buttonsColor;
	private bool hideGui;

	// Use this for initialization
	void Start () {
		currentAction = ActionState.Idle;
		currentMovement = MovementState.None;

		/*
		 * Player
		 */		
		playerPos = Player.transform;
		playerOffsetWithBall = Player.transform.position - Ball.transform.position;

		/*
		 * Ball properties initialization
		 */
		ballRigidBody = Ball.GetComponent<Rigidbody> ();
		ballRenderer = Ball.GetComponent<MeshRenderer> ();
		ballOriginalColor = ballCurrentColor = ballRenderer.material.color;
		ballIsWatched = false;

		/*
		 * GUI
		 */
		buttonLeftBtn = buttonLeft.GetComponent<Button> ();
		buttonRightBtn = buttonRight.GetComponent<Button> ();
		buttonsColor = buttonLeftBtn.image.color;
		hideGui = false;
	}

	void Update () {
	
		if (ballIsWatched) {
			ballCurrentColor = new Color(ballCurrentColor.r += 0.05f, ballCurrentColor.g, ballCurrentColor.b);
			ballRenderer.material.color = ballCurrentColor;
		} else if(ballCurrentColor != ballOriginalColor){
			ballCurrentColor = ballOriginalColor;
			ballRenderer.material.color = ballCurrentColor;
		}

		/*
		 * 	Rotation left/right
		 */
		if (currentMovement == MovementState.TurnLeft)
		{
			buttonLeftBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f); 
			playerPos.RotateAround (Ball.transform.position, Vector3.up, -1);
		} 
		else if (currentMovement == MovementState.TurnRight) 
		{
			playerPos.RotateAround (Ball.transform.position, Vector3.up, 1);
			buttonRightBtn.image.color = new Color (buttonsColor.r - 1f, buttonsColor.g, buttonsColor.b - 1f); 
		}
		else
		{
			buttonRightBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
			buttonLeftBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
		}
		
		if (currentAction != ActionState.Idle || ballRigidBody.velocity.magnitude > 0.5f) 
		{
			currentMovement = MovementState.None;
			hideGui = false;
			buttonLeft.SetActive(hideGui);
			buttonRight.SetActive(hideGui);
		} 
		else
		{
			hideGui = true;
			buttonLeft.SetActive(hideGui);
			buttonRight.SetActive(hideGui);
		}

	}

	// Update is called once per frame
	void FixedUpdate () {
		
		/*
		 * 	Moving to the ball
		 */
		if (currentMovement == MovementState.MoveToTheBall) {
			float step = moveVelocity * Time.deltaTime;
			Player.transform.position = Vector3.MoveTowards (Player.transform.position, Ball.transform.position + playerOffsetWithBall, step);
			if(transform.position == Ball.transform.position + playerOffsetWithBall)
				currentMovement = MovementState.MoveToTheBall;
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
