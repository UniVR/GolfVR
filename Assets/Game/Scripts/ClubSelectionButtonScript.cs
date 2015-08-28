using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClubSelectionButtonScript : MonoBehaviour {

	[HideInInspector]
	public ClubScript ClubScript;
	[HideInInspector]
	public bool Selected;
	[HideInInspector]
	public ClubsBagScript BagScript;

	public float MoveVelocity;
	public float MaxMove;

	private bool isInit = false;
	private Button button; 
	private Color oldColor;
	private bool watched = false;
	private Vector3 initialPosition;
	
	public void Init(Canvas menu, float posX){
		isInit = true;
		Selected = false;
		button = this.GetComponent<Button> ();
		oldColor = button.image.color;
		transform.localPosition = new Vector3(posX, 0f, 0f);
		initialPosition = transform.localPosition;

		var playerDistance = Vector3.Distance(MainScript.Get().Player.transform.position, transform.position);
		var playerPosition = MainScript.Get().Player.transform.position;
		var maxDistance = Vector3.Distance(playerPosition, menu.transform.position);
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (maxDistance - playerDistance) * 7);
		transform.LookAt(2 * transform.position - new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
	}

	void FixedUpdate () {
		if (watched && !Selected) {
			var pos = this.transform.localPosition;
			if(pos.z > -MaxMove){
				this.transform.position =  Vector3.MoveTowards(transform.position, MainScript.Get().CardboardGameObject.transform.position, MoveVelocity/3);//new Vector3 (pos.x, pos.y, pos.z - MoveVelocity);
			}
			else{
				BagScript.DeactiveAllButton();
				Select (true);
				BagScript.SelectClub(ClubScript);
			}
		}

	}

	public void Select(bool isSelected){
		if (isSelected) {
			button.image.color = Color.green;
			this.transform.localPosition = initialPosition;
			Selected = true;
			watched = false;
		}
		else{
			button.image.color = oldColor;
			Selected = false;
		}
	}

	public void PointerEnter(){
		watched = true;
	}

	public void PointerExit(){
		watched = false;
		this.transform.localPosition = initialPosition;
	}
}
