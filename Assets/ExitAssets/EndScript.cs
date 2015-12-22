using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour {

	// Update is called once per frame
	void shareScore () {
		//
	}

	void likeUs () {
		//Application.OpenURL ("https://www.facebook.com/UniVR.GolfVR");
	}

	void followUs () {
		//Application.OpenURL ("https://twitter.com/UniGolfVR");
	}

	void restart () {
		//Application.LoadLevel ("MainMenu");
	}

	void quit () {
		//Application.Quit (); //Do not use Application.Quit for iOs (I don't know why but this is what Unity says)
	}
}
