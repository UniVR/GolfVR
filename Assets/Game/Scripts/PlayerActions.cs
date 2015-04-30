using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour {

	public GameObject ball;
	private Rigidbody ballRigidBody;

	public ClubScript clubScript;

	public GameObject buttonLeft;
	public GameObject buttonRight;

	private Button buttonLeftBtn;
	private Button buttonRightBtn;

	public enum ActionRotation{
		TurnLeft,
		TurnRight,
		None
	}

	private ActionRotation action;
	private Transform playerPos;
	private Color buttonsColor;
	private bool hideGui;


	private bool moveToTheBall;
	public float PlayerToBallSpeed;
	private Vector3 offset;
	

	// Use this for initialization
	void Start () {
		ballRigidBody = ball.GetComponent<Rigidbody> ();
		playerPos = GetComponent<Transform> ();
		action = ActionRotation.None;
		buttonLeftBtn = buttonLeft.GetComponent<Button> ();
		buttonRightBtn = buttonRight.GetComponent<Button> ();
		buttonsColor = buttonLeftBtn.image.color;
		hideGui = false;

		// Moving to the ball initialisation
		moveToTheBall = false;
		offset = transform.position - ball.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*
		 * 	Rotation left/right
		 */
		if(action==ActionRotation.TurnLeft)
			playerPos.RotateAround(ball.transform.position, Vector3.up, -1);
		else if(action==ActionRotation.TurnRight)
			playerPos.RotateAround(ball.transform.position, Vector3.up, 1);

		if (clubScript.currentState != ClubScript.State.Idle || ballRigidBody.velocity.magnitude > 0.5f) 
		{
			action = ActionRotation.None;
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

		/*
		 * 	Moving to the ball
		 */
		if (moveToTheBall) {
			float step = PlayerToBallSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, ball.transform.position + offset, step);
			if(transform.position == ball.transform.position + offset)
				moveToTheBall = false;
		}
	}
	
	/*
	 * 	Rotation actions
	 */
	public void TurnLeft(){
		action = ActionRotation.TurnLeft;
		buttonLeftBtn.image.color =  new Color(buttonsColor.r-1f, buttonsColor.g, buttonsColor.b-1f); 
	}

	public void TurnRight(){
		action = ActionRotation.TurnRight;
		buttonRightBtn.image.color =  new Color(buttonsColor.r-1f, buttonsColor.g, buttonsColor.b-1f); 
	}

	public void StopTurn(){
		action = ActionRotation.None;
		buttonRightBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
		buttonLeftBtn.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
	}

	/*
	 * 	Moving to the ball actions
	 */
	public void MovePlayerToBall(){
		moveToTheBall = true;
	}
}
