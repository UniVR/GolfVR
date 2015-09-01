using UnityEngine;
using System.Collections.Generic;

public class BallScript : MonoBehaviour {

	public float TimeBeforeSlowing;
	public float AddedDragAfterTime;

	public float TimeMagnitudeCoef;
	public float WidthMagnitudeCoef;

	private MainScript mainScript;
	private DetectTerrainType detectTerrainType;

	private Vector3 oldPos;
	private Vector3 newPos;
	private Rigidbody rigidBody;
	private TrailRenderer trail;
	private float oldTrailTime;
	private AudioSource audioSource;

	private float timeFromShoot;
	private float addedDrag;
	private bool isOnGround;
	private bool isShooted;
	private bool isLocked;

	void Start(){
		mainScript = MainScript.Get ();
		detectTerrainType = GetComponent<DetectTerrainType> ();
		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		trail = GetComponent<TrailRenderer> ();
		oldTrailTime = trail.time;
		isShooted = false;
		isOnGround = true;
		isLocked = true;
	}

	void FixedUpdate () {
		if (!isLocked && isOnGround && isShooted) {
			timeFromShoot += Time.deltaTime;
			if(timeFromShoot>TimeBeforeSlowing){
				addedDrag += AddedDragAfterTime;
			}
			detectTerrainType.SetBallDrag(mainScript.CurrentTerrain, transform.position, rigidBody, addedDrag);
		}

		if (!isOnGround) {
			rigidBody.AddForce(mainScript.Wind.GetForce());
		}
	}

	public void Shoot (float magnitude, float angle, float orientation)
	{
		timeFromShoot = 0f;
		addedDrag = 0f;
		isShooted = true;
		isLocked = false;
		oldPos = transform.position;

		//trail.time = oldTrailTime;
		trail.time = magnitude * TimeMagnitudeCoef;
		trail.startWidth = magnitude * WidthMagnitudeCoef;

		detectTerrainType.SetBallDrag(mainScript.CurrentTerrain, transform.position, rigidBody);

		Vector3 direction = Quaternion.Euler(0, orientation, -angle) * new Vector3 (-1, 0, 0);
		rigidBody.AddForce(direction * magnitude, ForceMode.Impulse);

		audioSource.Play();
	}

	public void Stop(){
		isShooted = false;
		isOnGround = true;
		isLocked = true;

		rigidBody.velocity = new Vector3(0f, 0f, 0f);
		rigidBody.drag = 100f;	
		rigidBody.angularDrag = 100f;		

		trail.time = 0;
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
		var stopped = rigidBody.velocity.magnitude < 0.1f;
		if (stopped) {
			Stop();
		}
		return stopped;
	}

	public bool IsShooted(){
		return isShooted;
	}

	public bool IsOnGround(){
		return isOnGround;
	}

	public bool IsOutOfBound(){
		if (transform.position.y <= 0)
			return true;

		if(!isLocked && isOnGround && isShooted)
			return detectTerrainType.IsOutOfBound (mainScript.CurrentTerrain, transform.position);
		return false;
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.collider.GetType() == typeof(UnityEngine.TerrainCollider))
			isOnGround = true;
	}

	void OnCollisionExit (Collision col)
	{
		if(col.collider.GetType() == typeof(UnityEngine.TerrainCollider))
			isOnGround = false;
	}
}
