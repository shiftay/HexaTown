using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RNGEvents : MonoBehaviour {

	public Text[] rngText;
	public Text dayNum;
	

	void OnEnable()
	{
		dayNum.text = (GameManager.instance.currentTurn + 1).ToString();
	}

	public void buttonClicked() {
		GameManager.instance.finishTurn();
	}


	public void EVTChoice(EVENT_RNG type) {

		switch(type) {
			case EVENT_RNG.PERMITS:
				rngText[0].text = "Forgotten Permits";
				rngText[1].text = "Government extending all building times in the area.";
				GameManager.instance.permits();

				break;

			case EVENT_RNG.RAIN:
				rngText[0].text = "Heavy Rains";
				rngText[1].text = "Citizens be wary, flooding spotted in the area.";
				GameManager.instance.flooding();
				// TURN ON RAIN;
				break;

			case EVENT_RNG.CRIMEWAVE:
				rngText[0].text = "Crime Wave";
				rngText[1].text = "Shops closing up amid all the crime and chaos.";
				GameManager.instance.crimeWave();

				// PLAY SOUND
				break;

			case EVENT_RNG.BEDBUGS:
				rngText[0].text = "Bed Bugs";
				rngText[1].text = "Infestation causing citizens to move away";
				GameManager.instance.bedbugs();
				break;
		}

	}











}
