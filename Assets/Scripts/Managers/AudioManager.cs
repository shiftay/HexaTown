using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX { SHUFFLE, OFFICESOUNDS, STAMP, COMMUTER, PARTY, RECYCLE, DEAL, CASH, CONSTRUCT, JUSTICE, EXPLODE, COIN }

public class AudioManager : MonoBehaviour {

	public List<AudioClip> clips = new List<AudioClip>();
	static public AudioManager instance;
	public AudioSource sfxSource;
	public AudioSource musicSource;
	public List<AudioClip> BGM = new List<AudioClip>();
	public bool fadeIn = false;
	public bool fadeOut = false;
	public AudioClip clipToUse;
	public float multiplier;

	// Use this for initialization
	void Start () {
		instance = this;
		sfxSource = GetComponent<AudioSource>();
		musicSource.clip = BGM[0];
		musicSource.Play();
		
	}

	public void mute(bool mute) {
		BackEndManager.instance.mutedMusic = mute;
		musicSource.mute = mute;
		sfxSource.mute = mute;
	}

	public void setVolumes(float sfx, float music) {
		musicSource.volume = music;
		sfxSource.volume = sfx;
	}


	void Update() {

		if(!musicSource.mute) {
			if(fadeIn) {
				if(musicSource.volume < BackEndManager.instance.currentVolume) {
					musicSource.volume += Time.deltaTime * multiplier;
				} else {
					fadeIn = false;
				}

			}


			if(fadeOut) {
				if(musicSource.volume > 0) {
					musicSource.volume -= Time.deltaTime * multiplier;
				} else {
					fadeOut = false;
					fadeIn = true;
					musicSource.clip = clipToUse;
					musicSource.Play();
				}
			}
		} 


	}
	
	public void playSound(SFX sound) {
		sfxSource.clip = clips[(int)sound];
		sfxSource.Play();
	}

	public void startFadeO(bool game) {
		fadeOut = true;
		GameMusic();
	}



	public void GameMusic() {
		clipToUse = BGM[1];
		// fadeIn = true;
	}

	public void MenuMusic() {
		fadeOut = true;
		clipToUse = BGM[0];
	}



}