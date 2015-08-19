using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ActionState{
	Idle,
	Loading,
	Loaded,
	Firing,
	Fired, 
	MoveToTheBall,
	Won,
	OutOfBound
}

public class MainScript : MonoBehaviour {

	[HideInInspector]
	public ActionState currentAction;

	[HideInInspector]
	public Terrain CurrentTerrain{
		get{ return GetCurrentHole ().Terrain; }
	}

	/*
	 * 	Public Properties
	 */
	public GameObject CardboardGameObject; 	//TODO script
	public GameObject Player;				//TODO script

	public ClubScript Club;
	public BallScript Ball;
	public HolesScript Holes;
	public HudScript Hud;

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
	}


	void FixedUpdate () {
		/*
		 * 	Action
		 */
		switch (currentAction) 
		{
			/*
			 * Idle
			 */
			case ActionState.Idle:
				/*if(!Hud.IsVisible()
			   		&& currentMovement!=MovementState.FadeIn
			   		&& currentMovement!=MovementState.FadeOut
			   		&& currentMovement!=MovementState.MoveToTheBall)
				{
					Hud.SetVisible(true);
				}*/
			break;

			/*
			 * Loading
			 */
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

			/*
			 * Loaded
			 */
			case ActionState.Loaded:
			break;

			/*
			 * Firing
			 */
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

			/*
			 * Fired
			 */
			case ActionState.Fired:					
				if(Ball.IsOutOfBound()){
					currentAction = ActionState.OutOfBound;
					break;
				}		   		
				if(Ball.IsStopped())
				{
					Club.Reset();
					Hud.FadeOut();
					currentAction = ActionState.MoveToTheBall;
				}				
			break;

			case ActionState.Won:	
				Hud.ShowInformation(Localization.Hole);
				currentAction = ActionState.Fired;
				Ball.StopAndMove(Holes.CurrentHole.BeginPosition.transform.position);
			break;

			case ActionState.OutOfBound:	
				Hud.UpdateScore(score++);
				Hud.ShowInformation(Localization.OutOfZone);
				Ball.StopAndGetBackToOldPos();
				Club.Reset();
				Hud.FadeOut();	
				currentAction = ActionState.MoveToTheBall;				
			break;

			case ActionState.MoveToTheBall:	
				Player.transform.position = Ball.transform.position;
				//TODO: find how rotate the player to front the ball... !
				//Player.transform.LookAt(Holes.CurrentHole.transform);
				//Cardboard.SDK.HeadPose.Orientation.SetLookRotation(Holes.CurrentHole.transform.position);
				//var vectorHeadToHole = Holes.CurrentHole.transform.position - Cardboard.SDK.HeadPose.Position;
				//Cardboard.SDK.HeadPose.Orientation.SetLookRotation(vectorHeadToHole);
				//var vectorHeadToHole = Holes.CurrentHole.transform.position - Cardboard.SDK.HeadPose.Position;
				//var orientation = Quaternion.LookRotation(vectorHeadToHole, Vector3.up);
				//((MutablePose3D) Cardboard.SDK.HeadPose).Set(Cardboard.SDK.HeadPose.Position, orientation);	
				//Cardboard.SDK.HeadPose.Orientation.Set(orientation.x, orientation.y, orientation.z, orientation.w);
				//Player.transform.LookAt(Holes.CurrentHole.transform);
				//Debug.Log("########### MOVE !");
				if(Hud.IsFadedOut()){
					currentAction = ActionState.Idle;
				}
			break;
		}


		/*
		 * 	New rotation system (Every value here is in degree°)
		 */
		if (currentAction != ActionState.MoveToTheBall) {
			var headRotation = Cardboard.SDK.HeadPose.Orientation.eulerAngles;		// Head rotation
			var horizontalNeckRotation = headRotation.y;					// y rotation of the neck (horizontally)
			var forwardNeckRotation = headRotation.x;						// x rotation of the neck (forward) 
			var neckVector = Cardboard.SDK.HeadPose.Orientation * Vector3.up;		// Neck vector
		
			var forwardRotationThresholdMin = 10; 							// Player look in direction of the ground/ball
			var forwardRotationThresholdMax = 90; 	

			// Player look at the horizon (normal rotation around the ball)
			var lookHorizontally = forwardNeckRotation < forwardRotationThresholdMin || forwardNeckRotation > forwardRotationThresholdMax;
			if (lookHorizontally) {
				Player.transform.eulerAngles = new Vector3 (0, horizontalNeckRotation, 0);
			} else { // Player look at the ground we follow his neck direction (Horizontal projection of the neck vector)
				var direction = new Vector3 (neckVector.x, 0, neckVector.z);
				Player.transform.rotation = Quaternion.LookRotation (direction); // TODO: add a little threshold?
			}

			//Cardboard top/bottom rotation
			var cardBoardVect = CardboardGameObject.transform.eulerAngles;
			CardboardGameObject.transform.eulerAngles = new Vector3 (forwardNeckRotation, cardBoardVect.y, cardBoardVect.z);
			//Debug.Log("NECK: " + Cardboard.SDK.HeadPose.Orientation  * Vector3.up);
		}
	}




	public HoleScript GetCurrentHole(){
		return Holes.CurrentHole;
	}

	/*
	 * Watching ball (shoot/release)
	 */
	public void LoadShoot(){
		//ballIsWatched = true;
		if (currentAction == ActionState.Idle)
			currentAction = ActionState.Loading;
		/*
		 * TODO: Sounds
		 */
	}

	public void ReleaseShoot(){
		//ballIsWatched = false;
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
