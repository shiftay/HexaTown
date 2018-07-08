﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour {

	public Text ButtonInfo;
	public Text numberInfo;
	public Image buttonImg;
	public int cardNum;
	public int currentAmt = 0;
	CollectionManager cm;

	public void pressed() {
		//TODO: Remove from currentDeck.
		//TODO: delete if there is only one.
		Debug.Log("HI");

		UpdateAmt(-1);



	}

	public void UpdateAmt(int change) {
		currentAmt += change;

		if(currentAmt >= 1) {
			numberInfo.text = currentAmt.ToString();
		} 
		
		if(currentAmt <= 0) {
			cm.RemoveInfo(this);
			Destroy(gameObject);

		} else if(change < 0) {
			cm.AmtChange(cardNum);
		}
	}

	public void SetInfo(int cardNum, string name, TILETYPE type, CollectionManager c) {
		this.cardNum = cardNum;
		currentAmt = 1;
		numberInfo.text = currentAmt.ToString();
		ButtonInfo.text = name;
		cm = c;

		switch(type) {
			case TILETYPE.COMMERCIAL:
				buttonImg.color = Color.grey;
				break;
			case TILETYPE.SPELL:
				buttonImg.color = Color.blue;
				break;
			case TILETYPE.RESIDENTIAL:
				buttonImg.color = Color.green;
				break;
		}
	}

}