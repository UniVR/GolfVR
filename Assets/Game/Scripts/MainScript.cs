using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Cloud.Analytics;

public enum Difficulty{
	Easy,
	Medium,
	Hard
}

public enum ActionState{
	Idle,
	Locking,
	UnLocking,
	Loading,
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
	public CardboardScript Cardboard;
	public PlayerScript Player;
	public ClubScript Club;
	public BallScript Ball;
	public BallInfoScript BallInfo;
	public HolesScript Holes;
	public HudScript Hud;
	public ClubsBagScript Bag;
	public WindScript Wind;
	public AnemoScript Anemometer;

	public Difficulty Difficulty;

	private int score;
	private bool locked;
	private bool watchBallLock;
	private bool watchBall;

	private AudioSource applause;

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
		locked = false;
		watchBall = false;
		currentAction = ActionState.Idle;

		SetWind ();

		applause = GetComponent<AudioSource> ();
	}

	/*
	 * 	Action
	 */
	void FixedUpdate () {
		switch (currentAction) 
		{
			case ActionState.Idle:
				if(watchBallLock){
					currentAction = ActionState.Locking;
				}
			break;


			case ActionState.Locking:
				if(!watchBallLock){
					currentAction = ActionState.UnLocking;
				}else{
					if(Hud.Locking()){
						Ball.WatchBall.SetActive(true);	
						currentAction = ActionState.Loading;
					}
				}
			break;


			case ActionState.UnLocking:
				if(watchBallLock){
					currentAction = ActionState.Locking;
				}
				if(Hud.UnLocking()){
					currentAction = ActionState.Idle;
				}
			break;


			case ActionState.Loading:
				if(!watchBall && !watchBallLock){
					currentAction = ActionState.Firing;	
					Ball.WatchBall.SetActive(false);
				}

				Club.Load();
				Hud.SetPowerBarAmount(Club.LoadingAmount());
			break;

			case ActionState.Firing:
				Hud.SetPowerBarAmount(0);				
				Club.Fire();

				if(!Ball.IsShooted() && Club.HasShooted())							//Shoot now
				{
					Ball.Shoot(Club.LoadingTime * Club.clubForceCoef, Club.clubAngle, Player.transform.eulerAngles.y);
					Hud.UpdateScore(score++);

					UnityAnalytics.CustomEvent("Shoot", new Dictionary<string, object>
					{
						{ "TimeElapsed", Time.timeSinceLevelLoad },
						{ "Score", score },
						{ "ClubUsed", Club.name },
						{ "LoadingTime", Club.LoadingTime }
					});
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
				//BallInfo.ShowInformation(Ball.transform.position, Localization.Hole);
				Hud.FadeOut();	
				currentAction = ActionState.MoveToTheBall;

				UnityAnalytics.CustomEvent("Won", new Dictionary<string, object>
                {
					{ "TimeElapsed", Time.timeSinceLevelLoad },
					{ "Score", score }
				});
			break;

			case ActionState.OutOfBound:	
				BallInfo.ShowInformation(Ball.transform.position, Localization.OutOfZone);	
				Ball.StopAndGetBackToOldPos();
				Hud.UpdateScore(score++);
				Hud.FadeOut();	
				currentAction = ActionState.MoveToTheBall;	

				UnityAnalytics.CustomEvent("OutOfBound", new Dictionary<string, object>
                {
					{ "TimeElapsed", Time.timeSinceLevelLoad },
					{ "Score", score }
				});
			break;

			case ActionState.MoveToTheBall:	
				if(Hud.IsFadingIn()){
					Bag.MoveToTheBall(Ball.transform.position, Holes.CurrentHole.transform.position);
					Player.transform.position = Ball.transform.position;
					Club.Reset();	
					SetWind ();
					Hud.EnableReticle(true);
					currentAction = ActionState.Idle;
					locked = false;
				}
			break;
		}
	}



	public void SetCurrentClub(ClubScript clubScript){
		//Instantiate the new Club with the properties of the old one
		GameObject club = Club.InstantiateNewClub (clubScript.gameObject);
		Club = club.GetComponent<ClubScript>();
		Player.SetCurrentClub (club);

		UnityAnalytics.CustomEvent("ChangeClub", new Dictionary<string, object>
        {
			{ "TimeElapsed", Time.timeSinceLevelLoad },
			{ "Club", clubScript.name }
		});
	}

	public GameObject GetCurrentClub(){
		return Player.GetCurrentClub ();
	}


	public HoleScript GetCurrentHole(){
		return Holes.CurrentHole;
	}

	public void SetWind(){
		var orientation = Random.Range (0, 360);

		var currentHole = GetCurrentHole ();
		var force = currentHole.GetWindPower ();

		Wind.SetOrientation (orientation);
		Wind.SetVelocity(force);
		Anemometer.SetOrientation (orientation-180); //Invert from wind
		Anemometer.SetRotationSpeed (force);

		UnityAnalytics.CustomEvent("Wind", new Dictionary<string, object>
        {
			{ "TimeElapsed", Time.timeSinceLevelLoad },
			{ "Orientation", orientation },
			{ "Force", force }
		});
	}

	/*
	 * Watching ball (shoot/release)
	 */
	public void WatchBall(){
		watchBall = true;
	}

	public void UnWatchBall(){
		watchBall = false;
	}

	public void WatchBallLock(){
		watchBallLock = true;
	}
	
	public void UnWatchBallLock(){
		watchBallLock = false;
	}




	/*
	 * 	Events
	 */
	public void EnterHole()
	{
		currentAction = ActionState.Won;
		applause.Play ();
	}

	public void Win(){
		// TODO : Adding condition
		// Application.LoadLevel ("Exit");
	}
}
