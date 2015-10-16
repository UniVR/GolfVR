using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatorScript : MonoBehaviour {
	public GameObject WatchPanel;

	private Animator animator;
	private bool watched;

	void Start () {
		animator = GetComponent<Animator> ();

		//Add the event trigger to the panel
		var watchPanel = WatchPanel;
		watchPanel.AddComponent(typeof(EventTrigger));
		EventTrigger trigger = watchPanel.GetComponent<EventTrigger>();

		//Pointer enter
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener( (eventData) => 
       	{ 
			watched = true; 
			animator.SetBool("Watched", true);
		});
		trigger.triggers.Add(entry);

		//Pointer exit
		entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerExit;
		entry.callback.AddListener( (eventData) => 
		{ 
			watched = false;
			animator.SetBool("Watched", false);

		} );
		trigger.triggers.Add(entry);
	}


	// Update is called once per frame
	void Update () {
	}
}



