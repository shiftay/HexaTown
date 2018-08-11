using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public enum EVENT_RNG {	PERMITS = 0, RAIN, BEDBUGS, CRIMEWAVE, MONEYTROUBLES, QUAKE, COUNT }

public class CardData {
	public string name;
	TILETYPE type;
	SPELLTYPE spell;
	bool recycle, party, commute, funded;
	int buildTime, tileValue, corruptValue;
	
	public bool PARTY() {
		return party;
	}

	public bool FUNDED() {
		return funded;
	}

	public bool COMMUTE() {
		return commute;
	}

	public bool RECYCLE() {
		return recycle;
	}
	public int BUILD() {
		return buildTime;
	}

	public int TVALUE() {
		return tileValue;
	}

	public int CORRUPT() {
		return corruptValue;
	}

	public TILETYPE TYPE() {
		return type;
	}
	
	public SPELLTYPE sTYPE() {
		return spell;
	}

	public void SetData(int x, int y, TILETYPE t, bool r, bool p, bool com, bool f,  int c, string n) {
		type = t;
		buildTime = x;
		tileValue = y;
		recycle = r;
		corruptValue = c;
		party = p;
		commute = com;
		name = n;
		funded = f;
	}

	public void SetData(int x, int y, SPELLTYPE t, bool r, bool p, bool com, bool f, string n) {
		spell = t;
		buildTime = x;
		tileValue = y;
		recycle = r;
		name = n;
		type = TILETYPE.SPELL;
		party = p;
		commute = com;
		funded = f;
	}
}


public class GameManager : MonoBehaviour {
	int WINCONDITION = 100;
	public Sprite baseTile;
	public Sprite water;
	public static GameManager instance;
	public HandController hc;
	public UIManager um;
	public GridController gc;
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
	string path = "cards.txt";
	bool calculated = false;
	List<TileInfo> residential = new List<TileInfo>();
	bool unhappy = false;
	int turnsSinceEvt = 0, amtofEvts;
	public int prevHappy, prevObject, prevPopo;
	bool commuters;
	public bool COMMUTE {
		set {
			commuters = value;
			buffs.COMMUTE(commuters);
		}

		get {
			return commuters;
		}
	}
	bool party;
	public bool PARTY {
		set {
			party = value;
			buffs.PARTY(party);
		}

		get {
			return party;
		}
	}


	public BuffTracker buffs;
	public List<int> corruptVals;
	public List<int> industryVals;

	public List<int> prevHapp = new List<int>();
	public List<int> prevObjec = new List<int>();
	public List<int> prevPop = new List<int>();
	int commuterTracker = 0;
	int partyTracker = 0;
	public bool gameOver = false;
	public GameObject endgameUI;
	public GameObject pause;
	bool firstRun = true;
	public Animation phone;
	public AnimationClip[] clips;
	public Sprite industryArea;
	bool discardAnim = false;
	public Animation[] discardAnims;
	public Animation deal;
	public DeckManagement drawPile;
	public Animation shuffle;
	public GameObject drawObj;
	public ParticleSystem fireworks;

	bool funding = false;

	public bool FUNDING {
		set {
			funding = value;
			buffs.FUNDING(funding);
		}

		get {
			return funding;
		}
	}
	// Use this for initialization
	void OnEnable () {
		
		gameOver = false;
		pause.SetActive(true);
		if(firstRun) {
			instance = this;
			ReadCardData();
			gc = GetComponent<GridController>();
			um = GetComponent<UIManager>();
			firstRun = false;
		}

		

		if(BackEndManager.instance.resume) {
			ResumeGame();
		} else {
			currentDeck.AddRange(BackEndManager.instance.decks[BackEndManager.instance.deckChoice].cards);
			currentDeck.AddRange(industryVals);
			gc.setupIndustryTiles();
			AddCorruption();
			do {
				Shuffle();
				Deal();
			} while(possiblePlays() < 2);
		}
		// deal.Play();
		drawPile.setCards(currentDeck.Count / 5);
		phone.clip = clips[1];
		phone.Play();
	}

