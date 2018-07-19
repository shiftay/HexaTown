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
		
	}

	void Start() {
	}

	public void back() {
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
}
