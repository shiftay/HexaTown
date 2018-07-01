using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public enum EVENT_RNG {	PERMITS, RAIN, ARSON, CRIMEWAVE }

public class CardData {
	TILETYPE type;
	int buildTime, tileValue;

	public int BUILD() {
		return buildTime;
	}

	public int TVALUE() {
		return tileValue;
	}

	public TILETYPE TYPE() {
		return type;
	}

	public void SetData(int x, int y, TILETYPE t) {
		type = t;
		buildTime = x;
		tileValue = y;
	}
}


public class GameManager : MonoBehaviour {

	public Sprite baseTile;
	public Sprite water;
	public static GameManager instance;
	public HandController hc;
	UIManager um;
	GridController gc;
	public int currentHand, currentTurn, objectiveVal, populationVal, happinessVal;
	List<TileInfo> factories = new List<TileInfo>();
	public int cardsPlayed;
	public bool turnOver = false;
	public List<int> currentDeck;
	public List<int> currentDiscard;
	public List<int> activeHAND;
	public List<int> activeSpells; // used to track current player spells for lasting effects.
	public List<int> currentDEBUFFs; // used to track events.
	public List<int> turnCardPlayed;
	public List<CardData> cardData = new List<CardData>();
	public List<TileInfo> currentTiles = new List<TileInfo>();
	string path = "Assets/Resources/cards.txt";
	bool calculated = false;
	List<TileInfo> residential = new List<TileInfo>();
	bool unhappy = false;
	int turnsSinceEvt = 0, amtofEvts;

	List<TileInfo> buildingTiles = new List<TileInfo>();	
	// Use this for initialization
	void Start () {
		instance = this;
		ReadData();
		gc = GetComponent<GridController>();
		um = GetComponent<UIManager>();
		cardsPlayed = 0;
		Shuffle();
		Deal();
	}

	//tilenum TYPE / BUILD / VALUE
	
	// Update is called once per frame
	void Update () {

		if(turnOver && !calculated) {
			//TODO: Discard cards.
			disableHand();
			um.TurnOVER();
			//TODO: Calculate stats + show stats
			//TODO: Decide if random event happens + show event if so.
			//TODO: Deal cards, flip bool.
			Debug.Log("Turn Over");
		}
	}

	public void resolveTurn() {
		if(currentTurn > 3) {
			turnsSinceEvt++;
			if(randomBool((float)(turnsSinceEvt * 0.1))) {
				amtofEvts++;
				um.EventActivate();
				um.evnt.EVTChoice(EVENT_RNG.CRIMEWAVE);
				turnsSinceEvt = 0;
			} else {
				finishTurn();
			}
		} else {
			finishTurn();
		}
	}

	bool randomBool(float modifier) {
		bool retVal = false;

		if(UnityEngine.Random.value < modifier) {
			retVal = true;
		}

		return retVal;
	}


	public void finishTurn() {
		um.TurnStart();
		resetHand();

		cardsPlayed = 0;
		turnCardPlayed.Clear();

		AdvanceBuilds();

		turnOver = false;
		calculated = false;
	}

	// more complex shuffle @ https://stackoverflow.com/questions/273313/randomize-a-listt
	void Shuffle() {
		for (int i = 0; i < currentDeck.Count; i++) {
        	int temp = currentDeck[i];
        	int randomIndex = UnityEngine.Random.Range(i, currentDeck.Count);
        	currentDeck[i] = currentDeck[randomIndex];
        	currentDeck[randomIndex] = temp;
    	}
	}

