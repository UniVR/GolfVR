using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoleScript : MonoBehaviour {

	[HideInInspector]
	public HolesScript HolesListScript;

	public Terrain Terrain;
	public int HoleNumber;

	public GameObject BeginPosition;

	public float WindMin;
	public float WindMax;
	private bool WindIsInit = false;

	// Use this for initialization
	void Start () {
		HolesListScript = GetComponentInParent<HolesScript> ();
	}

	public float GetWindPower(){
		if (!WindIsInit) {
			var mainScript = MainScript.Get ();
			var difficutlyLength = System.Enum.GetNames(typeof(Difficulty)).Length;
			var range = (float)((WindMax - WindMin) / difficutlyLength);
			
			WindMin = (float)((int)mainScript.Difficulty) * range;
			WindMax = WindMin + range;
		}
		return Random.Range (WindMin, WindMax);
	}


	public void EnterHole(){
		HolesListScript.EnterHole(this);
	}
}
