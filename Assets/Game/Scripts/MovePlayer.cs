using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {
	
	public float moveSpeed;
	public GameObject ballDestination;
	public Vector3 offset;
	private bool isMoving;


	// Use this for initialization
	void Start () {
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			float step = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, ballDestination.transform.position + offset, step);
			if(transform.position == ballDestination.transform.position + offset)
				isMoving = false;
		}
	}

	public void MovePlayerToBall(){
		isMoving = true;
	}

	public void StopPlayer(){
		isMoving = false;
	}

}
