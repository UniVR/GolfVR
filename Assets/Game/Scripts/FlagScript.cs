using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagScript : MonoBehaviour {

	public float objectScale; 
	public float heightScale; 

	private HoleScript hole;
	private BallScript ball;
	private PlayerScript player;
	private Text text;

	private Vector3 initialScale;
	private float initialHeight; 

	// Use this for initialization
	void Start () {
		hole = MainScript.Get ().GetCurrentHole ();
		ball = MainScript.Get ().Ball;
		player = MainScript.Get ().Player;
		text = GetComponentInChildren<Text> ();

		initialScale = transform.localScale; 
		initialHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		//Show the distance between the ball and the hole
		float distance = Vector3.Distance(hole.transform.position, ball.transform.position);
		text.text = "Distance:\n" + distance.ToString("0.00") + "m";

		distance = Vector3.Distance(hole.transform.position, player.transform.position);
		transform.localScale = initialScale * distance * objectScale; 
		transform.position = new Vector3 (transform.position.x, initialHeight + distance * heightScale, transform.position.z);
	}
}
