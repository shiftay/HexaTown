using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {


	public Text title;
	public Text reason;
	public Animation anim;
	public AnimationClip putAway;
	public AnimationClip move;

	
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		anim.clip = move;
		anim.Play();
		if(BackEndManager.instance.gameWon) {
			title.text = "Congrats!";
			reason.text = "You've collected enough minerals!";
		} else {
			title.text = "Game Over";
			
			if(GameManager.instance.currentDeck.Count == 0) {
				reason.text = "You ran out of cards.";
			} else {
				reason.text = "You didn't have anymore plays you could make.";
			}
		}
	}

	public void homeBtn() {
		anim.clip = putAway;
		anim.Play();
		Invoke("pre", 1.5f);
	}

	public void statsBtn() {
		anim.clip = putAway;
		anim.Play();
		Invoke("end", 1.5f);
	}


	void end() {
		BackEndManager.instance.ChangeState(STATES.ENDGAME);
		this.gameObject.SetActive(false);
	}

	void pre() {
		BackEndManager.instance.ChangeState(STATES.PREGAME);
		this.gameObject.SetActive(false);
	}
}
