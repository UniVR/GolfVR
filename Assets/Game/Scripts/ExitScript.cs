using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var text = GetComponent<Text> ();
		text.text = string.Format(text.text, MainScript.Get().Score);
	}

}
