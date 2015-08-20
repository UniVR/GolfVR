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
	public float minAngle;
	public float midAngle;
	public float maxAngle;

	[HideInInspector]
	public float LoadingTime;

	private Quaternion clubDefaultRotation;
	private bool reset = false;

	void Start () {
		clubDefaultRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
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
		LoadingTime += Time.deltaTime;
		transform.Rotate (Vector3.down * Time.deltaTime * velocityLoading);
	}

	public bool IsLoaded(){
		return transform.localRotation.z > maxAngle;
	}
	
	public float LoadingAmount(){			//Progression of the loading
		return Mathf.InverseLerp (midAngle, maxAngle, transform.localRotation.z); 
	}

	/*
	 * 	FIRE
	 */
	public void Fire(){
		transform.Rotate (-Vector3.down * Time.deltaTime * velocityShooting * LoadingTime);
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
}
