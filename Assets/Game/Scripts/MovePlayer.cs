using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {
	
	public float moveSpeed;
	
	private GameObject destination;
	private bool isMoving;


	// Use this for initialization
	void Start () {
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			float step = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, destination.transform.position, step);
		}
	}

	public void MovePlayerTo(GameObject ball){
		destination = ball;
		isMoving = true;
	}

}