	public void Clear() {
		cardsPlayed = 0;
		currentDeck.Clear();
		activeHAND.Clear();
		currentDiscard.Clear();
		prevHapp.Clear();
		prevObjec.Clear();
		prevPop.Clear();
		populationVal = 0;
		happinessVal = 0;
		objectiveVal = 0;
		currentTurn = 0;
		party = false;
		commuters = false;
		turnOver = false;
		calculated = false;
		discardAnim = false;
		funding = false;
		turnCardPlayed.Clear();
		hc.resetHand();
		currentTiles.Clear();
		gc.resetTiles(baseTile);
		um.TurnStart();
	}

	void AddCorruption() {
		for(int i = 0; i < 4; i ++) {
			currentDeck.Add(corruptVals[UnityEngine.Random.Range(0, corruptVals.Count)]);
		}
	}

	//tilenum TYPE / BUILD / VALUE
	
	// Update is called once per frame
	void Update () {

		if(turnOver && !calculated) {
			//TODO: Discard cards.
			if(!discardAnim) {

				DiscardAnim();
				discardAnim = true;
			}
			

			

			//TODO: Calculate stats + show stats
			//TODO: Decide if random event happens + show event if so.
			//TODO: Deal cards, flip bool.
			

			// CHECK GAMEOVER
		}
	}

	void turnUISetup(){ 
		disableHand();
		um.TurnOVER();
		AdvanceBuilds();	
	}

	void DiscardAnim() {
		List<int> notPlayed = hc.notPlayed();

		for(int i = 0; i < notPlayed.Count; i++) {
			discardAnims[notPlayed[i]].Play();
		}

		Invoke("turnUISetup", 0.5f);
	}

