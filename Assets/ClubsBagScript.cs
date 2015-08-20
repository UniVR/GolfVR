using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ClubsBagScript : MonoBehaviour {

	private string ButtonAsset = "ClubSelectionButton";

	public Canvas Menu;
	public List<GameObject> Clubs;

	private List<Button> buttons;

	void Start(){
		foreach (GameObject club in Clubs) {
			var ressource = Resources.Load<Button>(ButtonAsset);
			Debug.Log(ButtonAsset);
			Button clubButton = (Button)GameObject.Instantiate(ressource);
			clubButton.transform.SetParent(Menu.transform, false);
			//clubButton.onClick.AddListener(delegate() {/* StartGame("Level1"); */});
			buttons.Add(clubButton);
		}
	}

	public void OpenMenu(){

	}

	public void CloseMenu(){

	}

}
