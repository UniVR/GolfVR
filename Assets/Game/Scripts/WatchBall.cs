using UnityEngine;
using System.Collections;

public class WatchBall : MonoBehaviour {

	public GameObject ball;

	private bool isWatched;
	private MeshRenderer renderer;
	private Color originalColor;
	private Color currentColor;

	// Use this for initialization
	void Start () {
		renderer = ball.GetComponent<MeshRenderer> ();
		originalColor = currentColor = renderer.material.color;
		isWatched = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isWatched) {
			currentColor = new Color(currentColor.r += 0.05f, currentColor.g, currentColor.b);
			renderer.material.color = currentColor;
		} else if(currentColor != originalColor){
			currentColor = originalColor;
			renderer.material.color = currentColor;
		}
	}

	public void IsWatched(){
		isWatched = true;
	}

	public void IsUnWatched(){
		isWatched = false;
	}
}
