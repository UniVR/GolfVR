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

	private Button button; 
	private Color oldColor;
	private bool watched = false;
	private Vector3 initialScale;
	private float maxScale;

	void Start(){
		Selected = false;
		button = this.GetComponent<Button> ();
		oldColor = button.image.color;
		initialScale = this.transform.localScale;
		maxScale = 2.5f;
	}

	void FixedUpdate () {
		if (watched && !Selected) {
			var scale = this.transform.localScale;
			if(scale.x < maxScale && scale.y < maxScale && scale.z < maxScale){
				this.transform.localScale = new Vector3 (scale.x + ScaleVelocity, scale.y + ScaleVelocity, scale.z + ScaleVelocity);
			}
			else{
				BagScript.DeactiveAllButton();
				button.onClick.Invoke();
				button.image.color = Color.green;
				this.transform.localScale = initialScale;
				Selected = true;
			}
		}
		if (Selected == false && button.image.color == Color.green) {
			button.image.color = oldColor;
		}
		if (Selected) {
			watched = false;
		}
	}

	public void PointerEnter(){
		watched = true;
	}

	public void PointerExit(){
		watched = false;
		this.transform.localScale = initialScale;
	}
}
