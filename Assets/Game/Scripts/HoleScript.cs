using UnityEngine;
using System.Collections;

public class HoleScript : MonoBehaviour {

	[HideInInspector]
	public HolesListScript HolesListScript;

	public Terrain Terrain;
	public int HoleNumber;

	public GameObject BeginPosition;

	// Use this for initialization
	void Start () {
		HolesListScript = GetComponentInParent<HolesListScript> ();
	}
	
	public void EnterHole(){
		HolesListScript.EnterHole(this);
	}
}
