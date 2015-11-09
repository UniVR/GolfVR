using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Grade {
	None,
	OneStar,
	TwoStar,
	ThreeStar
}

[Serializable]
public class SavedInformations{

	private int unlockedLevel;
	public int UnlockedLevel {
		set{
			if(value>unlockedLevel)
				unlockedLevel = value;
		}
		get{ return unlockedLevel;}
	}
	public Grade[] LevelScores;

	public SavedInformations(){
		unlockedLevel = 0;
		LevelScores = new Grade[18];
		for (var i=0; i<LevelScores.Length; i++) {
			LevelScores[i] = Grade.None;
		}
	}

	public Grade SetScore(int holeNumber, int parScore, int playerScore){
		Grade grade = Grade.None;
		if (playerScore < parScore)
			grade = Grade.ThreeStar;
		else if (playerScore == parScore)
			grade = Grade.TwoStar;
		else if (playerScore > parScore)
			grade = Grade.OneStar;

		if(grade > LevelScores[holeNumber])
			LevelScores[holeNumber] = grade;

		return grade;
	}
}

public class Global : MonoBehaviour {

	public static int LoadHoleNumber = -1;

	private static string fileName = Application.persistentDataPath + "/saveFile.dg";

	private static SavedInformations savedData;
	public static SavedInformations SavedData{ 
		get{
			if(savedData==null)
				savedData = LoadGame();
			return savedData;
		}
	}

	public void Awake()
	{
		DontDestroyOnLoad(this);		
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}

	public static void SaveGame(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (fileName);
		bf.Serialize (file, savedData);
		file.Close ();
	}

	private static SavedInformations LoadGame(){
		if (!File.Exists (fileName))
			return new SavedInformations ();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(fileName, FileMode.Open);
		SavedInformations savedInfo = (SavedInformations)bf.Deserialize (file);
		file.Close ();
		return savedInfo;
	}
}

