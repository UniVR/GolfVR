using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionScript : MonoBehaviour {

	public int LevelNumber;
	public bool AllowEnter;

	public static float GlobalLoadingSpeed = 0.4f;
	private static bool analyticsEnded = false;
	private static bool levelLoaded = false;

	private bool watched;
	private Image loading;

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
			loading.fillAmount += GlobalLoadingSpeed * Time.deltaTime; 
			if(loading.fillAmount>=0.5f && !analyticsEnded){
				analyticsEnded = true;
				AnalyticsMainMenu.End ();
			}
			if(loading.fillAmount>=0.7f && !levelLoaded){
				levelLoaded = true;
				Global.LoadHoleNumber = LevelNumber;
				Application.LoadLevelAsync("GolfVR");
			}
		} else if(loading && loading.fillAmount>0){
			loading.fillAmount -= GlobalLoadingSpeed; 
		}
	}
}
