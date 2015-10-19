using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {

	public static int ObjectWatched;

	void Start () {		
		ObjectWatched = 0;
	}	

	public static void End(){		
		Analytics.CustomEvent("MainMenu", new Dictionary<string, object>
		{
			{ "ObjectWatched", ObjectWatched },
			{ "TimeElapsed", Time.timeSinceLevelLoad }
		});
	}
}