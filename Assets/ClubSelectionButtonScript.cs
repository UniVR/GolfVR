using UnityEngine;
using System.Collections;

public class ClubSelectionButtonScript : MonoBehaviour {

	[HideInInspector]
	public ClubScript ClubScript;

	public float ScaleVelocity;

	private bool watched = false;
	private Vector3 initialScale;
	private float maxScale;

	void Start(){
		initialScale = this.transform.localScale;
		maxScale = 2.5f;
	}

	void FixedUpdate () {
		if (watched) {
			var scale = this.transform.localScale;
			if(scale.x < maxScale && scale.y < maxScale && scale.z < maxScale)
				this.transform.localScale = new Vector3 (scale.x + ScaleVelocity, scale.y + ScaleVelocity, scale.z + ScaleVelocity);
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
