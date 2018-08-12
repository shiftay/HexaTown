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
		
	}
	
	public void playSound(SFX sound) {
		sfxSource.clip = clips[(int)sound];
		sfxSource.Play();
	}

	public void startFadeO(bool game) {
		Debug.Log(musicSource.volume);

		StartCoroutine(fadeOut(1.0f));
		if(game) {
			Invoke("GameMusic", 1.0f);
		}
	}

	IEnumerator fadeOut(float fadeTime) {
		float startVolume = BackEndManager.instance.currentVolume;

		while(musicSource.volume > 0) {
			float vol = startVolume * Time.deltaTime / fadeTime;
			musicSource.volume -= vol;

			yield return null;
		}
	}

	public void GameMusic() {
		musicSource.clip = BGM[1];
		
		StartCoroutine(fadeIn(1f));
		Invoke("clearCoroutines", 1f);
	}

	public void MenuMusic() {
		musicSource.clip = BGM[0];
		
		StartCoroutine(fadeIn(1f));
		Invoke("clearCoroutines", 1f);
	}


	void clearCoroutines() {
		StopAllCoroutines();
		musicSource.volume = BackEndManager.instance.currentVolume;
	}


	IEnumerator fadeIn(float fadeTime) {
        musicSource.volume = 0f;
        musicSource.Play();
 
        while (musicSource.volume < BackEndManager.instance.currentVolume)
        {
            musicSource.volume += Time.deltaTime * fadeTime;
 
            yield return null;
        }
 
        musicSource.volume = BackEndManager.instance.currentVolume;
	}

}