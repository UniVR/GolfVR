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
	public float Distance;
	public float MaxMove;

	private float beginAngle = -30f;

	private bool isInit = false;
	private Button button; 
	private Color oldColor;
	private bool watched = false;
	private Vector3 initialPosition;
	
	public void Init(Canvas menu, int index){
		isInit = true;
		Selected = false;
		button = this.GetComponent<Button> ();
		oldColor = button.image.color;
		transform.localPosition = new Vector3(0f, 0f, Distance);
		var playerPosition = MainScript.Get().Player.transform.position;
		transform.RotateAround(playerPosition, Vector3.up, beginAngle + index * 30f);
		initialPosition = transform.localPosition;
		//transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (maxDistance - playerDistance) * 20);
		//transform.LookAt(2 * transform.position - new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
	}

	void FixedUpdate () {
		if (watched && !Selected) {
			var pos = this.transform.localPosition;
			if(pos.z > Distance-MaxMove){
				this.transform.position =  Vector3.MoveTowards(transform.position, MainScript.Get().GetHeadPosition(), MoveVelocity/3);//new Vector3 (pos.x, pos.y, pos.z - MoveVelocity);
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
		BagScript.ButtonWatched = true;
		watched = true;
	}

	public void PointerExit(){
		BagScript.ButtonWatched = false;
		watched = false;
		this.transform.localPosition = initialPosition;
	}
}
