using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {

	public static int ObjectWatched;

	void Start () {		
		const string projectId = "e70cb1cc-6f2c-4c87-91ac-4e2ef11fc1cf";
		UnityAnalytics.StartSDK (projectId);

		ObjectWatched = 0;
	}	

	public static void End(){		
		UnityAnalytics.CustomEvent("MainMenu", new Dictionary<string, object>
		{
			{ "ObjectWatched", ObjectWatched },
			{ "TimeElapsed", Time.timeSinceLevelLoad }
		});
	}
}