using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// *** Button script ***
/// Control how the GUI will react 
/// </summary>
public class ButtonScript : MonoBehaviour {

	private bool isViewed = false;
	private float time = 0f;
	
	// Update is called once per frame
	void OnGUI () {
		if (isViewed) {
			Button button = GetComponent<Button>();
			Color color = button.image.color;
			button.image.color =  new Color(color.r-0.005f, color.g-0.001f, color.b-0.005f); 
			
			time += 0.5f;
			if(time>100f)
				button.OnPointerClick (new PointerEventData(EventSystem.current));
		}
	}

	public void EnterButton(){
		if (!isViewed) {
			isViewed = true;
		}
	}
	
	public void ExitButton(){
		if (isViewed) {
			isViewed = false;
			Button button = GetComponent<Button>();
			button.image.color = Color.white;
			time = 0f;
		}
	}
}
