using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour {

	SpriteRenderer srFlash;

	bool flash = true;	
	public float dTime;
	public float TIME = 0;
	public float modifier;
	bool dir = false;

	// Use this for initialization
	void Start () {
		srFlash = GetComponent<SpriteRenderer>();
		TIME = dTime;
	}
	
	// Update is called once per frame
	void Update () {

		if(srFlash.sprite == null && flash) {
			flash = false;
		}

		if(dir) {
			TIME += Time.deltaTime * modifier;
		} else {
			TIME -= Time.deltaTime * modifier;
		}



		if(flash) {
			srFlash.color = Color.Lerp(Color.white, Color.black, TIME);
		}

		if(TIME >= 1) {
			dir = false;
		} else if(TIME <= 0) {
			dir = true;
		}



	}
}
