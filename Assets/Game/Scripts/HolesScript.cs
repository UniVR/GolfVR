using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HolesScript : MonoBehaviour {

	public MainScript MainScript;
	
	/// <summary>
	/// The current played hole
	/// </summary>
	public HoleScript CurrentHole; 

	/// <summary>
	/// Just to keep the hole list at one place
	/// </summary>
	public List<HoleScript> Holes;

	public void EnterHole(HoleScript enteredHole){
		var index = Holes.IndexOf (CurrentHole);
		if (index + 1 < Holes.Count) {
			CurrentHole = Holes [index + 1];
			MainScript.EnterHole();
		} else {
			MainScript.Win();
		}
	}
}