	public void resolveTurn() {
		// CHECK GAME OVER

		if(currentTurn > 3) {
			turnsSinceEvt++;
			if(randomBool((float)(turnsSinceEvt * 0.1))) {
				amtofEvts++;
				um.EventActivate();


				EVENT_RNG temp = (EVENT_RNG)UnityEngine.Random.Range(0, (int)EVENT_RNG.QUAKE);

				if(temp == EVENT_RNG.RAIN && currentTiles.Count > 40) {
					temp = EVENT_RNG.QUAKE;
				} // change this to "earthquake"

				um.evnt.EVTChoice(temp);
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
		gameOverCheck();
	

		if(gameOver) {
			// TODO: TURN ON A GAME OVER UI.
			disableHand();
			endgameUI.SetActive(true);
		} else {
			deal.Play();
			AudioManager.instance.playSound(SFX.DEAL);
			drawPile.setCards(currentDeck.Count / 5);
			Invoke("afterDealt", 0.5f);
		}
	}


	public void afterDealt() {
		cardsPlayed = 0;
		turnCardPlayed.Clear();
		saveGame();
		
		turnOver = false;
		calculated = false;
		discardAnim = false;
	}


	public void gameOverCheck() {
		//TODO: check if they can play their current hand.
		//		check if they beat win condition
		//		check if board is filled
		if(objectiveVal > WINCONDITION) {
			gameOver = true;
			BackEndManager.instance.gameWon = true;
		}

		int amtOfSpells = 0;

		for(int i = 0; i < activeHAND.Count; i++) {
			if(cardData[activeHAND[i]].TYPE() == TILETYPE.SPELL) {
				amtOfSpells++;
			}
		}

		if(currentTiles.Count == 56) {
			if(amtOfSpells < 2) {
				gameOver = true; 
			}
		}



		// int amtofInd = gc.INDX.Count;
		// int factory = 0;
		// for(int i = 0; i < currentTiles.Count; i++) {
		// 	if(currentTiles[i].type == TILETYPE.INDUSTRIAL){
		// 		factory++;
		// 	}
		// }

		// int industryCards = 0;
		// for(int i = 0; i < activeHAND.Count; i++) {
		// 	if(cardData[activeHAND[i]].TYPE() == TILETYPE.INDUSTRIAL) {
		// 		industryCards++;
		// 	}
		// }

		// Industry based loss
		// if(industryCards == 5 && (amtofInd - factory) < 2) {
		// 	gameOver = true;
		// }
		// Industry based loss
		// if((amtofInd - factory) <= 1 && industryCards > 1) {
		// 	if((amtOfSpells + industryCards) == activeHAND.Count && possiblePlays() < 2){
		// 		gameOver = true;
		// 	}
		// }

		if(possiblePlays() < 2) {
			gameOver = true;
		}

		// Debug.Log("POSSIBLE PLAYS: " + possiblePlays());

	}

	int possiblePlays() {
		int possiblePlays = 0;
		int dupCom = 0, dupPart = 0, dupJust = 0, dupBuild = 0, dupRecyc = 0, dupFund = 0;

		int amtofInd = gc.INDX.Count;
		int factory = 0;
		for(int i = 0; i < currentTiles.Count; i++) {
			if(currentTiles[i].type == TILETYPE.INDUSTRIAL){
				factory++;
			}
		}
		int possInd = amtofInd - factory;

		int spotsLeft = 56 - currentTiles.Count;
		int notDemos = 0;
		for(int i = 0; i < currentTiles.Count; i++) {
			if(!currentTiles[i].scheduledDemo) {
				notDemos++;
			}
		}



		for(int i = 0; i < activeHAND.Count; i++) {
			bool flip = false;
			if(cardData[activeHAND[i]].TYPE() == TILETYPE.SPELL) {
				switch(activeHAND[i]) {
					case 32: //commuter
						if(!COMMUTE && dupCom == 0) {
							possiblePlays++;
							Debug.Log("COMMUTER POSSIBLE");
						}

						dupCom++;
						break;

					case 33: // demo?
						if(notDemos > 0) {
							notDemos--;
							possiblePlays++;
							Debug.Log("DEMO POSSIBLE");
						}
						
						break;

					case 34: // funding
						if(!FUNDING && dupFund == 0) {
							possiblePlays++;
							Debug.Log("FUNDING POSSIBLE");
						}

						dupFund++;
						break;

					case 35: // justice
						int corruptTiles = 0;

						for(int x = 0; x < currentTiles.Count; x++) {
							if(currentTiles[x].corruptVal != 0) {
								corruptTiles++;
							}
						}

						if(corruptTiles != 0) {
							if(dupJust > 0) {
								corruptTiles -= dupJust;
								if(corruptTiles != 0) {
									possiblePlays++;
									Debug.Log("justice POSSIBLE");
								}
							} else {
								possiblePlays++;
								Debug.Log("justice POSSIBLE");
							}
						}

						dupJust++;
						break;
					
					case 36: // party
						if(!PARTY && dupPart == 0) {
							possiblePlays++;
							Debug.Log("party POSSIBLE");
						}
						
						dupPart++;
						break;
					
					case 37: // quick build
						int incompleteBuilds = 0;
						for(int x = 0; x < currentTiles.Count; x++) {
							if(currentTiles[x].buildTime != 0 && currentTiles[x].type != TILETYPE.EVENT && !currentTiles[x].scheduledDemo) {
								incompleteBuilds++;
							}
						}

						if(incompleteBuilds != 0) {
							if(dupBuild > 0) {
								incompleteBuilds -= dupBuild;
								if(incompleteBuilds != 0) {
									possiblePlays++;
									Debug.Log("quick build POSSIBLE");
								}
							} else {
								possiblePlays++;
								Debug.Log("quick build POSSIBLE");
							}
						}
						dupBuild++;
						break;

					case 38: // recycle
						if(currentDiscard.Count != 0 && dupRecyc == 0) {
							possiblePlays++;
							Debug.Log("recycle POSSIBLE");
						}
						dupRecyc++;
						break;

					case 39: // rush order
						for(int x = 0; x < currentTiles.Count; x++) {
							if(flip) {
								continue;
							} else if(currentTiles[x].type == TILETYPE.INDUSTRIAL && currentTiles[x].buildTime <= 0) {
								possiblePlays++;
								Debug.Log("rush order POSSIBLE");
								flip = true;
							}
						}
						break;
				}
			}

			if(cardData[activeHAND[i]].TYPE() == TILETYPE.INDUSTRIAL) {
				if(possInd > 0) {
					possInd--;
					possiblePlays++;
					Debug.Log("INDUSTRIAL POSSIBLE");
				}
			}


			if(cardData[activeHAND[i]].TYPE() == TILETYPE.RESIDENTIAL || cardData[activeHAND[i]].TYPE() == TILETYPE.COMMERCIAL) {
				if(spotsLeft > 0) {
					spotsLeft--;
					possiblePlays++;
					Debug.Log("RESIDENTIAL || COMMERCIAL POSSIBLE");
				}
			}

		}

		return possiblePlays;
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
				currentTiles[i].turnOffBuild = true;
			}
		}
	}