	void AdvanceBuilds() {
		for(int i = 0; i < currentTiles.Count; i++) {
			if(currentTiles[i].buildTime > 0) {
				currentTiles[i].buildTime--;
			}

			if(currentTiles[i].buildTime <= 0) {
				currentTiles[i].building(false);
			}
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

	void resetHand() {
		for(int i = 0; i < activeHAND.Count; i++) {
			currentDiscard.Add(activeHAND[i]);
		}

		activeHAND.Clear();

		hc.resetHand();

		Deal();
	}

	void TransferDiscard() {
		currentDeck.AddRange(currentDiscard);
		currentDiscard.Clear();
	}

	void Deal() {
		if(currentDeck.Count == 0) {
			TransferDiscard();
			Shuffle();
		}


		for(int i = 0; i < 5; i++) {
			activeHAND.Add(currentDeck[i]);
		}
		currentDeck.RemoveRange(0, 5);
		hc.setHand(activeHAND);
	}

	void ReadData() {
		StreamReader sr = new StreamReader(path);
		string line;

		while((line = sr.ReadLine()) != null) {
			string[] split = line.Split('/');

			CardData x = new CardData();	
			TILETYPE temp = (TILETYPE)Enum.Parse(typeof(TILETYPE), split[0]);	
			x.SetData(int.Parse(split[1]), int.Parse(split[2]), (TILETYPE)Enum.Parse(typeof(TILETYPE), split[0]));

			cardData.Add(x);
		}

		sr.Close();
	}

	public CardData Info(int num) {
		if(num >= 0) {
			return cardData[num];
		} else {
			// FOR EVENT: WATER
			CardData d = new CardData();
			d.SetData(3, -1,TILETYPE.EVENT);
			return d;
		}
	}

	public void CalculateTurn() {
		happinessVal = 0;
		populationVal = 0;

		int remainingPos = -1;
		bool flag = false;

		for (int i = 0; i < currentTiles.Count; i++) {
			if(currentTiles[i].buildTime <= 0) {
				switch (currentTiles[i].type)
				{
					case TILETYPE.COMMERCIAL:
						happinessVal += currentTiles[i].tileValue;
						break;

					case TILETYPE.INDUSTRIAL:
						factories.Add(currentTiles[i]);
						break;
					
					case TILETYPE.RESIDENTIAL:
						populationVal += currentTiles[i].tileValue;
						residential.Add(currentTiles[i]);
						break;
				}
			}
		}

		if((populationVal * 0.75) > happinessVal && currentTurn > 3) {
			//UNHAPPY PEOPLES.
			foreach(TileInfo info in residential) {
				info.unhappy(true);
			}

			populationVal /= 2;
		} else {
			foreach(TileInfo info in residential) {
				info.unhappy(false);
			}
		}


		int tempPop = populationVal;

		for(int i = 0; i < factories.Count; i++) {
			if(tempPop - factories[i].tileValue >= 0 && !flag) {
				tempPop -= factories[i].tileValue;
				objectiveVal++;
			} else {
				remainingPos = i;
				flag = true;
			}
		}

		foreach (TileInfo item in factories) {
			item.workersNeeded(false);
		}

		if(flag) {
			//show the factories not working.
			for(int i = 0; i < factories.Count; i++) {
				if(i >= remainingPos){
					factories[i].workersNeeded(true);
				} else {
					factories[i].workersNeeded(false);
				}
			}
		}
		calculated = true;

		factories.Clear();
		residential.Clear();
		currentTurn++;
	}

//=====================================EVENTS=============================================

	public void permits() {
		foreach (TileInfo info in currentTiles) {
			if(info.buildTime > 0 && info.type != TILETYPE.EVENT) {
				info.buildTime += 2;
			}
		}
	}


	public void flooding() {
		bool flag = false;
		int amtChanged = 0;
		List<int> coords = new List<int>();
		int prevX = -1, prevY = -1;
		do{
			int x = UnityEngine.Random.Range(0,6);
			int y = UnityEngine.Random.Range(0,9);

			if(gc.gameplayObj[x,y] == -1 && prevX != x && prevY != y) {
				coords.Add(x);
				coords.Add(y);
				gc.grid[x,y].AddComponent<TileInfo>().SetInfo(coords, -1);
				gc.grid[x,y].GetComponent<SpriteRenderer>().sprite = water;
				GameManager.instance.currentTiles.Add(gc.grid[x,y].GetComponent<TileInfo>());
				amtChanged++;
				prevX = x;
				prevY = y;
			}

			if(amtChanged >= 2) {
				flag = true;
			}
			coords.Clear();
		} while(!flag);

	}


	public void crimeWave() {
		bool flag = false;

		do{
			int x = UnityEngine.Random.Range(0,currentTiles.Count);

			if(currentTiles[x].type == TILETYPE.COMMERCIAL) {
				gc.removeFromGrid(currentTiles[x].gameObject);
				currentTiles[x].clearTile();
				flag = true;
			}

		} while(!flag);

	}







//=====================================EVENTS=============================================
}
