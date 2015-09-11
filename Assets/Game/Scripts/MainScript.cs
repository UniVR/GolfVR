using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public HolesScript Holes;
	public HudScript Hud;
	public ClubsBagScript Bag;
	public WindScript Wind;
	public AnemoScript Anemometer;

	public Difficulty Difficulty;

	private int score;
	private bool locked;
	private bool watchBall;


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
	}

	/*
	 * 	Action
	 */
	void FixedUpdate () {
		/*
		if (!locked && Cardboard.WatchDown ()) {
			currentAction = ActionState.Locking;
		} 
		else if (currentAction == ActionState.Locking && !locked && !Cardboard.WatchDown ()) {
			currentAction = ActionState.UnLocking;
		}
		else if (locked && watchBall) 
		{
			currentAction = ActionState.Loading;
		} 
		else if (currentAction == ActionState.Loading || currentAction == ActionState.Loaded && !watchBall) {
			currentAction = ActionState.Firing;
		}*/

		switch (currentAction) 
		{
			case ActionState.Idle:
				if(watchBall){
					currentAction = ActionState.Loading;
				}
			break;


			case ActionState.Locking:
				if(!watchBall){
					currentAction = ActionState.Idle;
				}else{
					currentAction = ActionState.Loading;
				}
			break;


			case ActionState.UnLocking:
				if(watchBall){
					currentAction = ActionState.Locking;
				}
			break;


			case ActionState.Loading:
				if(!watchBall){
					currentAction = ActionState.Firing;	
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
