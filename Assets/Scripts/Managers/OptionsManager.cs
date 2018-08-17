using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {
	BackEndManager bm;
	public Slider music;
	public Slider sfx;
	public Sprite muted;
	public Sprite unmuted;
	public GameObject phone;
	public GameObject gameBtns;
	public GameObject outofGameBtns;
	bool movePhone;
	public Animation anim;
	public AnimationClip clip;
	public AnimationClip away;
	public Image mutedImg;
	

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		if(BackEndManager.instance) {
			bm = BackEndManager.instance;
			float temp = bm.currentVolume;
			music.value = temp;
			sfx.value = bm.currentSFX;
		}

		if((STATES)BackEndManager.instance.prvState == STATES.GAME) {
			gameBtns.SetActive(true);
			outofGameBtns.SetActive(false);
		} else {
			gameBtns.SetActive(false);
			outofGameBtns.SetActive(true);
		}

		if(BackEndManager.instance.mutedMusic) {
			mutedImg.sprite = muted;
		} else {
			mutedImg.sprite = unmuted;
		}

		// phone.transform.position = startPos;
		movePhone = true;
		
	}
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(movePhone) {
			anim.clip = clip;
			anim.Play();
			movePhone = false;
		}
	}


	public void back() {
		anim.clip = away;
		anim.Play();
		Invoke("revert", 1.0f);
	}

	public void revert() {
		if((STATES)BackEndManager.instance.prvState == STATES.GAME) {
			GameManager.instance.unPause();
		}
		bm.RevertState();
	}

	public void mute(Image test) {
		if(test.sprite == muted && !(sfx.value == 0 && music.value == 0)) {
			test.sprite = unmuted;	// unmute
		} else {
			test.sprite = muted; 	//mute
		}

		if(muted) {
			muteVolumes();
		}

		AudioManager.instance.mute(test.sprite == muted);
	}

	public void musicChanged() {
		BackEndManager.instance.currentVolume = music.value;
		AudioManager.instance.musicSource.volume = BackEndManager.instance.currentVolume;

		if(music.value > 0) {
			mutedImg.sprite = unmuted;
			AudioManager.instance.mute(false);
		} else if(sfx.value == 0 && music.value == 0) {
			mutedImg.sprite = muted;
			AudioManager.instance.mute(true);			
		}
	}
	void muteVolumes() {
		BackEndManager.instance.currentVolume = 0;
		AudioManager.instance.musicSource.volume = BackEndManager.instance.currentVolume;
		BackEndManager.instance.currentSFX = 0;
		AudioManager.instance.sfxSource.volume = BackEndManager.instance.currentSFX;
		music.value = 0;
		sfx.value = 0;
	}

	public void sfxChanged() {
		BackEndManager.instance.currentSFX = sfx.value;
		AudioManager.instance.sfxSource.volume = BackEndManager.instance.currentSFX;

		if(sfx.value > 0) {
			mutedImg.sprite = unmuted;
			AudioManager.instance.mute(false);
		} else if(sfx.value == 0 && music.value == 0) {
			mutedImg.sprite = muted;
			AudioManager.instance.mute(true);			
		}	
	}

	public void Credits() {
		BackEndManager.instance.ChangeState(STATES.CREDITS);
	}

	public void help() {
		BackEndManager.instance.ChangeState(STATES.HELP);
	}
	public void ExitGame() {
		anim.clip = away;
		anim.Play();
		Invoke("exit", 1.0f);
	}

	void exit() {
		BackEndManager.instance.LeaveGame();
		AudioManager.instance.MenuMusic();
	}
}
