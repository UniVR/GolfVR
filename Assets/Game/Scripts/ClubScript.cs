using UnityEngine;
using System.Collections;

public class ClubScript : MonoBehaviour {
	/*
	 * 	Club properties
	 */
	public Sprite ClubImage;
		
	public float clubForceCoef;
	public float clubAngle;

	public float velocityLoading;
	public float velocityShooting;	
	private float minAngle;
	public float midAngle;
	public float maxAngle;

	[HideInInspector]
	public float LoadingTime;

	private Vector3 clubDefaultPosition;
	private Quaternion clubDefaultRotation;
	private bool reset = false;
	private int currentDirection;

	private AudioSource audioSource;

	void Start () {
		LoadingTime = 0f;
		clubDefaultPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		clubDefaultRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
		audioSource = GetComponent<AudioSource> ();
		currentDirection = 1;
	}

	void Update () {
		if (reset && transform.localRotation.z < midAngle) {
			transform.localRotation = Quaternion.RotateTowards (transform.localRotation, clubDefaultRotation, 5f);
		} else if (reset) {
			reset = false;
		}
	}
	/*
	 * 	LOAD
	 */
	public void Load(){
		LoadingTime += Time.deltaTime * currentDirection;
		transform.Rotate (Vector3.down * Time.deltaTime * velocityLoading * currentDirection);
		minAngle = -transform.localRotation.z;
		if (IsMaxLoad ()) {
			currentDirection = -1;
		} else if (IsMinLoad ()) {
			currentDirection = 1;
		}
	}

	public bool IsMaxLoad(){
		return transform.localRotation.z > maxAngle;
	}

	public bool IsMinLoad(){
		return transform.localRotation.z <= midAngle;
	}
	
	public float LoadingAmount(){			//Progression of the loading
		return Mathf.InverseLerp (midAngle, maxAngle, transform.localRotation.z); 
	}

	/*
	 * 	FIRE
	 */
	public void Fire(){
		transform.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * LoadingTime);
		audioSource.Play ();
	}

	public bool IsFired(){
		return transform.localRotation.z <= minAngle;
	}
	
	public bool HasShooted(){
		return transform.localRotation.z <= midAngle;
	}

	public void Reset(){
		LoadingTime = 0;
		reset = true;
	}


	public GameObject InstantiateNewClub(GameObject newClubPrefb){
		return (GameObject)GameObject.Instantiate(newClubPrefb, clubDefaultPosition, clubDefaultRotation);
	}

	public string GetName(){
		return this.name;
	}
}
