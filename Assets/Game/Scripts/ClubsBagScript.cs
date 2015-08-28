using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ClubsBagScript : MonoBehaviour {

	public MainScript MainScript;

	public Canvas Menu;
	public GameObject DefaultButton;
	public List<GameObject> Clubs;
	
	private List<GameObject> buttons;
	private Vector2 canvasSize;
	private bool bagWatched;
	private bool canvasWatched;

	void Start(){
		buttons = new List<GameObject> ();
		bagWatched = false;
		canvasWatched = false;

		var canvasRect = Menu.GetComponent<RectTransform>();
		var canvasSizeX = canvasRect.sizeDelta.x;
		canvasSize = canvasRect.sizeDelta;

		var buttonRect = DefaultButton.GetComponent<RectTransform>();
		var buttonSizeX = buttonRect.sizeDelta.x;

		var begin = -canvasSizeX / 3 + buttonSizeX;
		var buttonSpace = (2*canvasSizeX/3)/Clubs.Count;

		for(var i=0; i<Clubs.Count; i++) {
			var club = Clubs[i];

			//Instantiate the button and add it to the canvas at the good place
			GameObject clubButton = (GameObject)GameObject.Instantiate(DefaultButton);
			clubButton.transform.SetParent(Menu.transform, false);

			//Set the button image depending on the club
			var clubScript = club.GetComponent<ClubScript>();
			var clubButtonConverted = clubButton.GetComponent<Button>();
			clubButtonConverted.image.overrideSprite = clubScript.ClubImage;

			//Associate the clubScript to the button
			var buttonScript = clubButton.GetComponent<ClubSelectionButtonScript>();
			buttonScript.Init(Menu, begin + (i*buttonSpace));
			buttonScript.ClubScript = clubScript;
			buttonScript.BagScript = this;
			if(MainScript.GetCurrentClub().gameObject.name == clubScript.gameObject.name){
				buttonScript.Select(true);
			}

			buttons.Add(clubButton);
		}
	}

	public void MoveToTheBall(Vector3 ball, Vector3 direction){
		transform.position = ball;
		transform.LookAt(direction);
	}

	public void DeactiveAllButton(){
		foreach (GameObject button in buttons) {
			var btn = button.GetComponent<ClubSelectionButtonScript>();
			btn.Select(false);
		}
	}

	public void SelectClub(ClubScript script){
		MainScript.SetCurrentClub (script);
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
