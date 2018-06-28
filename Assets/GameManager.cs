using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public HandController hc;
	UIManager um;
	GridController gc;
	public int currentHand, currentTurn, objectiveVal, populationVal;
	public int cardsPlayed;
	public bool turnOver = false;
	public List<int> currentDeck;
	public List<int> currentDiscard;
	public List<int> activeHAND;
	public List<int> activeSpells; // used to track current player spells for lasting effects.
	public List<int> currentDEBUFFs; // used to track events.
	public List<int> turnCardPlayed;
	// Use this for initialization
	void Start () {
		instance = this;
		gc = GetComponent<GridController>();
		um = GetComponent<UIManager>();
		cardsPlayed = 0;
		Shuffle();
		Deal();
	}
	
	// Update is called once per frame
	void Update () {
	

		if(turnOver) {
			//TODO: Discard cards.
			disableHand();
			um.TurnOVER();
			//TODO: Calculate stats + show stats
			//TODO: Decide if random event happens + show event if so.
			//TODO: Deal cards, flip bool.
			Debug.Log("Turn Over");
		}
	}





	// more complex shuffle @ https://stackoverflow.com/questions/273313/randomize-a-listt
	void Shuffle() {
		for (int i = 0; i < currentDeck.Count; i++) {
        	int temp = currentDeck[i];
        	int randomIndex = Random.Range(i, currentDeck.Count);
        	currentDeck[i] = currentDeck[randomIndex];
        	currentDeck[randomIndex] = temp;
    	}
	}


	public void playedCard(int cardPlayed) {
		cardsPlayed++;
		turnCardPlayed.Add(cardPlayed);
		if(cardsPlayed >= 2) {
			turnOver = true;
		}
	}


	void disableHand() {
		foreach (GameObject item in hc.cards)
		{
			item.SetActive(false);
		}
	}

	void Deal() {
		for(int i = 0; i < 5; i++) {
			activeHAND.Add(currentDeck[i]);
			
		}
		currentDeck.RemoveRange(0, 5);
		hc.setHand(activeHAND);
	}

}
