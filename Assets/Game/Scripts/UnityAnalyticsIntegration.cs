using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	void Start () {		
		const string projectId = "e70cb1cc-6f2c-4c87-91ac-4e2ef11fc1cf";
		UnityAnalytics.StartSDK (projectId);		
	}	
}