using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour {

	public Text ButtonInfo;
	public Text numberInfo;
	public Image buttonImg;
	public int cardNum;
	public int currentAmt = 0;

	public void pressed() {
		//TODO: Remove from currentDeck.
		//TODO: delete if there is only one.
		Debug.Log("HI");
	}

	public void UpdateAmt(int change) {
		currentAmt += change;

		if(currentAmt > 1) {
			numberInfo.text = currentAmt.ToString();
		}
	}

	public void SetInfo(int cardNum, string name, TILETYPE type) {
		this.cardNum = cardNum;
		currentAmt = 1;
		numberInfo.text = currentAmt.ToString();
		ButtonInfo.text = name;


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
