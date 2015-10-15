using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animation))]
public class AnimatorScript : MonoBehaviour {
	public GameObject WatchPanel;

	private Animation animation;
	private float currentTime;
	private bool watched;

	void Start () {
		animation = GetComponent<Animation> ();
		animation.wrapMode = WrapMode.Once;
		currentTime = 0f;

		var watchPanel = WatchPanel;
		watchPanel.AddComponent(typeof(EventTrigger));
		EventTrigger trigger = watchPanel.GetComponent<EventTrigger>();

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( (eventData) => 
       	{ 
			watched = true; 
			animation[animation.clip.name].speed = 1f;
			animation.Play();
		});
		trigger.triggers.Add(entry);

		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener( (eventData) => 
		{ 
			watched = false;
			animation[animation.clip.name].speed = -1f;
			animation[animation.clip.name].time = 1f;
			animation.Play();
		} );
		trigger.triggers.Add(entry);
	}


	// Update is called once per frame
	void Update () {
	}
}



