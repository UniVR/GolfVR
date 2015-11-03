using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionScript : MonoBehaviour {

	public int LevelNumber;
	public bool AllowEnter;

	private bool watched;
	private Image loading;
	private float loadingSpeed = 0.005f;

	// Use this for initialization
	void Start () {

		var cmt = transform.Find ("Loading");
		if(cmt)
			loading = cmt.GetComponent<Image>();

		var levelHidder = transform.Find ("Hidder");
		var maxUnlockedLevel = Global.SavedData.UnlockedLevel;
		if (LevelNumber > maxUnlockedLevel) {
			AllowEnter = false;
			if(levelHidder)
				levelHidder.gameObject.SetActive(true);
		} else {
			if(levelHidder)
				levelHidder.gameObject.SetActive(false);
		}

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
		if (watched && loading) {
			loading.fillAmount += loadingSpeed; 
		} else if(loading && loading.fillAmount>0){
			loading.fillAmount -= loadingSpeed; 
		}
	}
}
