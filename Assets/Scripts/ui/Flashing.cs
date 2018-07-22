using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour {

	SpriteRenderer srFlash;
	Image test;
	public bool flash = true;	
	public float dTime;
	public float TIME = 0;
	public float modifier;
	bool dir = false;
	public Color darkest;
	public Color lightest;

	// Use this for initialization
	void Start () {
		if(GetComponent<SpriteRenderer>()) {
			srFlash = GetComponent<SpriteRenderer>();
		} else {
			test = GetComponent<Image>();
		}
		
		TIME = dTime;
	}
	
	// Update is called once per frame
	void Update () {

	

		if(dir) {
			TIME += Time.deltaTime * modifier;
		} else {
			TIME -= Time.deltaTime * modifier;
		}



		if(flash) {
			if(srFlash) {
				srFlash.color = Color.Lerp(lightest, darkest, TIME);
			} else {
				test.color = Color.Lerp(lightest, darkest, TIME);
			}
			
		}

		if(TIME >= 1) {
			dir = false;
		} else if(TIME <= 0) {
			dir = true;
		}



	}
}
