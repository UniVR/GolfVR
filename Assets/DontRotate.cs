using UnityEngine;
using System.Collections;

public class DontRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void LateUpdate() {
		if (transform.rotation != Quaternion.Euler(0, 0, 0)) {
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
	}
}
