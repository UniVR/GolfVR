using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject Club;
	private GameObject CurrentClub;

	public void SetCurrentClub(GameObject newClub){

		foreach (Transform child in Club.transform) {
			GameObject.Destroy(child.gameObject);
		}

		newClub.transform.SetParent(Club.transform, false);
		CurrentClub = newClub;
	}

	public GameObject GetCurrentClub(){
		if (CurrentClub == null) {
			CurrentClub = Club.transform.GetChild(0).gameObject;
		}
		return CurrentClub;
	}
}
