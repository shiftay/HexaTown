using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class CollectionManager : MonoBehaviour {

	public GameObject leftBtn;
	public GameObject rightBtn;
	public List<CardData> cardData = new List<CardData>();
	public List<CardData> modifiedList = new List<CardData>();
	string path = "Assets/Resources/cards.txt";
	public SpriteRenderer[] cardPositions;
	public Sprite[] cards;
	public List<CardData> currentSearch = new List<CardData>();
	public List<CardData> cardsShowing = new List<CardData>();

	public int currentPage = 0;

	public int maxPages = 0;

	public bool commuter = false;
	public bool party = false;
	public bool recycle = false;

	public bool res = false;
	public bool comm = false;
	public bool spell = false;

	bool firstRun = true;

	// Use this for initialization
	void Start () {
		if(firstRun) {
			ReadCardData();
			modify();
			firstRun = false;
		}

	}

	void OnEnable()	{
		Start();
		currentPage = 0;

		currentSearch.AddRange(modifiedList);

		maxPages = currentSearch.Count / 6;

		if(currentSearch.Count % maxPages != 0) {
			maxPages++;
		}
		
		updatePage();
	}

	void setupCards() {
		foreach(SpriteRenderer s in cardPositions) {
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
				res = !res;
				break;
			case "COMM":
				comm = !comm;
				break;
			case "SPELLS":
				spell = !spell;
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

		t.GetComponent<Outline>().enabled = !t.GetComponent<Outline>().enabled;

		updateSearch();
	}

	void updateSearch() {



		resetSearch();
		currentPage = 0;

		if(!spell && !comm && !res && !commuter && !party && !recycle) {

		} else {
			List<CardData> temp = new List<CardData>();

			foreach(CardData x in currentSearch) {
				// if(searchParams().Count == 0) {
				// 	if((x.COMMUTE() && commuter) || (x.PARTY() && party) ||	(x.RECYCLE() && recycle)) {
				// 		temp.Add(x);
				// 	}
				// } else if()
				if(res) {
					if(x.TYPE() == TILETYPE.RESIDENTIAL) {

						if(commuter || recycle || party) {
							if((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
							(x.RECYCLE() && recycle)) {
								if(!temp.Contains(x)) {
									temp.Add(x);
								}
							}
						} else {
							if(!temp.Contains(x)) {
								temp.Add(x);
							}
						}

					}
				}

				if(comm) {
					if(x.TYPE() == TILETYPE.COMMERCIAL) {
						if(commuter || recycle || party) {
							if ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
							(x.RECYCLE() && recycle)) {
								if(!temp.Contains(x)) {
									temp.Add(x);
								}
							}
						} else {
							if(!temp.Contains(x)) {
								temp.Add(x);
							}
						}

					} 
				}

				if(spell) {
					if(x.TYPE() == TILETYPE.RESIDENTIAL && ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
					(x.RECYCLE() && recycle))) {
						if(!temp.Contains(x)) {
							temp.Add(x);
						}
					}
				}



				if((commuter || recycle || party) && (searchParams().Count == 0)) {
					if ((x.COMMUTE() && commuter) || (x.PARTY() && party) ||
					(x.RECYCLE() && recycle)) {
						if(!temp.Contains(x)) {
							temp.Add(x);
						}
					}
				}
				// if((x.TYPE() == TILETYPE.COMMERCIAL && comm ) || (x.TYPE() == TILETYPE.RESIDENTIAL && res) || 
				// 	(x.TYPE() == TILETYPE.SPELL && spell ) || (x.COMMUTE() && commuter) || (x.PARTY() && party) ||
				// 	(x.RECYCLE() && recycle)) {
				// 	temp.Add(x);
				// }
			}

			currentSearch.Clear();
			currentSearch.AddRange(temp);
		}

		maxPages = currentSearch.Count / 6;

		if(maxPages == 0 && currentSearch.Count > 0) {
			maxPages++;
		} else if(currentSearch.Count % 6 != 0) {
			maxPages++;
		}

		updatePage();
	}

	List<TILETYPE> searchParams() {
		List<TILETYPE> temp = new List<TILETYPE>();
		if(res) {
			temp.Add(TILETYPE.RESIDENTIAL);
		}

		if(comm) {
			temp.Add(TILETYPE.COMMERCIAL);
		}

		if(spell) {
			temp.Add(TILETYPE.SPELL);
		}

		return temp;
	}



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




	void resetSearch() {
		currentSearch.RemoveRange(0,currentSearch.Count);
		currentSearch.AddRange(modifiedList);
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		// Debug.Log("HI");
	}

	
	int position() {
		return currentPage * 6;
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
		int x = position() + 6;
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




	void ReadCardData() {
		StreamReader sr = new StreamReader(path);
		string line;
		bool flip = false;

		while((line = sr.ReadLine()) != null) {
			string[] split = line.Split('/');

			if(split[0] == "=") {
				flip = true;
			} else if (flip) {
				CardData x = new CardData();	
				x.SetData(int.Parse(split[1]), int.Parse(split[2]), (SPELLTYPE)Enum.Parse(typeof(SPELLTYPE), split[0]), bool.Parse(split[3]), bool.Parse(split[4]), bool.Parse(split[5]), split[6]);
				cardData.Add(x);
			} else {
				CardData x = new CardData();	
				x.SetData(int.Parse(split[1]), int.Parse(split[2]), (TILETYPE)Enum.Parse(typeof(TILETYPE), split[0]), bool.Parse(split[3]), bool.Parse(split[4]), bool.Parse(split[5]), int.Parse(split[6]), split[7]);
				cardData.Add(x);
			}
		}

		sr.Close();
	}


	

}
