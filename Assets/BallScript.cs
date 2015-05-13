using UnityEngine;
using System.Collections.Generic;

public class BallScript : MonoBehaviour {

	public MainScript mainScript;

	void OnCollisionEnter (Collision col)
	{
		mainScript.CollisionEnter(gameObject, col);
	}

	void OnCollisionExit (Collision col)
	{
		mainScript.CollisionExit(gameObject, col);
	}
}
