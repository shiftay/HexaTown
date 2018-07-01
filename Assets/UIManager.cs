using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Sprite[] arrows;
	public Color[] colors;
	public GameObject mainUI;
	public GameObject TurnOVERUI;
	public GameObject EventUI;
	public RNGEvents evnt;
	public CardManager cm;
	public TurnOVER over;
	
	// Update is called once per frame
	void Update () {
		
	}


	public void TurnOVER() {
		mainUI.SetActive(false);
		TurnOVERUI.SetActive(true);
		EventUI.SetActive(false);
		for(int i = 0; i < over.cardsPlayed.Length; i++) {
			over.cardsPlayed[i].sprite = cm.cards[GameManager.instance.turnCardPlayed[i]];
		}

		GameManager.instance.CalculateTurn();

		over.summaryInfo[0].text = "Workers: " + GameManager.instance.populationVal.ToString();
		over.summaryInfo[1].text = "Happiness: " + GameManager.instance.happinessVal.ToString();
		over.summaryInfo[2].text = "Objective: " + GameManager.instance.objectiveVal.ToString();

		over.tMods[0].text = (GameManager.instance.populationVal - GameManager.instance.prevPop).ToString();
		over.modifiers[0].sprite = arrows[ArrowMod(GameManager.instance.populationVal, GameManager.instance.prevPop)];
		over.modifiers[0].color = colors[ArrowMod(GameManager.instance.populationVal, GameManager.instance.prevPop)];

		over.tMods[1].text = (GameManager.instance.happinessVal - GameManager.instance.prevHapp).ToString();
		over.modifiers[1].sprite = arrows[ArrowMod(GameManager.instance.happinessVal, GameManager.instance.prevHapp)];
		over.modifiers[1].color = colors[ArrowMod(GameManager.instance.happinessVal, GameManager.instance.prevHapp)];

		over.tMods[2].text = (GameManager.instance.objectiveVal - GameManager.instance.prevObjec).ToString();
		over.modifiers[2].sprite = arrows[ArrowMod(GameManager.instance.objectiveVal, GameManager.instance.prevObjec)];
		over.modifiers[2].color = colors[ArrowMod(GameManager.instance.objectiveVal, GameManager.instance.prevObjec)];
	}


	public void EventActivate() {
		mainUI.SetActive(false);
		EventUI.SetActive(true);
		TurnOVERUI.SetActive(false);
	}

	public void TurnStart() {
		mainUI.SetActive(true);
		EventUI.SetActive(false);
		TurnOVERUI.SetActive(false);
	}


	public int ArrowMod(int x, int y) {
		int retVal = -1;

		if(x > y) {
			retVal = 0;
		} else if (x < y) {
			retVal = 1;
		} else {
			retVal = 2;
		}



		return retVal;
	}

}