	public void playedCard(int cardPlayed) {
		cardsPlayed++;
		turnCardPlayed.Add(cardPlayed);
		if(cardData[cardPlayed].RECYCLE()) {
			if(currentDiscard.Count > 0) {
				TransferDiscard();
				Shuffle();
				shuffle.Play();
				AudioManager.instance.playSound(SFX.RECYCLE);
				drawPile.setCards(currentDeck.Count / 5);
			}
		}

		if(FUNDING) {
			currentTiles[currentTiles.Count-1].funding();
			FUNDING = false;
		}

		if(cardData[cardPlayed].COMMUTE() && !COMMUTE) {
			//TODO: SOUND EFFECT BUS
			AudioManager.instance.playSound(SFX.COMMUTER);
			COMMUTE = true;
			commuterTracker++;
		}

		if(cardData[cardPlayed].PARTY() && !PARTY) {
			//TODO: SPARKS / FIREWORKS
			fireworks.Play();
			AudioManager.instance.playSound(SFX.PARTY);
			PARTY = true;
			partyTracker++;
		}

		if(cardData[cardPlayed].FUNDED() && !FUNDING) {
			// PLAY SOUND
			FUNDING = true;
		}


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
			gameOver = true;
		} else {
			for(int i = 0; i < 5; i++) {
				activeHAND.Add(currentDeck[i]);
			}
			currentDeck.RemoveRange(0, 5);
			hc.setHand(activeHAND);
		}
	}

	void ReadCardData() {
		string filePath = Path.Combine(Application.streamingAssetsPath, path);
		string result = "";

		bool flip = false;
	
		if (Application.platform == RuntimePlatform.Android) {
			WWW reader = new WWW(filePath);

          	while(!reader.isDone) { }

			result = reader.text;
        } else {
            result = System.IO.File.ReadAllText(filePath);
    	}
		string[] split = result.Split('\n');

		for(int i = 0; i < split.Length; i++) {
			string[] temp = split[i].Split('/');

			if(temp[0] == "=") {
				flip = true;
			} else if (flip) {
				CardData x = new CardData();	
				x.SetData(int.Parse(temp[1]), int.Parse(temp[2]), (SPELLTYPE)Enum.Parse(typeof(SPELLTYPE), temp[0]), bool.Parse(temp[3]), bool.Parse(temp[4]), bool.Parse(temp[5]), bool.Parse(temp[6]), temp[7]);
				cardData.Add(x);
			} else {
				CardData x = new CardData();	
				x.SetData(int.Parse(temp[1]), int.Parse(temp[2]), (TILETYPE)Enum.Parse(typeof(TILETYPE), temp[0]), bool.Parse(temp[3]), bool.Parse(temp[4]), bool.Parse(temp[5]), bool.Parse(temp[6]), int.Parse(temp[7]), temp[8]);
				cardData.Add(x);
			}
		}
	}

	public CardData Info(int num) {
		if(num >= 0) {
			return cardData[num];
		} else {
			// FOR EVENT: WATER
			CardData d = new CardData();
			d.SetData(3, -1, TILETYPE.EVENT, false, false, false, false, 0, "Water");
			return d;
		}
	}

	public void CalculateTurn() {
		
		prevPop.Add(populationVal);
		prevHapp.Add(happinessVal);
		prevObjec.Add(objectiveVal);
		prevHappy = happinessVal;
		prevObject = objectiveVal;
		prevPopo = populationVal;


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

				if(currentTiles[i].corruptVal > 0) {
					happinessVal -= currentTiles[i].corruptVal;
				}
			}
		}

		if(PARTY) {
			partyTracker--;
			happinessVal *= 2;

			if(partyTracker == 0) {
				PARTY = false;
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

		if(COMMUTE) {
			commuterTracker--;
			populationVal *= 2;
			if(commuterTracker == 0) {
				COMMUTE = false;
			}
		}

		int tempPop = populationVal;

		for(int i = 0; i < factories.Count; i++) {
			if(tempPop - factories[i].tileValue >= 0 && !flag) {
				tempPop -= factories[i].tileValue;
				objectiveVal += factories[i].buildValue;
			} else {
				if(!flag) {
					remainingPos = i;
					flag = true;
				}
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


	public bool playSpell(int value, TileInfo tile) {
		bool retVal = true;

		switch(cardData[value].TVALUE()) {
			case 20:
				if(COMMUTE) {
					retVal = false;
					hc.warning = 20;
				} else {
					COMMUTE = true;

					if(FUNDING) {
						commuterTracker = 4;
						FUNDING = false;
					} else {
						commuterTracker = 2;
					}

					AudioManager.instance.playSound(SFX.COMMUTER);
				}
				break;

			case 25:
				if(currentDiscard.Count != 0) {
					TransferDiscard();
					Shuffle();
					shuffle.Play();
					drawPile.setCards(currentDeck.Count / 5);
					AudioManager.instance.playSound(SFX.RECYCLE);
				} else {
					retVal = false;
					hc.warning = 25;
				}
				break;

			case 28: // party
				if(PARTY) {
					retVal = false;
					hc.warning = 28;
				} else {
					PARTY = true;

					if(FUNDING) {
						FUNDING = false;
						partyTracker = 4;
					} else {
						partyTracker = 2;
					}

					AudioManager.instance.playSound(SFX.PARTY);
					fireworks.Play();
				}
				break;

			case 32: // rush
				int count = 0;
				for(int i = 0; i < currentTiles.Count; i++) {
					if(currentTiles[i].type == TILETYPE.INDUSTRIAL && currentTiles[i].buildTime <= 0) {
						count++;
					}
				}

				if(count == 0) {
					retVal = false;
					hc.warning = 32;
				} else {


					if(FUNDING)  {

						objectiveVal += (count * 2);
						FUNDING = false;
					} else {
						objectiveVal += count;
					}


					AudioManager.instance.playSound(SFX.CASH);
				}
				break;
			case 39: //funding

				if(FUNDING) {
					retVal = false;
					hc.warning = 39;
				} else if(objectiveVal < 5) {
					retVal = false;
					hc.warning = 38;
				} else {
					FUNDING = true;
					objectiveVal -= 5;
					// PLAY SOUND

				}



				break;	
		

			case 2: // build
				if(tile.buildTime > 0 && tile.type != TILETYPE.EVENT) {
					tile.buildTile();
					AudioManager.instance.playSound(SFX.CONSTRUCT);
				} else {
					retVal = false;
					hc.warning = 2;
				}
				break;

			case -99: // demolish
				if(tile.scheduledDemo) {
					retVal = false;
					hc.warning = -99;
				} else {
					tile.demolish(true);
					AudioManager.instance.playSound(SFX.EXPLODE);
				}
				break;

			case 35: // justice
				if(tile.corruptVal != 0) {
					tile.purify();
					AudioManager.instance.playSound(SFX.JUSTICE);
				} else {
					retVal = false;
					hc.warning = 35;
				}
				break;
			default:
				retVal = false;
				break;
		}

		return retVal;
	}



	public bool firstTile(TILETYPE t) {
		bool retVal = false;
		int count = 0;

		for(int i = 0; i < currentTiles.Count; i++) {
			if(currentTiles[i].type == t) {
				count++;
			}
		}

		if(count == 0) {
			retVal = true;
		}

		return retVal;
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
				gc.gameplayObj[x,y] = -3;
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
				// gc.removeFromGrid(currentTiles[x].gameObject);
				currentTiles[x].crime(true);
				// SWITCH TO CRIMEWAVE
				flag = true;
			}

		} while(!flag);

	}


	public void bedbugs() {
		bool flag = false;

		do{
			int x = UnityEngine.Random.Range(0,currentTiles.Count);

			if(currentTiles[x].type == TILETYPE.RESIDENTIAL) {
				// gc.removeFromGrid(currentTiles[x].gameObject);
				currentTiles[x].bugs(true);
				// SWITCH TO BEDBUGS
				flag = true;
			}

		} while(!flag);

	}


	public void earthquake() {
		bool flag = false;
		int counter = 0;

		do {
			int x = UnityEngine.Random.Range(0,currentTiles.Count);

			if(currentTiles[x].type != TILETYPE.EVENT) {
				currentTiles[x].clearTile();
				counter++;
			}

			if(counter >= 2) {
				flag = true;
			}
		} while(!flag);
	}


	public void economy() {
		objectiveVal -= Mathf.RoundToInt(objectiveVal / 5);
	}

//=====================================EVENTS=============================================
	void saveGame() {
		SavedGame temp = new SavedGame();

		temp.happinessVal = happinessVal;
		temp.objectiveVal = objectiveVal;
		temp.populationVal = populationVal;
		temp.commuter = commuterTracker;
		temp.party = partyTracker;
		temp.prevHappiness.AddRange(prevHapp);
		temp.prevObjective.AddRange(prevObjec);
		temp.prevPopulation.AddRange(prevPop);
		

		for(int i = 0; i < gc.rows; i++) {
			for(int j = 0; j < gc.cols; j++) {
				temp.tileSpace.Add(gc.gameplayObj[i,j]);
				if(gc.gameplayObj[i,j] != -1) {
					if(gc.grid[i,j].GetComponent<TileInfo>()) {
						temp.tileState.Add(gc.grid[i,j].GetComponent<TileInfo>().corruptVal);
						temp.tileState.Add(gc.grid[i,j].GetComponent<TileInfo>().buildTime);
						temp.tileState.Add(gc.grid[i,j].GetComponent<TileInfo>().activeChild());
					}
				} 
			}
		}

		
		temp.currentDiscard.AddRange(currentDiscard);
		temp.currentDeck.AddRange(currentDeck);
		temp.currentHand.AddRange(activeHAND);

		temp.industryX.AddRange(gc.INDX);
		temp.industryY.AddRange(gc.INDY);

		//TODO Save Board State. Tile info.

		BackEndManager.instance.save = temp;
	}


	


	void ResumeGame() {
		currentDeck.AddRange(BackEndManager.instance.sGame.currentDeck);
		currentDiscard.AddRange(BackEndManager.instance.sGame.currentDiscard);
		activeHAND.AddRange(BackEndManager.instance.sGame.currentHand);

		prevHapp.AddRange(BackEndManager.instance.sGame.prevHappiness);
		prevPop.AddRange(BackEndManager.instance.sGame.prevPopulation);
		prevObjec.AddRange(BackEndManager.instance.sGame.prevObjective);

		happinessVal = BackEndManager.instance.sGame.happinessVal;
		populationVal = BackEndManager.instance.sGame.populationVal;
		objectiveVal = BackEndManager.instance.sGame.objectiveVal;
		partyTracker = BackEndManager.instance.sGame.party;
		commuterTracker = BackEndManager.instance.sGame.commuter;

		if(partyTracker > 0) {
			PARTY = true;
		}
		
		if(commuterTracker > 0) {
			COMMUTE = true;
		}

		gc.ResumeGrid(BackEndManager.instance.sGame.tileSpace, BackEndManager.instance.sGame.tileState);


		for(int i = 0; i < BackEndManager.instance.sGame.industryX.Count; i++) {
			gc.SetupCoord(BackEndManager.instance.sGame.industryX[i], BackEndManager.instance.sGame.industryY[i]);
		}

		gc.IndustrySprites();

		hc.setHand(activeHAND);

	}



	public void Pause() {
		phone.clip = clips[0];
		phone.Play();
		Invoke("changestate", 0.4f);
	}

	void changestate() {
		BackEndManager.instance.ChangeState(STATES.OPTIONS);
	}

	public void unPause() {
		phone.clip = clips[1];
		phone.Play();
	}

}
