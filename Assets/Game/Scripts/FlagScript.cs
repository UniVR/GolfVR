using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagScript : MonoBehaviour {

	public float distanceFactor;

	public float scaleFactor; 
	public float heightFactor; 

	private HoleScript hole;
	private BallScript ball;
	private PlayerScript player;
	private Text text;

	private Vector3 initialScale;
	private float initialHeight; 

	// Use this for initialization
	void Start () {
		hole = GetComponentInParent<HoleScript> ();
		ball = MainScript.Get ().Ball;
		player = MainScript.Get ().Player;
		text = GetComponentInChildren<Text> ();

		initialScale = transform.localScale; 
		initialHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		//Show the distance between the ball and the hole
		float distance = Vector3.Distance(hole.transform.position, ball.transform.position) * distanceFactor;
		text.text = distance.ToString("0") + " m";

		//Scale the GUI depending on the camera distance
		distance = Vector3.Distance(hole.transform.position, player.transform.position);
		var scale = distance * scaleFactor;
		transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
		transform.localScale = new Vector3 (initialScale.x + scale, initialScale.y + scale, initialScale.z + scale); 
		transform.position = new Vector3 (transform.position.x, initialHeight + distance * heightFactor, transform.position.z);			
	}

	public void SetActive(bool active){
		this.gameObject.SetActive(active);
	}
}
