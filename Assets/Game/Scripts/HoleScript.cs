using UnityEngine;
using System.Collections;

public class HoleScript : MonoBehaviour {

	public MainScript MainScript;
	public int HoleNumber;

	public GameObject BeginPosition;

	// Use this for initialization
	void Start () {

	}
	
	public void EnterHole(){
		MainScript.EnterHole (HoleNumber);
	}
}
