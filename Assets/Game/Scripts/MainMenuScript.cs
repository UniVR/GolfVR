using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// *** Event manager ***
/// The event manager used for the GUI to change the level etc...
/// </summary>
public class MainMenuScript : MonoBehaviour {

	AsyncOperation loadLevel;

	void Start () {
		loadLevel = Application.LoadLevelAsync ("GolfVR");
		loadLevel.allowSceneActivation = false;
	}
	public void LoadFirstScene(){
		//loadLevel = Application.LoadLevel("GolfVR");
		loadLevel.allowSceneActivation = true;
	}

	public void ExitGame(){
		Application.Quit();
	}


}
