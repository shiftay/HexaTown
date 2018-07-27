using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SPELLTYPE { TARGETED, ZONE }

public class CardManager : MonoBehaviour {
	public Sprite[] cards;
	public Sprite[] tiles;
	public Sprite[] spells;
	public HandController hc;
	public SpriteRenderer description;
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

	public Sprite changeToSpell(int val) {
		Sprite temp;

		switch(val) {
			case 29: //demo
				temp = spells[2];
				break;
			case 30: //justice
				temp = spells[0];
				break;
			case 32: // build
				temp = spells[1];
				break;
			default:
				temp = new Sprite();
				break;
		}


		return temp;
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

	public void CardDescription(bool toggle, int val = -1) {
		if(toggle) {
			description.color = new Color(1,1,1,1);
			description.sprite = cards[val];
		} else {
			description.color = new Color(1,1,1,0);
		}

	}

}
