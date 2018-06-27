﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

	public GameObject[] cards;
	public Vector2[] cardPositions;
	public GridController hexGrid;
	public CardManager cm;

	// Use this for initialization
	void Start () {
		cardPositions = new Vector2[cards.Length];

		for(int i = 0; i < cards.Length; i++) {
			cardPositions[i] = cards[i].transform.position;
		}
	}
	





	// Update is called once per frame
	void Update () {
		
	}


	public bool reorganizeHand(GameObject currentCard, Vector2 droppedPos) {
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
						hexGrid.updateGRID(position, cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite));
						currentCard.transform.position = new Vector2(-100,-100);
						hitTile.sprite = currentCard.GetComponent<SpriteRenderer>().sprite;
						GameManager.instance.playedCard(cm.cardValue(currentCard.GetComponent<SpriteRenderer>().sprite));
					} else {
						
						currentCard.transform.position = cardPositions[indexOfCard(currentCard)];

						retVal = false;
					}



					// touchOffset = (Vector2)hit.transform.position - inputPosition;
					// draggedObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
					// draggedObject.GetComponent<SpriteRenderer>().sprite = cm.changetoTile(draggedObject.GetComponent<SpriteRenderer>().sprite);
				} else if(hit.transform.tag == "SpellArea") {
					// PLAY THE CARD
					Debug.Log("SPELL");
					currentCard.transform.position = new Vector2(-100,-100);
					//TODO : DO TURN ON SPELL MODIFIERS AND SUCH.
					GameManager.instance.playedCard(cm.ValueOfAll(currentCard.GetComponent<SpriteRenderer>().sprite));
				
				} else {
					currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
					retVal = false;
				}
			}
		} else {
			currentCard.transform.position = cardPositions[indexOfCard(currentCard)];
			retVal = false;
		}

		if(!retVal && cm.isSpell(currentCard.GetComponent<SpriteRenderer>().sprite)) {
			cm.spellArea.color = new Color(1,1,1,0); // TODO: FADE?
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
}