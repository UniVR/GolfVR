using UnityEngine;
using System.Collections;

public class RotationClub : MonoBehaviour {

	public float rotationSpeed;

	private Transform ClubRotationPoint;

	// Use this for initialization
	void Start () {
		ClubRotationPoint = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		ClubRotationPoint.Rotate (new Vector3 (0, 0, rotationSpeed));
	}
}
