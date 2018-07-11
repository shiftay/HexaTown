using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour {

	public Sprite[] buttonImgs;
	public Button[] btns;
	public Image[] btnImgs;
	public Text[] btnTxt;


	void OnEnable() {
		BackEndManager.instance.editDeck = false;
		BackEndManager.instance.deckToEdit = -1;

		foreach(Button btn in btns) {
			btn.gameObject.SetActive(false);
		}

		for(int i = 0; i < BackEndManager.instance.decks.Count; i++) {
			btns[i].gameObject.SetActive(true);
			btnImgs[i].sprite = buttonImgs[BackEndManager.instance.decks[i].imageNumber];
			btnTxt[i].text = BackEndManager.instance.decks[i].name;
			int x = i;
			btns[i].onClick.AddListener(delegate { pressed(x); });
		}

		if(BackEndManager.instance.decks.Count < 4) {
			btns[BackEndManager.instance.decks.Count].gameObject.SetActive(true);
			btnImgs[BackEndManager.instance.decks.Count].sprite = buttonImgs[0];
			btnTxt[BackEndManager.instance.decks.Count].text = "Create a Deck";
			btns[BackEndManager.instance.decks.Count].onClick.AddListener(createDeck);
		}

	}
	

	public void pressed(int currentDeck) {
		Debug.Log(currentDeck);
		Debug.Log(BackEndManager.instance.decks[currentDeck].cards.Count);
		BackEndManager.instance.editDeck = true;
		BackEndManager.instance.deckToEdit = currentDeck;
		BackEndManager.instance.ChangeState(STATES.COLLECTION);
	}

	public void createDeck() {
		BackEndManager.instance.editDeck = false;
		BackEndManager.instance.deckToEdit = -1;
		BackEndManager.instance.ChangeState(STATES.COLLECTION);
	}
}
