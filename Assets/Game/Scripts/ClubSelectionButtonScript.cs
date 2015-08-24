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

	public float ScaleVelocity;
	public float MaxScale;

	public float MoveVelocity;
	public float MaxMove;

	private bool isInit = false;
	private Button button; 
	private Color oldColor;
	private bool watched = false;
	private Vector3 initialScale;
	private Vector3 initialPosition;


	void Start(){
		if (!isInit)
			Init ();
	}

	public void Init(){
		isInit = true;
		Selected = false;
		button = this.GetComponent<Button> ();
		oldColor = button.image.color;
		initialScale = this.transform.localScale;
		initialPosition = this.transform.localPosition;
	}

	void FixedUpdate () {
		if (watched && !Selected) {
			var scale = this.transform.localScale;
			var pos = this.transform.localPosition;
			if(scale.x < MaxScale && scale.y < MaxScale && scale.z < MaxScale && pos.z > -MaxMove){
				//this.transform.localScale = new Vector3 (scale.x + ScaleVelocity, scale.y + ScaleVelocity, scale.z + ScaleVelocity);
				this.transform.localPosition = new Vector3 (pos.x, pos.y, pos.z - MoveVelocity);
			}
			else{
				BagScript.DeactiveAllButton();
				Select (true);
				button.onClick.Invoke();
			}
		}

	}

	public void Select(bool isSelected){
		if (isSelected) {
			button.image.color = Color.green;
			this.transform.localScale = initialScale;
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
		this.transform.localScale = initialScale;
		this.transform.localPosition = initialPosition;
	}
}
