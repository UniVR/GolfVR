using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HolesScript : MonoBehaviour {

	public MainScript MainScript;
	[HideInInspector]
	public HoleScript PreviousHole; 
	public HoleScript CurrentHole; 
	public List<HoleScript> Holes;

	public HoleScript GetNext(){
		var index = Holes.IndexOf (CurrentHole);
		if (index + 1 < Holes.Count) {
			return Holes [index + 1];
		}
		return null;
	}

	public void EnterHole(HoleScript enteredHole){
		PreviousHole = CurrentHole;
		CurrentHole.Enable(false);
		CurrentHole = GetNext ();
		if (CurrentHole != null) {
			CurrentHole.Enable(true);
			MainScript.EnterHole();
		} else {
			MainScript.Win();
		}
	}

	public void SetHole(int holeNumber){
		CurrentHole = Holes[holeNumber];
	}
}
