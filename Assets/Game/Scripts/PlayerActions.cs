using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour {

	public GameObject rotateAround;

	public Button buttonLeft;
	public Button buttonRight;

	public enum ActionRotation{
		TurnLeft,
		TurnRight,
		None
	}

	private ActionRotation action;
	private Transform playerPos;
	private Color buttonsColor;

	// Use this for initialization
	void Start () {
		playerPos = GetComponent<Transform> ();
		action = ActionRotation.None;
		buttonsColor = buttonLeft.image.color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(action==ActionRotation.TurnLeft)
			playerPos.RotateAround(rotateAround.transform.position, Vector3.up, -1);
		else if(action==ActionRotation.TurnRight)
			playerPos.RotateAround(rotateAround.transform.position, Vector3.up, 1);
	}

	public void TurnLeft(){
		action = ActionRotation.TurnLeft;
		buttonLeft.image.color =  new Color(buttonsColor.r-1f, buttonsColor.g, buttonsColor.b-1f); 
	}

	public void TurnRight(){
		action = ActionRotation.TurnRight;
		buttonRight.image.color =  new Color(buttonsColor.r-1f, buttonsColor.g, buttonsColor.b-1f); 
	}

	public void StopTurn(){
		action = ActionRotation.None;
		buttonRight.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
		buttonLeft.image.color =  new Color(buttonsColor.r, buttonsColor.g, buttonsColor.b); 
	}
}
