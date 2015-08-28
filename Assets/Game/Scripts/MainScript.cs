using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ActionState{
	Idle,
	Loading,
	Loaded,
	Firing,
	Fired, 
	Won,
	OutOfBound,
	MoveToTheBall
}

public class MainScript : MonoBehaviour {

	[HideInInspector]
	public ActionState currentAction;
		
	public Terrain CurrentTerrain{
		get{ return GetCurrentHole ().Terrain; }
	}

	/*
	 * 	Public Properties
	 */
	public GameObject CardboardGameObject; 	//TODO script

	public PlayerScript Player;
	public ClubScript Club;
	public BallScript Ball;
	public HolesScript Holes;
	public HudScript Hud;
	public ClubsBagScript Bag;
	public WindScript Wind;
	public AnemoScript Anemometer;

	private int score;


	// Singleton
	private static MainScript instance;
	public static MainScript Get(){
		return instance;
	}

	/*
	 * Initialisation
	 */
	void Start () {
		instance = this;

		score = 0;
		currentAction = ActionState.Idle;

		SetWind ();
	}

	/*
	 * 	Action
	 */
	void FixedUpdate () {

		switch (currentAction) 
		{
			case ActionState.Idle:
				//Do nothing
			break;



			case ActionState.Loading:
				if(!Club.IsLoaded())
				{
					Club.Load();
					Hud.SetPowerBarAmount(Club.LoadingAmount());
				}
				else
				{
					currentAction = ActionState.Loaded;
				}
			break;



			case ActionState.Loaded:
				//Wait
			break;



			case ActionState.Firing:
				Hud.SetPowerBarAmount(0);				
				Club.Fire();

				if(!Ball.IsShooted() && Club.HasShooted())							//Shoot now
				{
					Ball.Shoot(Club.LoadingTime * Club.clubForceCoef, Club.clubAngle, Player.transform.eulerAngles.y);
					Hud.UpdateScore(score++);
				}
				else if (Club.IsFired())
				{					
					currentAction = ActionState.Fired;
				}
			break;



			case ActionState.Fired:					
				if(Ball.IsOutOfBound()){
					currentAction = ActionState.OutOfBound;
					break;
				}		   		
				if(Ball.IsStopped())
				{
					Hud.FadeOut();
					currentAction = ActionState.MoveToTheBall;
				}				
			break;

			case ActionState.Won:	
				Ball.StopAndMove(Holes.CurrentHole.BeginPosition.transform.position); //Go to next hole
				Hud.ShowInformation(Localization.Hole);
				Hud.FadeOut();	
				currentAction = ActionState.MoveToTheBall;
			break;

			case ActionState.OutOfBound:	
				Ball.StopAndGetBackToOldPos();
				Hud.UpdateScore(score++);
				Hud.ShowInformation(Localization.OutOfZone);
				Hud.FadeOut();	
				currentAction = ActionState.MoveToTheBall;				
			break;

			case ActionState.MoveToTheBall:	
				if(Hud.IsFadingIn()){
					Player.transform.position = Ball.transform.position;
					Club.Reset();	
					Bag.MoveToTheBall(Ball.transform.position, Holes.CurrentHole.transform.position);
					SetWind ();
					currentAction = ActionState.Idle;
				}
			break;
		}


		/*
		 * 	Rotation system
		 */
		var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;		// Head rotation

		var forwardRotationThresholdMin = 10; 							// Player look in direction of the ground/ball
		var forwardRotationThresholdMax = 90; 	

		// Player look at the horizon (normal rotation around the ball)
		var lookHorizontally = headRotation.x < forwardRotationThresholdMin || headRotation.x > forwardRotationThresholdMax;
		if (lookHorizontally) {
			Player.transform.eulerAngles = new Vector3 (0, headRotation.y, 0);
			var cardboardRotation = CardboardGameObject.transform.eulerAngles;		//Gameobject parent to the Cardboard head (cancel SDK rotations)
			CardboardGameObject.transform.eulerAngles = new Vector3 (headRotation.x, cardboardRotation.y, headRotation.z);
		}  // Player look at the ground we follow his neck direction (Horizontal projection of the neck vector)
		else { 
			var neckVector = Cardboard.SDK.HeadPose.Orientation * Vector3.up;		// Neck vector
			var direction = new Vector3 (neckVector.x, 0, neckVector.z);
			Player.transform.rotation = Quaternion.LookRotation (direction); // TODO: add a little threshold?
			CardboardGameObject.transform.eulerAngles = new Vector3 (headRotation.x, headRotation.y, headRotation.z);
		}

		//Cardboard top/bottom rotation
		//var cardboardRotation = CardboardGameObject.transform.eulerAngles;		//Gameobject parent to the Cardboard head (cancel SDK rotations)
		//CardboardGameObject.transform.eulerAngles = new Vector3 (forwardNeckRotation, cardboardRotation.y, cardboardRotation.z);
		//Debug.Log("NECK: " + Cardboard.SDK.HeadPose.Orientation  * Vector3.up);


	}



	public void SetCurrentClub(ClubScript clubScript){
		//Instantiate the new Club with the properties of the old one
		GameObject club = Club.InstantiateNewClub (clubScript.gameObject);
		Club = club.GetComponent<ClubScript>();
		Player.SetCurrentClub (club);
	}

	public GameObject GetCurrentClub(){
		return Player.GetCurrentClub ();
	}


	public HoleScript GetCurrentHole(){
		return Holes.CurrentHole;
	}

	public void SetWind(){
		var orientation = Random.Range (0, 360);
		var force = Random.Range (0, 10);

		Wind.SetOrientation (orientation);
		Wind.SetVelocity(force);
		Anemometer.SetOrientation (orientation-180); //Invert from wind
		Anemometer.SetRotationSpeed (force);
	}

	/*
	 * Watching ball (shoot/release)
	 */
	public void LoadShoot(){
		if (currentAction == ActionState.Idle)
			currentAction = ActionState.Loading;
		/*
		 * TODO: Sounds
		 */
	}

	public void ReleaseShoot(){
		if (currentAction == ActionState.Loading || currentAction == ActionState.Loaded)
			currentAction = ActionState.Firing;
		/*
		 * TODO: Sounds
		 */
	}




	/*
	 * 	Events
	 */
	public void EnterHole()
	{
		currentAction = ActionState.Won;
	}

	public void Win(){
		// TODO
	}
}
