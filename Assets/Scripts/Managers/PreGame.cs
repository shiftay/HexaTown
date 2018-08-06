using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour {

	public Sprite[] buttonImgs;
	public Button[] btns;
	public Outline[] btnOutLines;
	public Image[] btnImgs;
	public Text[] btnTxt;
	public Button[] delBtns;
	public Button[] editBtns;
	public PopUp po;
	public int currentSelected = -1;

	void OnEnable() {
		BackEndManager.instance.editDeck = false;
		BackEndManager.instance.resume = false;
		BackEndManager.instance.deckToEdit = -1;

		if(currentSelected != -1) {
			editBtns[currentSelected].gameObject.SetActive(false);
			delBtns[currentSelected].gameObject.SetActive(false);
			btnOutLines[currentSelected].enabled = false;
		}

		foreach(Button edit in editBtns) {
			edit.onClick.RemoveAllListeners();
		}

		foreach(Button del in delBtns) {
			del.onClick.RemoveAllListeners();
		}

		currentSelected = -1;

		SetupBtns();
	}
	

	public void pressed(int currentDeck) {


		if(currentSelected != currentDeck) {
			if(currentSelected != -1) {
				editBtns[currentSelected].gameObject.SetActive(false);
				delBtns[currentSelected].gameObject.SetActive(false);
				btnOutLines[currentSelected].enabled = false;
			}

			editBtns[currentDeck].gameObject.SetActive(true);
			editBtns[currentDeck].onClick.AddListener(delegate {Edit(currentDeck);});

			delBtns[currentDeck].gameObject.SetActive(true);
			delBtns[currentDeck].onClick.AddListener(delegate {Delete(currentDeck);});
			
			btnOutLines[currentDeck].enabled = true;

			currentSelected = currentDeck;
		} else {
			BackEndManager.instance.sGame = null;
			BackEndManager.instance.deckChoice = currentSelected;
			BackEndManager.instance.ChangeState(STATES.GAME);
		}

	}

	public void createDeck() {
		BackEndManager.instance.editDeck = false;
		BackEndManager.instance.deckToEdit = -1;
		BackEndManager.instance.ChangeState(STATES.COLLECTION);
	}

	public void Edit(int currentDeck) {
		BackEndManager.instance.editDeck = true;
		BackEndManager.instance.deckToEdit = currentDeck;
		BackEndManager.instance.ChangeState(STATES.COLLECTION);
	}

	public void Delete(int currentDeck) {
		BackEndManager.instance.decks.Remove(BackEndManager.instance.decks[currentDeck]);
		editBtns[currentSelected].gameObject.SetActive(false);
		delBtns[currentSelected].gameObject.SetActive(false);
		btnOutLines[currentSelected].enabled = false;
		currentSelected = -1;
		SetupBtns();
	}

	void SetupBtns(){ 
		foreach(Button btn in btns) {
			btn.gameObject.SetActive(false);
			btn.onClick.RemoveAllListeners();
		}

		for(int i = 0; i < BackEndManager.instance.decks.Count; i++) {
			btns[i].gameObject.SetActive(true);
			btnImgs[i].sprite = po.stockImages[BackEndManager.instance.decks[i].imageNumber];
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

}
