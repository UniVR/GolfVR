using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public void LoadFirstScene(){
		Application.LoadLevel("GolfVR");
	}

	public void ExitGame(){
		Application.Quit();
	}
}
