using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public int currentHand, currentTurn, objectiveVal, populationVal;
	public int cardsPlayed;
	public bool turnOver = false;
	public List<int> currentDeck;
	public List<int> currentDiscard;
	public List<int> activeSpells; // used to track current player spells for lasting effects.
	public List<int> currentDEBUFFs; // used to track events.




	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
