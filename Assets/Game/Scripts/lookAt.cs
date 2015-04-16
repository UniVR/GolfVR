using UnityEngine;
using System.Collections;

/// <summary>
/// *** Look at ***
/// Unused. Look at a target...
/// </summary>
public class lookAt : MonoBehaviour {

	public Transform target;
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target);
	}
}
