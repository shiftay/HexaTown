using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

	public GameObject[] cards;
	public Vector3[] cardPositions;
	public GridController hexGrid;
	public CardManager cm;
	public int warning;
	// 1- adjacency | 2- 
	// 5- spell on water | spell
	// Use this for initialization
	void Start () {
		cardPositions = new Vector3[cards.Length];

		for(int i = 0; i < cards.Length; i++) {
			cardPositions[i] = cards[i].transform.position;
		}
	}
	





	// Update is called once per frame
	void Update () {
		
	}


	public bool reorganizeHand(GameObject currentCard, Vector2 droppedPos, int value) {
		bool retVal = true;

		RaycastHit2D[] touches = Physics2D.RaycastAll(droppedPos, droppedPos, 0.5f);
		if (touches.Length > 1)
		{
			var hit = touches[1];
			if (hit.transform != null) {
				if(hit.transform.tag == "Hex") {
					
					GameObject position = hit.transform.gameObject;
					SpriteRenderer hitTile = position.GetComponent<SpriteRenderer>();
					
					if(!cm.containsTile(hitTile.sprite) && !cm.isSpell(currentCard.GetComponent<SpriteRenderer>().sprite)) {
						TILETYPE t = GameManager.instance.cardData[cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite)].TYPE();
						if ((t == TILETYPE.INDUSTRIAL && hexGrid.industryCheck(hexGrid.COORDS(position)) || (t != TILETYPE.INDUSTRIAL))) {
							if(hexGrid.placeTile(hexGrid.COORDS(position), t, GameManager.instance.firstTile(t), GameManager.instance.currentTiles.Count == 0)) {					
								hit.transform.gameObject.AddComponent<TileInfo>().SetInfo(hexGrid.updateGRID(position, cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite)), cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite));
								GameManager.instance.currentTiles.Add(hit.transform.gameObject.GetComponent<TileInfo>());
								currentCard.transform.position = new Vector2(-100,-100);
								hitTile.sprite = currentCard.GetComponent<SpriteRenderer>().sprite;
								GameManager.instance.playedCard(cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite));
							} else {
								
								if(hexGrid.surrounded(t) && !GameManager.instance.firstTile(t)) {
									hit.transform.gameObject.AddComponent<TileInfo>().SetInfo(hexGrid.updateGRID(position, cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite)), cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite));
									GameManager.instance.currentTiles.Add(hit.transform.gameObject.GetComponent<TileInfo>());
									currentCard.transform.position = new Vector2(-100,-100);
									hitTile.sprite = currentCard.GetComponent<SpriteRenderer>().sprite;
									GameManager.instance.playedCard(cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite));
								} else {
									currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
									retVal = false;
									warning = 1;
								}
							}
						} else {
							currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
							retVal = false;
							warning = 0;
						}

					} else {
						
						if(cm.spellType(value) == SPELLTYPE.TARGETED) {
							if(cm.containsTile(hitTile.sprite)) {
								
								TileInfo tile = position.GetComponent<TileInfo>();
								
								if(tile) {
									
									if(GameManager.instance.playSpell(value, tile)) {
										currentCard.transform.position = new Vector2(-100,-100);
										//TODO : DO TURN ON SPELL MODIFIERS AND SUCH.
										GameManager.instance.playedCard(value);

									} else {
										currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
										retVal = false;
									}

								} else {
									// NO TILE INFO, SO WATER.
									currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
									retVal = false;
									
								}
							} else {
								currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
								retVal = false;
							}
						} else {
							currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
							retVal = false;
						}
					}
					// touchOffset = (Vector2)hit.transform.position - inputPosition;
					// draggedObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
					// draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoTile(draggedObject.GetComponent<SpriteRenderer>().sprite);
				} else if(hit.transform.tag == "SpellArea") {
					// PLAY THE CARD
					Debug.Log("SPELL");
					
					if(GameManager.instance.playSpell(value, null)) {
						currentCard.transform.position = new Vector2(-100,-100);
						//TODO : DO TURN ON SPELL MODIFIERS AND SUCH.
						GameManager.instance.playedCard(value);
					} else {
						currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
						retVal = false;
					}
					
					cm.spellArea.color = new Color(1,1,1,0); // TODO: FADE?
				
				} else {
					currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
					retVal = false;
					// not placed on anything.
					warning = 5;
				}
			}
		} else {
			currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
			retVal = false;
			warning = 5;
			// not placed on anything.
		}

		if(!retVal && cm.isSpell(currentCard.GetComponent<SpriteRenderer>().sprite)) {
			cm.spellArea.color = new Color(1,1,1,0); // TODO: FADE?
		}

		if(!retVal) {
			setWarningTxt();
		}


		return retVal;
	}


	int indexOfCard(GameObject card) {
		int retVal = -1;

		for(int i = 0; i < cards.Length; i++) {
			if(card == cards[i]) {
				retVal = i;
			}
		}

		return retVal;
	}

	void setWarningTxt() {
		string txt = "";
		switch(warning) {
			case 0: // industry not in area;
				txt = "Must place industry within set area";
				break;
			case 1: // not adjacent
				txt = "Must be adjacent to other tiles";
				break;
			case 20: // commute
				txt = "Commuters is already active";
				break;
			case 25: // recycle
				txt = "Nothing to recycle";
				break;
			case 28: // party
				txt = "Party is already active";
				break;
			case 32: // rush
				txt = "No factories to rush";
				break;
			case 2: // build
				txt = "Building is already built";
				break;
			case -99: // demo
				txt = "Nothing to demolish";
				break;
			case 35: // justic
				txt = "Tile is not corrupt";
				break;
			case 5: //played in a random spot
				txt = "Must be played over the policy change area";
				break;
		}

		GameManager.instance.um.warningTxt.text = txt;
		GameManager.instance.um.fadeIn();
		Invoke("fadeOff", 1.0f);
	}

	void fadeOff() {
		GameManager.instance.um.fadeOut();
	}

	public void setHand(List<int> hand) {
		for(int i = 0; i < hand.Count; i++) {
			cards[i].GetComponent<SpriteRenderer>().sprite = cm.cards[hand[i]];
		}
	}

	public void resetHand() {
		for (int i = 0; i < cards.Length; i++) {
			cards[i].SetActive(true);
			cards[i].transform.position = cardPositions[i];
		}
	}
}
