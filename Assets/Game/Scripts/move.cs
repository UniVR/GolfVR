using UnityEngine;
using System.Collections;


/// <summary>
/// *** Move ***
/// Unused: deplace an object depending on the mouse moves
/// </summary>
public class move : MonoBehaviour {

	GameObject ball;

	public float horizontalSpeed;
	public float verticalSpeed;

	private float h1;
	private float v1;

	void Start () {
		h1 = horizontalSpeed * Input.GetAxis ("Mouse X");
		v1 = verticalSpeed * Input.GetAxis ("Mouse Y");
	}
	
	void FixedUpdate () { // Get the mouse delta. This is not in the range -1...1
		
		var h = horizontalSpeed * Input.GetAxis ("Mouse X");
		var v = verticalSpeed * Input.GetAxis ("Mouse Y");

		//transform.Translate (h, 0, v);
		var rigidBody = GetComponent<Rigidbody> ();
		rigidBody.AddForce(h-h1, 0, v1-v);

		//var transform = GetComponent<Transform> ();
		//transform.Translate(h, 0, v1);

		h1 = h;
		v1 = v;
	}
}
