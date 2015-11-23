using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelectionScript : MonoBehaviour {

	public int LevelNumber;
	public bool Locked;

	public static float GlobalLoadingSpeed = 0.4f;
	private static bool analyticsEnded = false;
	private static bool levelLoaded = false;

	private bool watched;
	private Image loading;

	private LevelSelectionReferences refs;

	// Use this for initialization
	void Start () {
		analyticsEnded = false;
		levelLoaded = false;

		var cmt = transform.Find ("Loading");
		if(cmt)
			loading = cmt.GetComponent<Image>();

		var maxUnlockedLevel = Global.SavedData.UnlockedLevel;

		var medal = transform.Find ("Medal");
		var levelHidder = transform.Find ("Hidder");
		if (LevelNumber > maxUnlockedLevel) {
			Locked = true;
			if(levelHidder)
				levelHidder.gameObject.SetActive(true);
			if(medal)
				medal.gameObject.SetActive(false);
		} else {
			if(levelHidder)
				levelHidder.gameObject.SetActive(false);
			if(medal)
				medal.gameObject.SetActive(true);
		}

		if (Locked)
			return;

		if (medal) {
			medal.gameObject.SetActive (true);
			var medalImage = medal.GetComponent<Image>();
			var grade = Global.SavedData.LevelScores[LevelNumber];
			var medalsRef = this.GetComponentInParent<LevelSelectionReferences>();
			if(grade==Grade.None){
				medal.gameObject.SetActive(false);
			}
			else if(grade==Grade.OneStar){
				medalImage.sprite = medalsRef.BronzeMedal;
			}
			else if(grade==Grade.TwoStar){
				medalImage.sprite = medalsRef.SilverMedal;
			}
			else if(grade==Grade.ThreeStar){
				medalImage.sprite = medalsRef.GoldMedal;
			}
		}

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
			if(loading.fillAmount>=0.4f && !analyticsEnded){
				analyticsEnded = true;
				AnalyticsMainMenu.End ();
			}
			if(loading.fillAmount>=0.6f && !levelLoaded){
				levelLoaded = true;
				Global.LoadHoleNumber = LevelNumber;
				Application.LoadLevelAsync("GolfVR");
			}
		} else if(loading && loading.fillAmount>0){
			loading.fillAmount -= GlobalLoadingSpeed; 
		}
	}
}
