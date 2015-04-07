using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour {

	private bool isViewed = false;
	private float time = 0f;
	private Button currentButton = null;



	public void LoadFirstScene(){
		Application.LoadLevel("GolfVR");
	}

	public void ExitGame(){
		Application.Quit();
	}


}
