using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsGame : MonoBehaviour {
		
	private static string holeName;
	private static Dictionary<string, int> clubUsage;
	private static string currentClubName;
	private static float beginTime;
	private static int nbShoot;
	private static int outOfBound;

	// Use this for initialization
	void Start () {
	}

	public static void BeginHole(string name){
		holeName = name;
		clubUsage = new Dictionary<string, int> ();
		beginTime =  Time.timeSinceLevelLoad;
		nbShoot = 0;
		outOfBound = 0;
	}

	public static void EndHole(){
		var statDico = new Dictionary<string, object> ();
		statDico.Add("TotalShoot", nbShoot);
		statDico.Add("OutOfBound", outOfBound);
		statDico.Add("TimeElapsed", Time.timeSinceLevelLoad - beginTime);
		foreach(var item in clubUsage) {
			statDico.Add(item.Key, item.Value);
		}
		Analytics.CustomEvent(holeName, statDico);
	}

	public static void ChangeClub(string clubName){
		currentClubName = clubName;
	}

	public static void Shoot(){
		if (currentClubName!=null && !clubUsage.ContainsKey (currentClubName))
			clubUsage.Add (currentClubName, 1);
		else
			clubUsage [currentClubName]++;

		nbShoot++;
	}

	public static void OutOfBound(){
		outOfBound++;
	}

	public static void Won(int score){
		Analytics.CustomEvent("WonTheGame", new Dictionary<string, object>
          {
			{ "TotalScore", score },
			{ "TotalTime", Time.timeSinceLevelLoad }
		});
	}
}
