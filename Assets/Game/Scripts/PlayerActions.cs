using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {

	private Transform playerPos;

	// Use this for initialization
	void Start () {
		playerPos = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnLeft(){
		playerPos.Rotate (Vector3.up, 1);
	}

	public void TurnRight(){
		playerPos.Rotate (Vector3.up, -1);
	}
}
