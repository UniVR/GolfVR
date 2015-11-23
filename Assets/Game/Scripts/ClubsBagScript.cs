using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ClubsBagScript : MonoBehaviour {

	public MainScript MainScript;
	public GameObject WatchPlane;
	public GameObject Leave;

	public Canvas Menu;
	public GameObject DefaultButton;
	public List<GameObject> Clubs;

	[HideInInspector]
	public bool ButtonWatched;
	private bool BagWatched;

	private List<GameObject> buttons;
	private Vector2 canvasSize;
	private bool bagWatched;
	private bool canvasWatched;

	void Start(){
		buttons = new List<GameObject> ();
		ButtonWatched = false;

		var canvasRect = Menu.GetComponent<RectTransform>();
		var canvasSizeX = canvasRect.sizeDelta.x;
		canvasSize = canvasRect.sizeDelta;

		var buttonRect = DefaultButton.GetComponent<RectTransform>();
		var buttonSizeX = buttonRect.sizeDelta.x;

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
			buttonScript.Init(Menu, i);
			buttonScript.ClubScript = clubScript;
			buttonScript.BagScript = this;
			if(MainScript.GetCurrentClub().gameObject.name == clubScript.gameObject.name){
				buttonScript.Select(true);
			}

			buttons.Add(clubButton);
		}

		Menu.enabled = false;
		WatchPlane.AddComponent(typeof(EventTrigger));
		EventTrigger trigger = WatchPlane.GetComponent<EventTrigger>();
		//Pointer enter
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( (eventData) => { BagWatched = true; });
		trigger.triggers.Add(entry);
		
		//Pointer exit
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener( (eventData) => { BagWatched = false; } );
		trigger.triggers.Add(entry);

		Leave.AddComponent(typeof(EventTrigger));
		trigger = Leave.GetComponent<EventTrigger>();
		//Pointer enter
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( (eventData) => { Application.LoadLevel("MainMenu"); });
		trigger.triggers.Add(entry);
	}

	void FixedUpdate(){
		if (ButtonWatched || BagWatched) {
			Menu.enabled = true;
		} else {
			Menu.enabled = false;
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
}
