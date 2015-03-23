using UnityEngine;
using System.Collections;

public class ballScript : MonoBehaviour {

	public Collider club;
	public AudioSource audio;

	public float yVelocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnCollisionEnter(Collision collision) {
		if (collision.collider != club)
			return;

		float velocity = yVelocity * collision.relativeVelocity.z;
		var ball = GetComponent<Rigidbody> ();
		ball.AddForce (0, velocity, 0);

		// Play a sound if the coliding objects had a big impact.		
		if (collision.relativeVelocity.magnitude > 2)
			audio.Play();
	}
}
