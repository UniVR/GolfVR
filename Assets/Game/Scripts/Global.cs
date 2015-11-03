using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;


[Serializable]
public class SavedInformations{
	public int UnlockedLevel;
	public List<int> LevelScores;
}

public class Global : MonoBehaviour {

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
		Debug.Log ("AWAKE");
	}

	public static void SaveGame(){
		SavedInformations save = new SavedInformations ();
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (fileName);
		bf.Serialize (file, save);
		file.Close ();
	}

	public static SavedInformations LoadGame(){
		if (!File.Exists (fileName))
			return new SavedInformations ();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(fileName, FileMode.Open);
		SavedInformations savedInfo = (SavedInformations)bf.Deserialize (file);
		file.Close ();
		return savedInfo;
	}
}

