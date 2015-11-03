using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectionScript : MonoBehaviour {

	public int LevelNumber;
	public bool AllowEnter;

	private bool watched;

	// Use this for initialization
	void Start () {

		var maxLevel = Global.SavedData.UnlockedLevel;
		if (LevelNumber > maxLevel)
			AllowEnter = false;

		if (!AllowEnter)
			return;

		gameObject.AddComponent(typeof(EventTrigger));
		EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
		
		//Pointer enter
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( (eventData) => { watched = true; });
		trigger.triggers.Add(entry);
		
		//Pointer exit
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener( (eventData) => { watched = false; } );
		trigger.triggers.Add(entry);
	}
	
	// Update is called once per frame
	void Update () {
		if (watched)
			Debug.Log ("ok");
	}
}
