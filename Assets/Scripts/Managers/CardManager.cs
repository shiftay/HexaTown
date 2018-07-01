using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SPELLTYPE { TARGETED, ZONE }

public class CardManager : MonoBehaviour {
	public Sprite[] cards;
	public Sprite[] tiles;
	public Sprite[] spells;
	public HandController hc;

	public GameObject spellSpot;
	public SpriteRenderer spellArea;
	// Use this for initialization
	void Start () {
		spellArea = spellSpot.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public Sprite changetoTile(Sprite currCard) {
		int index = -1;

		for(int i = 0; i < cards.Length; i++) {
			if(currCard == cards[i]) {
				index = i;
			}
		}

		return tiles[index];
	}

	public Sprite changetoCard(Sprite currTile, int x = -1) {
		int index = -1;

		if(x != -1) {
			index = x;
		} else {
			for(int i = 0; i < tiles.Length; i++) {
				if(currTile == tiles[i]) {
					index = i;
				}
			}
		}


		return cards[index];
	}

	public bool containsTile(Sprite tile) {
		bool retVal = false;

		for(int i = 0; i < tiles.Length; i++) {
			if(tile == tiles[i]) {
				retVal = true;
			}
		}

		return retVal;
	}


	public int cardValue(Sprite card) {
		int retVal = -1;

		for(int i = 0; i < tiles.Length; i++) {
			if(tiles[i] == card) {
				retVal = i;
			}
		}


		return retVal;
	}

	public Sprite changeToSpell() {
		return spells[0];
	}

	public int ValueOfAll(Sprite card) {
		int retVal = -1;
		
		for(int i = 0; i < cards.Length; i++) {
			if(cards[i] == card) {
				retVal = i;
			}
		}

		return retVal;
	}


	public bool isSpell(Sprite card) {
		bool retVal = true;

		for(int i = 0 ; i < tiles.Length; i++) {
			if(card == cards[i]) {
				retVal = false;
			}
		}

		for(int i = 0; i < tiles.Length; i++) {
			if(card == tiles[i]) {
				retVal = false;
			}
		}


		return retVal;
	}

	public SPELLTYPE spellType(int cardNum) {
		return GameManager.instance.cardData[cardNum].sTYPE();
	}

}
