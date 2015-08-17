using UnityEngine;
using System.Collections;

public class EnterHoleTrigger : MonoBehaviour {
	
	private HoleScript holeScript;

	void Start()
	{
		holeScript = GetComponentInParent<HoleScript> ();
	}

	void OnTriggerEnter (Collider collider)
	{
		if(collider.gameObject==holeScript.HolesListScript.MainScript.Ball)
			holeScript.EnterHole();
	}

}
