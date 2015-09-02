using UnityEngine;
using System.Collections;

public class MainMenu_Door : MonoBehaviour {

	Animator animator;
	int openHash = Animator.StringToHash ("Open");
	int idleHash = Animator.StringToHash ("Idle");
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool open = Input.GetKey (KeyCode.Space);
		if(open)
			animator.SetTrigger ("Close");
		if (open) {
			//animator.SetTrigger (openHash);
			animator.SetBool ("DoOpen", true);
		}// else {
	//		animator.SetTrigger (idleHash);
	//	}

		//Get the current state
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
	}
}
