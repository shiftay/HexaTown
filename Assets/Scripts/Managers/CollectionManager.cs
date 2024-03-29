﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Networking;

public class CollectionManager : MonoBehaviour {

    public string filePath = "";
    public string result = "";
	public Vector2 xTest;
	public GameObject leftBtn;
	public GameObject rightBtn;
	public List<CardData> cardData = new List<CardData>();
	public Image bookTest;
	public List<CardData> modifiedList = new List<CardData>();
	string path = "cards.txt";
	public Image[] cardPositions;
	public Sprite[] cards;
	public List<CardData> currentSearch = new List<CardData>();
	public List<CardData> cardsShowing = new List<CardData>();
	public List<CardInfo> cardsInDeck = new List<CardInfo>();
	public GameObject scrollContent;
	public GameObject cardPrefab;
	public int currentPage = 0;
	public ScrollRect scrollRect;
	public int maxPages = 0;

	public bool commuter = false;
	public bool party = false;
	public bool recycle = false;

	public bool res = false;
	public bool comm = false;
	public bool spell = false;
	public List<Transform> folders;

	bool firstRun = true;

	public List<int> currentDeck = new List<int>();
	public GameObject popup;
	PopUp popInfo;
	public Text warning;
	int count;
	public GameObject approved;
	public int LIMITEDCARDS;
	public Text deckTracker;
	int currentHappiness = 0;
	int currentPopulation = 0;
	const int DECKMODIFIER = 20;
	public int CURRENTPOP {
		get {
			return currentPopulation;
		}

		set {
			currentPopulation = value;
			popColor();
		}
	}

	public int CURRENTHAPP {
		get {
			return currentHappiness;
		}

		set {
			currentHappiness = value;
			happColor();
		}
	}
	public Text commercial;
	public Text residential;

	// Use this for initialization
	void Start () {
		if(firstRun) {
			// filePath = Path.Combine(Application.streamingAssetsPath, "cards.txt");
			// StartCoroutine(Example());
			ReadCards();
			modify();
			firstRun = false;
			popInfo = popup.GetComponent<PopUp>();
		}
	}


	void Update()
	{
		deckTracker.text = currentDeck.Count.ToString() + " of 20";
		residential.text = currentPopulation.ToString();
		commercial.text = currentHappiness.ToString();
	}

	void OnEnable()	{
		Start();
		currentPage = 0;

		currentSearch.AddRange(modifiedList);

		maxPages = currentSearch.Count / 8;

		if(currentSearch.Count % maxPages != 0) {
			maxPages++;
		}
		
		res = true;
		comm = false;
		spell = false;

		folders[0].SetAsLastSibling();
		currentDeck.Clear();
		cardsInDeck.Clear();


		for(int i = scrollContent.transform.childCount - 1; i >= 0; i--) {
			if(scrollContent.transform.GetChild(i).gameObject != bookTest.gameObject) {
				Destroy(scrollContent.transform.GetChild(i).gameObject);
			}
		}

		

		if(BackEndManager.instance) {
			if(BackEndManager.instance.editDeck) {
				EditDeck();
			}
		}

		warning.text = "";
		warning.gameObject.SetActive(false);
		approved.SetActive(false);
		popup.SetActive(false);
		
		updateSearch();
		updatePage();
	}

	void popColor() {
		if(CURRENTPOP > DECKMODIFIER) {
			residential.color = Color.red;
		} else {
			residential.color = Color.black;
		}
	}
	void happColor() {
		if(CURRENTHAPP > DECKMODIFIER) {
			commercial.color = Color.red;
		} else {
			commercial.color = Color.black;
		}
	}

	void EditDeck() {
		foreach(int val in BackEndManager.instance.decks[BackEndManager.instance.deckToEdit].cards) {

			if(!currentDeck.Contains(val)) {
				GameObject test = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
				cardsInDeck.Add(test.GetComponent<CardInfo>());
				test.GetComponent<CardInfo>().SetInfo(val, cardData[val].name, cardData[val].TYPE(), this);
				test.transform.SetParent(scrollContent.transform);
				currentDeck.Add(val);

				bookTest.transform.SetAsLastSibling();


				
			} else {
				// int pos = -1;
				int amt = 0;
				for (int i = 0; i < currentDeck.Count; i++) {
					if(currentDeck[i] == val) {
						amt++;
					}
				}

				if(amt < LIMITEDCARDS) {
					for(int i = 0; i < cardsInDeck.Count; i++) {
						if(cardsInDeck[i].cardNum == val) {
							cardsInDeck[i].UpdateAmt(1);
							currentDeck.Add(val);
						}
					}
				}
			}

			if(cardData[val].TYPE() == TILETYPE.COMMERCIAL) {
				CURRENTHAPP += cardData[val].TVALUE();
			} else if(cardData[val].TYPE() == TILETYPE.RESIDENTIAL) {
				CURRENTPOP += cardData[val].TVALUE();
			}
					
		}
	}


