using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	public AnimationClip[] anims;
	public Animation box;
	public Image blackBox;
	// public bool fade = false;
	public bool flag = false;
	int counter = 0;


	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		box.AddClip(anims[1], "FadeOut");
		box.AddClip(anims[0], "FadeIn");
	}

	public void fade(STATES state, BackEndManager bm) {
		// box.clip = anims[1];
		box.Play("FadeOut");
		
		if(state == STATES.GAME) {
			AudioManager.instance.startFadeO(true);
			if(GameManager.instance) {
				GameManager.instance.Clear();
			}
			
		} 
		// else if(BackEndManager.instance.prvState == (int)STATES.GAME) {
		// 	AudioManager.instance.startFadeO(false);
		// }
		if(state == STATES.CREDITS || state == STATES.HELP) {
			Invoke("sfx", 0.5f);	
			Invoke("fadeinAlt", 2.0f);
		} else {
			Invoke("sfx", 0.5f);
			Invoke("fadeIn", 2.0F);
		}
	}

	void sfx() {
		if((STATES)BackEndManager.instance.currentState == STATES.GAME) {
			AudioManager.instance.playSound(SFX.SHUFFLE);
		} else {
			AudioManager.instance.playSound(SFX.OFFICESOUNDS);
		}
	}

	void fadeIn() {
		BackEndManager.instance.states[BackEndManager.instance.prvState].SetActive(false);
		BackEndManager.instance.states[BackEndManager.instance.currentState].SetActive(true);

		if(BackEndManager.instance.creditsState != -1) {
			BackEndManager.instance.states[BackEndManager.instance.prvState].SetActive(true);
			// BackEndManager.instance.states[BackEndManager.instance.currentState].SetActive(false);
			// BackEndManager.instance.states[BackEndManager.instance.currentState].SetActive(true);
			BackEndManager.instance.states[(int)STATES.CREDITS].SetActive(false);
			BackEndManager.instance.states[(int)STATES.HELP].SetActive(false);
			BackEndManager.instance.creditsState = -1;
		}

		box.Play("FadeIn");
	}

	void fadeinAlt() {
		BackEndManager.instance.states[BackEndManager.instance.creditsState].SetActive(false);
		BackEndManager.instance.states[BackEndManager.instance.prvState].SetActive(false);
		BackEndManager.instance.states[BackEndManager.instance.currentState].SetActive(true);
		box.Play("FadeIn");
	}


	public void creditsFade() {
		box.Play("FadeOut");
		Invoke("sfx", 0.5f);
		Invoke("creditFadeIn", 2.0F);
	}


	void creditFadeIn() {
		box.Play("FadeIn");
	}




	IEnumerator wait(BackEndManager bm) {
		yield return new WaitForSeconds(2.0f);
		bm.states[bm.prvState].SetActive(false);
		bm.states[bm.currentState].SetActive(true);
		box.clip = anims[0];
		box.Play();
	}



}
