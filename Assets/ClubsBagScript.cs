using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ClubsBagScript : MonoBehaviour {

	public Canvas Menu;
	public GameObject DefaultButton;
	public List<GameObject> Clubs;

	private List<GameObject> buttons;
	private bool bagWatched;
	private bool canvasWatched;

	void Start(){
		buttons = new List<GameObject> ();
		bagWatched = false;
		canvasWatched = false;

		var canvasRect = Menu.GetComponent<RectTransform>();
		var canvasSizeX = canvasRect.sizeDelta.x;
		var buttonSpace = canvasSizeX/Clubs.Count;

		var buttonRect = DefaultButton.GetComponent<RectTransform>();
		var buttonSizeX = buttonRect.sizeDelta.x;

		var begin = -canvasSizeX / 2 + buttonSizeX / 2 + buttonSizeX / 2;

		for(var i=0; i<Clubs.Count; i++) {
			var club = Clubs[i];

			//Instantiate the button and add it to the canvas at the good place
			GameObject clubButton = (GameObject)GameObject.Instantiate(DefaultButton);
			clubButton.transform.SetParent(Menu.transform, false);
			clubButton.transform.localPosition = new Vector3(begin + (i*buttonSpace), 0f, 0f);

			//Set the button image depending on the club
			var clubScript = club.GetComponent<ClubScript>();
			var clubButtonConverted = clubButton.GetComponent<Button>();
			clubButtonConverted.image.overrideSprite = clubScript.ClubImage;

			//Associate the clubScript to the button
			var buttonScript = clubButton.GetComponent<ClubSelectionButtonScript>();
			buttonScript.ClubScript = clubScript;


			//TODO

			//clubButton.onClick.AddListener(delegate() {/* StartGame("Level1"); */});
			buttons.Add(clubButton);
		}

		Menu.enabled = false;
	}

	void FixedUpdate(){
		if (bagWatched || canvasWatched) {
			Menu.enabled = true;
		} else {
			Menu.enabled = false;
		}			
	}

	public void WatchBag(){
		bagWatched = true;
	}

	public void UnWatchBag(){
		bagWatched = false;
	}

	public void WatchCanvas(){
		canvasWatched = true;
	}
	
	public void UnWatchCanvas(){
		canvasWatched = false;
	}
}
