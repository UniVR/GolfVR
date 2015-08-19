using UnityEngine;
using System.Collections;

public class HoleScript : MonoBehaviour {

	[HideInInspector]
	public HolesScript HolesListScript;

	public Terrain Terrain;
	public int HoleNumber;

	public GameObject BeginPosition;

	// Use this for initialization
	void Start () {
		HolesListScript = GetComponentInParent<HolesScript> ();
	}
	
	public void EnterHole(){
		HolesListScript.EnterHole(this);
	}
}
