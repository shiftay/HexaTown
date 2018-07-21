using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {
	BackEndManager bm;
	public Slider music;
	public Slider sfx;
	bool firstRun = true;
	public Sprite muted;
	public Sprite unmuted;
	public GameObject phone;
	public GameObject gameBtns;
	public GameObject outofGameBtns;
	bool movePhone;
	Vector3 startPos;
	Vector3 endPos;
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

	void Start() {
		startPos = phone.transform.position;
		Vector3 temp = startPos;
		temp.y += 260;
		endPos = temp;
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
		if(test.sprite == muted) {
			test.sprite = unmuted;	// unmute
		} else {
			test.sprite = muted; 	//mute
		}

		AudioManager.instance.mute(test.sprite == muted);
	}

	public void musicChanged() {
		BackEndManager.instance.currentVolume = music.value;
		AudioManager.instance.musicSource.volume = BackEndManager.instance.currentVolume;
	}

	public void sfxChanged() {
		BackEndManager.instance.currentVolume = sfx.value;
		AudioManager.instance.musicSource.volume = BackEndManager.instance.currentSFX;
	}

	public void Credits() {
		BackEndManager.instance.ChangeState(STATES.CREDITS);
	}
	public void ExitGame() {
		anim.clip = away;
		anim.Play();
		Invoke("exit", 1.0f);
	}

	void exit() {
		BackEndManager.instance.LeaveGame();
	}
}
