using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX { SHUFFLE, OFFICESOUNDS }

public class AudioManager : MonoBehaviour {

	public List<AudioClip> clips = new List<AudioClip>();
	static public AudioManager instance;
	AudioSource sfxSource;
	public AudioSource musicSource;

	// Use this for initialization
	void Start () {
		instance = this;
		sfxSource = GetComponent<AudioSource>();


		
	}
	

	public void playSound(SFX sound) {
		sfxSource.clip = clips[(int)sound];
		sfxSource.Play();
	}


}