	void setupCards() {
		foreach(Image s in cardPositions) {
			s.gameObject.SetActive(true);
		}

		if(cardsShowing.Count == cardPositions.Length) {
			for(int i = 0 ; i < cardPositions.Length; i++) {
				cardPositions[i].sprite = cards[cardData.IndexOf(cardsShowing[i])];
			}
		} else {

			for(int i = 0; i < cardsShowing.Count; i++) {
				cardPositions[i].sprite = cards[cardData.IndexOf(cardsShowing[i])];
			}

			for(int i = cardsShowing.Count; i < cardPositions.Length; i++) {
				cardPositions[i].gameObject.SetActive(false);
			}
		}
	}


	public void modify() {
		foreach(CardData x in cardData) {
			if(x.TYPE() != TILETYPE.INDUSTRIAL) {
				modifiedList.Add(x);
			}

			if(x.CORRUPT() != 0) {
				modifiedList.Remove(x);
			}
		}
	}

	public void Commute(GameObject t) {

		commuter = !commuter;
		t.GetComponent<Outline>().enabled = !t.GetComponent<Outline>().enabled;

		updateSearch();
	}

	public void Residential(GameObject t) {
		switch(t.name) {
			case "RES":
				res = true;
				comm = false;
				spell = false;
				folders[0].SetAsLastSibling();
				break;
			case "COMM":
				comm = true;
				res = false;
				spell = false;
				folders[1].SetAsLastSibling();
				break;
			case "SPELLS":
				spell = true;
				comm = false;
				res = false;
				folders[2].SetAsLastSibling();
				break;
			case "Commute":
				commuter = !commuter;
				break;
			case "Party":
				party = !party;
				break;
			case "Recycle":
				recycle = !recycle;
				break;
		}

		//TODO: Check back on this for fix???
		// t.GetComponentInChildren<Outline>().enabled = !t.GetComponentInChildren<Outline>().enabled;

		updateSearch();
	}

	void updateSearch() {
		resetSearch();
		currentPage = 0;
		List<CardData> temp = new List<CardData>();

		foreach(CardData x in currentSearch) {
			if(x.TYPE() == currSearch()) {
				temp.Add(x);
			}
		}

		currentSearch.Clear();
		currentSearch.AddRange(temp);


		maxPages = currentSearch.Count / 8;

		if(maxPages == 0 && currentSearch.Count > 0) {
			maxPages++;
		} else if(currentSearch.Count % 8 != 0) {
			maxPages++;
		}

		updatePage();
	}


	TILETYPE currSearch() {
		TILETYPE retVal;


		if(res) {
			retVal = TILETYPE.RESIDENTIAL;
		} else if(comm) {
			retVal = TILETYPE.COMMERCIAL;
		} else {
			retVal = TILETYPE.SPELL;
		}


		return retVal;
	}

//========
	// void updateSearch222() {
	// 	resetSearch();
	// 	currentPage = 0;

	// 	if(!spell && !comm && !res && !commuter && !party && !recycle) {
	
	// 	} else {
	// 		List<CardData> temp = new List<CardData>();

	// 		foreach(CardData x in currentSearch) {
	// 			// if(searchParams().Count == 0) {
	// 			// 	if((x.COMMUTE() && commuter) || (x.PARTY() && party) ||	(x.RECYCLE() && recycle)) {
	// 			// 		temp.Add(x);
	// 			// 	}
	// 			// } else if()
	// 			if(res) {
	// 				if(x.TYPE() == TILETYPE.RESIDENTIAL) {

	// 					if(commuter || recycle || party) {
	// 						if((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
	// 						(x.RECYCLE() && recycle)) {
	// 							if(!temp.Contains(x)) {
	// 								temp.Add(x);
	// 							}
	// 						}
	// 					} else {
	// 						if(!temp.Contains(x)) {
	// 							temp.Add(x);
	// 						}
	// 					}

	// 				}
	// 			}

	// 			if(comm) {
	// 				if(x.TYPE() == TILETYPE.COMMERCIAL) {
	// 					if(commuter || recycle || party) {
	// 						if ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
	// 						(x.RECYCLE() && recycle)) {
	// 							if(!temp.Contains(x)) {
	// 								temp.Add(x);
	// 							}
	// 						}
	// 					} else {
	// 						if(!temp.Contains(x)) {
	// 							temp.Add(x);
	// 						}
	// 					}

	// 				} 
	// 			}

