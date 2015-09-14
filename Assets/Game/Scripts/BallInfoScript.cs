using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BallInfoScript : MonoBehaviour {

	public float distanceFactor;

	public float scaleFactor; 
	public float heightFactor; 

	public float InformationShowTime;
	private float InformationTimeShowed;

	private Canvas canvas;
	private PlayerScript player;
	private Vector3 initialScale;
	private float initialHeight; 

	private Text text;

	// Use this for initialization
	void Start () {
		player = MainScript.Get ().Player;
		canvas = GetComponent<Canvas>();
		text = GetComponentInChildren<Text> ();

		initialScale = transform.localScale; 
		initialHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (InformationTimeShowed > 0) {
			InformationTimeShowed -= Time.deltaTime;

			//Scale the GUI depending on the camera distance
			var distance = Vector3.Distance(transform.position, player.transform.position);
			var scale = distance * scaleFactor;
			transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
			transform.localScale = new Vector3 (initialScale.x + scale, initialScale.y + scale, initialScale.z + scale); 
			transform.position = new Vector3 (transform.position.x, initialHeight + distance * heightFactor, transform.position.z);
		} else {
			canvas.enabled = false;
		}
	}

	public void Show(Vector3 position){
		transform.position = position;
	}

	public void ShowInformation(Vector3 position, string informations){
		if (InformationTimeShowed <= 0) {
			Show (position);
			text.text = informations;
			canvas.enabled = true;
			InformationTimeShowed = InformationShowTime;
		}
	}
}
