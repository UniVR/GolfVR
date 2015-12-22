using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var text = GetComponent<Text> ();
		var main = MainScript.Get ();
		if(main!=null)
			text.text = string.Format(text.text, main.TotalScore);
	}


	public void ExitGame(){
		Application.Quit ();
	}

	public void Replay(){
		Application.LoadLevel ("MainMenu");
	}
}
