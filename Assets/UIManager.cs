using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject mainUI;
	public GameObject TurnOVERUI;
	public CardManager cm;
	public TurnOVER over;
	
	// Update is called once per frame
	void Update () {
		
	}


	public void TurnOVER() {
		mainUI.SetActive(false);
		TurnOVERUI.SetActive(true);

		for(int i = 0; i < over.cardsPlayed.Length; i++) {
			over.cardsPlayed[i].sprite = cm.cards[GameManager.instance.turnCardPlayed[i]];
		}

		GameManager.instance.CalculateTurn();

		over.summaryInfo[0].text = "Workers: " + GameManager.instance.populationVal.ToString();
		over.summaryInfo[1].text = "Happiness: " + GameManager.instance.happinessVal.ToString();
		over.summaryInfo[2].text = "Objective: " + GameManager.instance.objectiveVal.ToString();

	}

	public void TurnStart() {
		mainUI.SetActive(true);
		TurnOVERUI.SetActive(false);
	}

}
