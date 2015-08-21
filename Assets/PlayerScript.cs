using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public GameObject Club;

	public void SetCurrentClub(GameObject newClub){

		foreach (Transform child in Club.transform) {
			GameObject.Destroy(child.gameObject);
		}

		GameObject instantiateClub = (GameObject)GameObject.Instantiate(newClub);
		instantiateClub.transform.SetParent(Club.transform, false);
	}
}
