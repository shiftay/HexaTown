using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RNGEvents : MonoBehaviour {

	public Text[] rngText;
	
	// Update is called once per frame
	void Update () {
		
	}

	public void buttonClicked() {
		GameManager.instance.finishTurn();
	}


	public void EVTChoice(EVENT_RNG type) {

		switch(type) {
			case EVENT_RNG.PERMITS:
				rngText[0].text = "PERMITS";
				rngText[1].text = "EXPLANATION TEXT +2 buildTime";
				GameManager.instance.permits();

				break;

			case EVENT_RNG.RAIN:
				rngText[0].text = "RAIN";
				rngText[1].text = "FLOODING";
				GameManager.instance.flooding();
				// TURN ON RAIN;
				break;

			case EVENT_RNG.CRIMEWAVE:
				rngText[0].text = "CRIMEWAVE";
				rngText[1].text = "A few shops are closing up and leaving";
				GameManager.instance.crimeWave();

				// PLAY SOUND
				break;

			case EVENT_RNG.BEDBUGS:
				rngText[0].text = "BEDBUGS";
				rngText[1].text = "Infestation causing citizens to move away";
				GameManager.instance.bedbugs();
				break;
		}

	}











}
