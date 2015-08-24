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

	private bool isInit = false;
	private Button button; 
	private Color oldColor;
	private bool watched = false;
	private Vector3 initialScale;
	private float maxScale;

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
				Select (true);
				button.onClick.Invoke();
			}
		}

	}

	public void Select(bool isSelected){
		if (isSelected) {
			button.image.color = Color.green;
			this.transform.localScale = initialScale;
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
	}
}
