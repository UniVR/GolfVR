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

	public void Shoot (float magnitude, float orientation, ClubProperties prop)
	{
		Vector3 direction = Quaternion.Euler(0, orientation, -prop.angle) * new Vector3 (-1, 0, 0);
		ball.AddForce(direction * magnitude * prop.forceCoef, ForceMode.Impulse);

		particle.Play();
	}
}
