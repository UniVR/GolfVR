using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// *** Event manager ***
/// The event manager used for the GUI to change the level etc...
/// </summary>
public class EventManager : MonoBehaviour {

	public void LoadFirstScene(){
		Application.LoadLevel("GolfVR");
	}

	public void ExitGame(){
		Application.Quit();
	}


}
