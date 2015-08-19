using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public enum FadeDirection{
	FadeIn,
	FadeOut,
	None
}


public class HudScript : MonoBehaviour {
	
	//Fade plane
	public GameObject FadePlane;
	public float FadeSpeed;
	private FadeDirection fadeDirection;
	private Material fadePlaneMaterial;
	private float fadeAlphaValue;

	//Score
	public Text ScoreHUD;

	//Informations
	public Text InformationsHUD;
	public float InformationShowTime;
	private float InformationTimeShowed;

	//Powerbar
	public Image PowerBar;


	// Use this for initialization
	void Start () {
		FadePlane.SetActive (false);
		fadePlaneMaterial = FadePlane.GetComponent<Renderer>().material;
		fadeDirection = FadeDirection.None;
		PowerBar.enabled = false;
		ScoreHUD.text = Localization.Score + 0;	
	}
	
	// Update is called once per frame
	void Update () {
		if(fadeDirection == FadeDirection.FadeOut) {
			fadePlaneMaterial.color = new Color(fadePlaneMaterial.color.r, fadePlaneMaterial.color.g, fadePlaneMaterial.color.b, fadeAlphaValue);
			if(fadeAlphaValue>=1f)
				fadeDirection = FadeDirection.FadeIn;
			fadeAlphaValue += Time.deltaTime / FadeSpeed;
		}
		else if (fadeDirection == FadeDirection.FadeIn) {
			fadePlaneMaterial.color = new Color(fadePlaneMaterial.color.r, fadePlaneMaterial.color.g, fadePlaneMaterial.color.b, fadeAlphaValue);
			if(fadeAlphaValue<=0f){
				FadePlane.SetActive (false);
				fadeDirection = FadeDirection.None;	
			}
			fadeAlphaValue -= Time.deltaTime / FadeSpeed;
		}


		if (InformationTimeShowed > 0) {
			InformationTimeShowed -= Time.deltaTime;
		} else {
			InformationsHUD.enabled = false;
		}
	}

	
	public void FadeOut(){
		fadeAlphaValue = 0;
		FadePlane.SetActive (true);
		fadeDirection = FadeDirection.FadeOut;
	}

	public bool IsFadingOut(){
		return fadeDirection == FadeDirection.FadeOut;
	}

	public bool IsFadingIn(){
		return fadeDirection == FadeDirection.FadeIn;
	}

	public void SetPowerBarAmount(float amount){
		if (amount == 0) {
			PowerBar.enabled = false;
		} else {
			PowerBar.enabled = true;
			PowerBar.fillAmount = amount;
		}
	}

	public void UpdateScore(int score){
		ScoreHUD.text = Localization.Score + score;	
	}
	
	public void ShowInformation(string text){
		InformationsHUD.text = text;
		InformationsHUD.enabled = true;
		InformationTimeShowed = InformationShowTime;
	}
}
