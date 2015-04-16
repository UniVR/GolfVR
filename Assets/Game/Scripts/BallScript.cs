using UnityEngine;
using System.Collections;

/// <summary>
/// Ball script. This script does many things:
/// 
/// *** Collision detection ***
/// Detect when the ball is hitted. 
/// If the collider contains a script ClubProperties it use theses
/// properties to control how the ball will react to the collision
/// 
/// *** Particle emmission ***
/// ...
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BallScript : MonoBehaviour {

	Rigidbody ball;
	ParticleSystem particle;

	void Start () {
		ball = GetComponent<Rigidbody> ();
		particle = GetComponent<ParticleSystem> ();
	}

	void Update(){
		float magnitude = ball.velocity.magnitude;
		particle.startSize = magnitude/20;
	}

	void OnCollisionEnter (Collision col)
	{
		ClubProperties prop = col.gameObject.GetComponent<ClubProperties> ();
		if (prop == null)
			return;
		Vector3 normal = col.contacts[0].normal;
		float magnitude = col.relativeVelocity.magnitude;
		ball.AddForce (prop.forceCoef *  normal, ForceMode.Impulse);

		particle.Play();
	}
}
