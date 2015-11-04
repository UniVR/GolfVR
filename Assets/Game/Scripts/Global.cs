using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;


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
	public List<int> LevelScores;

	public SavedInformations(){
		unlockedLevel = 0;
		LevelScores = new List<int>();
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

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
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

