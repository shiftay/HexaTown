using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CollectionManager : MonoBehaviour {

	public List<CardData> cardData = new List<CardData>();
	public List<CardData> modifiedList = new List<CardData>();
	string path = "Assets/Resources/cards.txt";
	public SpriteRenderer[] cardPositions;
	public Sprite[] cards;
	public List<CardData> currentSearch = new List<CardData>();
	public List<CardData> cardsShowing = new List<CardData>();

	public int currentPage = 0;

	public int maxPages = 0;



	// Use this for initialization
	void Start () {
		ReadCardData();
		modify();
	}

	void OnEnable()	{
		Start();
		currentPage = 0;

		currentSearch.AddRange(modifiedList);

		maxPages = currentSearch.Count / 6;

		if(currentSearch.Count % maxPages != 0) {
			maxPages++;
		}
		
		for(int i = position(); i < position() + 6; i++) {
			cardsShowing.Add(currentSearch[i]);
		}

		setupCards();
	}

	void setupCards() {
		if(cardsShowing.Count == cardPositions.Length) {
			foreach(SpriteRenderer s in cardPositions) {
				s.gameObject.SetActive(true);
			}

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
		if(currentPage == maxPages - 1) {
			x = currentSearch.Count;
		}

		for(int i = position(); i < x; i++) {
			cardsShowing.Add(currentSearch[i]);
		}
		setupCards();
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
				x.SetData(int.Parse(split[1]), int.Parse(split[2]), (SPELLTYPE)Enum.Parse(typeof(SPELLTYPE), split[0]), bool.Parse(split[3]), split[4]);
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