	// 			if(spell) {
	// 				if(x.TYPE() == TILETYPE.SPELL) {
	// 					if(commuter || recycle || party) {
	// 						if ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
	// 						(x.RECYCLE() && recycle)) {
	// 							if(!temp.Contains(x)) {
	// 								temp.Add(x);
	// 							}
	// 						}
	// 					}else {
	// 						if(!temp.Contains(x)) {
	// 							temp.Add(x);
	// 						}
	// 					}
	// 				}
	// 			}


	// 			if((commuter || recycle || party) && (searchParams().Count == 0)) {
	// 				if ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
	// 				(x.RECYCLE() && recycle)) {
	// 					if(!temp.Contains(x)) {
	// 						temp.Add(x);
	// 					}
	// 				}
	// 			}
	// 			// if((x.TYPE() == TILETYPE.COMMERCIAL && comm ) || (x.TYPE() == TILETYPE.RESIDENTIAL && res) || 
	// 			// 	(x.TYPE() == TILETYPE.SPELL && spell ) || (x.COMMUTE() && commuter) || (x.PARTY() && party) ||
	// 			// 	(x.RECYCLE() && recycle)) {
	// 			// 	temp.Add(x);
	// 			// }
	// 		}

	// 		currentSearch.Clear();
	// 		currentSearch.AddRange(temp);
	// 	}

	// 	maxPages = currentSearch.Count / 8;

	// 	if(maxPages == 0 && currentSearch.Count > 0) {
	// 		maxPages++;
	// 	} else if(currentSearch.Count % 8 != 0) {
	// 		maxPages++;
	// 	}

	// 	updatePage();
	// }

	// List<TILETYPE> searchParams() {
	// 	List<TILETYPE> temp = new List<TILETYPE>();
	// 	if(res) {
	// 		temp.Add(TILETYPE.RESIDENTIAL);
	// 	}

	// 	if(comm) {
	// 		temp.Add(TILETYPE.COMMERCIAL);
	// 	}

	// 	if(spell) {
	// 		temp.Add(TILETYPE.SPELL);
	// 	}

	// 	return temp;
	// }



	/*
		if(res) {
			if(x.TYPE() == TILETYPE.RESIDENTIAL && ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
			(x.RECYCLE() && recycle))) {

			}
		}

		if(comm) {
			if(x.TYPE() == TILETYPE.RESIDENTIAL && ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
			(x.RECYCLE() && recycle))) {

			}
		}

		if(spells) {
			if(x.TYPE() == TILETYPE.RESIDENTIAL && ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
			(x.RECYCLE() && recycle))) {

			}
		}
	
	 */
//========



	void resetSearch() {
		currentSearch.RemoveRange(0,currentSearch.Count);
		currentSearch.AddRange(modifiedList);
	}
	
	int position() {
		return currentPage * 8;
	}

	public void nextPage() {
		if(currentPage != maxPages - 1) {
			currentPage++;
			updatePage();
		}
	}

	public void backPage() {
		if(currentPage != 0) {
			currentPage--;
			updatePage();
		}
	}

	void updatePage() {
		cardsShowing.Clear();
		int x = position() + 8;
		if(currentPage == maxPages - 1 || currentSearch.Count == 0) {
			x = currentSearch.Count;
		}

		for(int i = position(); i < x; i++) {
			cardsShowing.Add(currentSearch[i]);
		}
		setupCards();
		setButtons();
	}

	void setButtons() {
		if(currentPage == 0) {
			leftBtn.SetActive(false);
		} else {
			leftBtn.SetActive(true);
		}

		if(currentPage == maxPages - 1){
			rightBtn.SetActive(false);
		} else {
			rightBtn.SetActive(true);
		}

		if(currentSearch.Count == 0) {
			rightBtn.SetActive(false);
			leftBtn.SetActive(false);
		}
	}

	public string cardInfo;

