using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsMainMenu : MonoBehaviour {
	
	private static Dictionary<string, float> objectTimeWatched;
	private static float timeWatched;
	private static string currentObjectName;

	void Start(){
		objectTimeWatched = new Dictionary<string, float> ();
	}

	public static void End(){	
		if (currentObjectName != null)
			UnWatchObject (currentObjectName);

		var statDico = new Dictionary<string, object> ();
		foreach(var item in objectTimeWatched) {
			statDico.Add(item.Key, item.Value);
		}
		Analytics.CustomEvent("Main: time object are watched", statDico);
	}

	public static void WatchObject(string objectName){
		if (!objectTimeWatched.ContainsKey (objectName))
			objectTimeWatched.Add(objectName, 0);
		timeWatched = Time.timeSinceLevelLoad;
		currentObjectName = objectName;
	}

	public static void UnWatchObject(string objectName){
		objectTimeWatched[objectName] += Time.timeSinceLevelLoad - timeWatched;
		currentObjectName = null;
	}
}