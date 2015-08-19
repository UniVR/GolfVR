using UnityEngine;
using System.Collections.Generic;

public class BallScript : MonoBehaviour {

	private MainScript mainScript;
	private DetectTerrainType detectTerrainType;

	private Vector3 oldPos;
	private Rigidbody rigidBody;
	private TrailRenderer trail;
	private float oldTrailTime;
	private AudioSource audioSource;
	private bool ballIsOnGround;
	private bool isShooted;

	void Start(){
		mainScript = MainScript.Get ();
		detectTerrainType = GetComponent<DetectTerrainType> ();
		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		trail = GetComponent<TrailRenderer> ();
		oldTrailTime = trail.time;
		isShooted = false;
		ballIsOnGround = true;
	}

	void FixedUpdate () {
		if (ballIsOnGround && isShooted) {
			detectTerrainType.SetBallDrag(mainScript.CurrentTerrain, transform.position, rigidBody);
		}
	}

	public void Shoot (float magnitude, float angle, float orientation)
	{
		isShooted = true;
		oldPos = transform.position;

		Vector3 direction = Quaternion.Euler(0, orientation, -angle) * new Vector3 (-1, 0, 0);
		rigidBody.AddForce(direction * magnitude, ForceMode.Impulse);

		audioSource.Play();
	}

	public void Stop(){
		isShooted = false;
		rigidBody.velocity = new Vector3(0f, 0f, 0f);
		rigidBody.angularDrag = 20f;		
		trail.enabled = false;
		trail.time = 0;
		ballIsOnGround = true;
	}

	public void StopAndMove(Vector3 position){
		transform.position = position;
		Stop ();
	}

	public void StopAndGetBackToOldPos(){
		transform.position = oldPos;
		Stop ();
	}

	public bool IsStopped(){
		return rigidBody.velocity.magnitude < 0.1f;
	}

	public bool IsShooted(){
		return isShooted;
	}

	public bool IsOnGround(){
		return ballIsOnGround;
	}

	public bool IsOutOfBound(){
		return detectTerrainType.IsOutOfBound (mainScript.CurrentTerrain, transform.position);
	}

	void OnCollisionEnter (Collision col)
	{
		ballIsOnGround = true;
	}

	void OnCollisionExit (Collision col)
	{
		ballIsOnGround = false;
	}
}
