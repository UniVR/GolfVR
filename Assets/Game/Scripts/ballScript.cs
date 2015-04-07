using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject club;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform transform = club.GetComponent<Transform> ();
		transform.Rotate(0, 0, Time.deltaTime*10f, Space.World);
	}

	public void OnPointerEnter(){
		Transform transform = club.GetComponent<Transform> ();
		Quaternion q = transform.localRotation;
		transform.Rotate(q.x, q.y, q.z+100f);
	}

	public void OnPointerExit(){
		
	}
}
