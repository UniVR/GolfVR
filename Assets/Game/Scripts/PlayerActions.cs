using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour {

	public GameObject ball;
	private Rigidbody ballRigidBody;

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

	// Use this for initialization
	void Start () {
		ballRigidBody = ball.GetComponent<Rigidbody> ();
		playerPos = GetComponent<Transform> ();
		action = ActionRotation.None;
		buttonLeftBtn = buttonLeft.GetComponent<Button> ();
		buttonRightBtn = buttonRight.GetComponent<Button> ();
		buttonsColor = buttonLeftBtn.image.color;
		hideGui = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(action==ActionRotation.TurnLeft)
			playerPos.RotateAround(ball.transform.position, Vector3.up, -1);
		else if(action==ActionRotation.TurnRight)
			playerPos.RotateAround(ball.transform.position, Vector3.up, 1);

		if (ballRigidBody.velocity.magnitude < 1f) {
			hideGui = true;
			buttonLeft.SetActive(hideGui);
			buttonRight.SetActive(hideGui);
		} else {
			hideGui = false;
			buttonLeft.SetActive(hideGui);
			buttonRight.SetActive(hideGui);
		}
	}
	

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
}
