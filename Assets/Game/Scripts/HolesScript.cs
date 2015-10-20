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

	public HoleScript GetNext(){
		var index = Holes.IndexOf (CurrentHole);
		if (index + 1 < Holes.Count) {
			return Holes [index + 1];
		}
		return null;
	}

	public void EnterHole(HoleScript enteredHole){
		CurrentHole.Enable(false);
		CurrentHole = GetNext ();
		if (CurrentHole != null) {
			CurrentHole.Enable(true);
			MainScript.EnterHole();
		} else {
			MainScript.Win();
		}
	}
}
