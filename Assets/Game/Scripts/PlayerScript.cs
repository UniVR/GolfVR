using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject Club;

	public float moveForwardSpeed;
	public float maxDistanceWithBall;

	private GameObject CurrentClub;
	private Vector3 initialPosition;

	public void Start(){
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z - maxDistanceWithBall);
		initialPosition = this.transform.localPosition;
	}

	public void Reset(){
		this.transform.localPosition = initialPosition;
	}

	public void MoveForward ()
	{
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + moveForwardSpeed);
	}

	public void MoveBackward ()
	{
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z - moveForwardSpeed);
	}

	public bool isFarFromBall(){
		return this.transform.localPosition.z <= initialPosition.z;
	}

	public bool isOnTheBall(){
		return this.transform.localPosition.z >= initialPosition.z + maxDistanceWithBall;
	}

	public void SetCurrentClub(GameObject newClub){

		foreach (Transform child in Club.transform) {
			GameObject.Destroy(child.gameObject);
		}

		newClub.transform.SetParent(Club.transform, false);
		CurrentClub = newClub;
	}

	public GameObject GetCurrentClub(){
		if (CurrentClub == null) {
			CurrentClub = Club.transform.GetChild(0).gameObject;
		}
		return CurrentClub;
	}
}
