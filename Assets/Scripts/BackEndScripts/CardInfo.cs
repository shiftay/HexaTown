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
	public List<Sprite> cardColors;

	public void pressed() {
		//TODO: Remove from currentDeck.
		//TODO: delete if there is only one.
		Debug.Log("HI");

		UpdateAmt(-1);



	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		// Vector3 test = Vector3.one;

		GetComponent<RectTransform>().localScale = Vector3.one;
		Debug.Log("width ?? " + transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x);
		// Vector2 newSize = new Vector2(200,50);
		// // Vector2 ourSize = buttonImg.gameObject.GetComponent<RectTransform>().sizeDelta;

		// // newSize.y = ourSize.y * (ourSize.x / transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x);

		// // newSize.x = transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x;
		// Debug.Log("newSize " + newSize);
		// buttonImg.gameObject.GetComponent<RectTransform>().sizeDelta = newSize;
	}

	public void UpdateAmt(int change) {
		currentAmt += change;

		if(currentAmt > 1) {
			numberInfo.text = currentAmt.ToString();
		} 

		if(currentAmt == 1) {
			numberInfo.text = "";
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
		numberInfo.text = "";
		ButtonInfo.text = name;
		cm = c;

		switch(type) {
			case TILETYPE.COMMERCIAL:
				buttonImg.sprite = cardColors[0];
				break;
			case TILETYPE.SPELL:
				buttonImg.sprite = cardColors[2];
				break;
			case TILETYPE.RESIDENTIAL:
				buttonImg.sprite = cardColors[1];
				break;
		}


		
	}

}
