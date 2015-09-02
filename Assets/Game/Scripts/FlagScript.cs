using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlagScript : MonoBehaviour {

	private BallScript ball;
	private Text text;

	// Use this for initialization
	void Start () {
		ball = MainScript.Get ().Ball;
		text = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(text==null)
			text = GetComponentInChildren<Text> ();

		float distance = Vector3.Distance(transform.position, ball.transform.position);
		distance = 3.00f;
		distance += 0.001f;
		text.text = "Distance:\n" + distance.ToString("0.00") + "m";
	}
}