	void ReadCards() {

		string filePath = Path.Combine(Application.streamingAssetsPath, path);
		string result = "";

		bool flip = false;
	
		// if (Application.platform == RuntimePlatform.Android) {
		// 	WWW reader = new WWW(filePath);

        //   	while(!reader.isDone) { }

		// 	result = reader.text;
        // } else {
        //     result = System.IO.File.ReadAllText(filePath);
    	// }
		string[] split = cardInfo.Split('|');

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

	public void cardPressed(Image hldr) {
		int val = cardNum(hldr);

		if(currentDeck.Count < 20) {

			if(!currentDeck.Contains(val)) {
				GameObject test = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
				cardsInDeck.Add(test.GetComponent<CardInfo>());
				test.GetComponent<CardInfo>().SetInfo(val, cardData[val].name, cardData[val].TYPE(), this);
				

				test.transform.SetParent(scrollContent.transform);
				currentDeck.Add(val);

				bookTest.transform.SetAsLastSibling();
				updateLimits(val);
				
			} else {
				// int pos = -1;
				int amt = 0;
				for (int i = 0; i < currentDeck.Count; i++) {
					if(currentDeck[i] == val) {
						amt++;
					}
				}

				if(amt < LIMITEDCARDS) {
					for(int i = 0; i < cardsInDeck.Count; i++) {
						if(cardsInDeck[i].cardNum == val) {
							cardsInDeck[i].UpdateAmt(1);
							currentDeck.Add(val);
							updateLimits(val);
						}
					}
				}
			}



		}


	}

	void updateLimits(int val) {
		if(cardData[val].TYPE() == TILETYPE.COMMERCIAL) {
			CURRENTHAPP += cardData[val].TVALUE();
		} else if(cardData[val].TYPE() == TILETYPE.RESIDENTIAL) {
			CURRENTPOP += cardData[val].TVALUE();
		}
	}

	public void SaveAndExit() {
		warning.text = "";
		warning.gameObject.SetActive(false);
		warning.color = Color.red;
		warning.fontStyle = FontStyle.Bold;
		warning.fontSize = 12;
		string test = "";

		if(currentDeck.Count == 20 && popInfo.dName != ""  && currentHappiness <= DECKMODIFIER && currentPopulation <= DECKMODIFIER) { // || and the warning is on.
			test = validateDeck();

			if(test == "" || count > 0) {
				Deck temp = new Deck();
				List<int> templist = new List<int>();
				templist.AddRange(currentDeck);

				temp.SetDeck(templist, popInfo.dName, popInfo.currentImage);

				if(BackEndManager.instance.editDeck) {
					BackEndManager.instance.decks[BackEndManager.instance.deckToEdit] = temp;
				} else {
					BackEndManager.instance.decks.Add(temp);
				}

				approved.SetActive(true);
				AudioManager.instance.playSound(SFX.STAMP);

				BackEndManager.instance.ChangeState(STATES.PREGAME);
				
				
			} else {
				count++;

				warning.gameObject.SetActive(true);
				warning.text = test;
				warning.color = Color.gray;
				warning.fontStyle = FontStyle.BoldAndItalic;
				warning.fontSize = 9;
			}
		} else {
			if(currentDeck.Count < 20) {
				test += "Deck needs 20 cards.";
			}

			if(popInfo.dName == "") {
				test += " Deck needs a name.";
			}

			if(currentHappiness > DECKMODIFIER) {
				test += " Commercial over zoning limit.";
			}

			if(currentPopulation > DECKMODIFIER) {
				test += " Residential over zoning limit.";
			}

			warning.gameObject.SetActive(true);
			warning.text = test;
		}
	}


	string validateDeck() {
		string retVal = "";
		int recycle = 0, res = 0, comm = 0, spells = 0;

		for(int i = 0; i < currentDeck.Count; i++) {

			switch(cardData[currentDeck[i]].TYPE() ) {
				case TILETYPE.COMMERCIAL:
					comm++;
					break;
				case TILETYPE.RESIDENTIAL:
					res++;
					break;
				case TILETYPE.SPELL:
					spells++;
					break;
			}

			if(cardData[currentDeck[i]].RECYCLE()) {
				recycle++;
			}
		}


		if(recycle < 2) {
			retVal += "Your Recycle count is a little low.";
		}

		if(comm > res || res > comm) {
			retVal += " Commercial and Residential are a little out of sync.";
		}

		if(spells > (currentDeck.Count * 0.66 )) {
			retVal += " Potentially too many spells.";
		}

		return retVal;
	}

	public void Exit() {
		BackEndManager.instance.ChangeState(STATES.PREGAME);
	}

	public void homeButton() {
		popup.SetActive(true);
	}

	public void back() {
		popup.SetActive(false);
		count = 0;
	}
	public void RemoveInfo(CardInfo ci) {
		currentDeck.Remove(ci.cardNum);
		cardsInDeck.Remove(ci);

		if(cardData[ci.cardNum].TYPE() == TILETYPE.COMMERCIAL) {
			CURRENTHAPP -= cardData[ci.cardNum].TVALUE();
		} else if(cardData[ci.cardNum].TYPE() == TILETYPE.RESIDENTIAL) {
			CURRENTPOP -= cardData[ci.cardNum].TVALUE();
		}

	}

	public void AmtChange(int val) {
		currentDeck.Remove(val);

		if(cardData[val].TYPE() == TILETYPE.COMMERCIAL) {
			CURRENTHAPP -= cardData[val].TVALUE();
		} else if(cardData[val].TYPE() == TILETYPE.RESIDENTIAL) {
			CURRENTPOP -= cardData[val].TVALUE();
		}
	
	}


	int cardNum(Image go) {
		int retVal = -1;

		for(int i = 0; i < cards.Length; i++) {
			if(go.sprite == cards[i]) {
				retVal = i;
			}
		}

		return retVal;
	}
}