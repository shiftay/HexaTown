using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TUTSTAGE { BOARD, CARDDESC, INDUSTRY, CORRUPTION, PARTY, RECYCLE, TIPS };

public class TutorialManager : MonoBehaviour {

	public GameObject[] tutorialStages;
	public int currentStage;
	static public TutorialManager instance;
	public List<GhostWrites> gw = new List<GhostWrites>();
	public List<Text> ghostWriteArea;
	public List<string> ghostWriteTxt;
	public List<string> corruptionTxt;
	public List<string> partyCommuteTxt;
	public List<string> recycleTxt;
	public List<string> industryTxt;
	public List<string> boardTxt;
	public List<string> fundingTxt;
	public GameObject[] indArrow;
	public int currentString;
	public int currentGW;
	public int counter = 0;
	public GameObject[] BuildArrows;
	public GameObject happyArrow;
	public GameObject workerArrow;
	public GameObject houseArrow;
	public GameObject corruptArrow;
	public GameObject[] modifierArrow;
	public Sprite[] card_PartyCommute;
	public Sprite[] spell_PartyCommute;
	public Sprite[] dirtTiles;
	public Image[] img_dirt;
	public Image centerDirt;
	public GameObject[] incorrectTiles;
	public Sprite correctTile;
	public Image[] img_PnC;
	public GameObject[] recycle_Cards;
	public Sprite tile;
	bool ghostWrite = false;
	bool erase = false;
	float timePassed;
	public float speed;
	bool halfPoint = true;
	bool invoked = false;
	bool firstRun = true;
	public GameObject[] btns;
	public Text test;
	float clickCounter = 0;
	public GameObject skipBtn;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		instance = this;
		gw.Add(new GhostWrites(boardTxt, ghostWriteArea[0]));
		gw.Add(new GhostWrites(ghostWriteTxt, ghostWriteArea[1]));
		gw.Add(new GhostWrites(industryTxt, ghostWriteArea[2]));
		gw.Add(new GhostWrites(corruptionTxt, ghostWriteArea[3]));
		gw.Add(new GhostWrites(partyCommuteTxt, ghostWriteArea[4]));
		gw.Add(new GhostWrites(recycleTxt, ghostWriteArea[5]));
		gw.Add(new GhostWrites(fundingTxt, ghostWriteArea[6]));
	}

	public void changeState(TUTSTAGE stage) {
		tutorialStages[currentStage].SetActive(false);
		currentStage = (int)stage;
		tutorialStages[currentStage].SetActive(true);

		if(stage == TUTSTAGE.PARTY) {
			for(int i = 0; i < img_PnC.Length; i++) {
				img_PnC[i].sprite = spell_PartyCommute[i];
			}
		}
	}

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		firstRun = BackEndManager.instance.firstRun;
		foreach(GameObject stage in tutorialStages) {
			stage.SetActive(false);
		}

		foreach(GameObject button in btns) {
			button.SetActive(true);
		}

		currentGW = 0;
		currentStage = 0;
		currentString = 0;
		counter = 0;

		tutorialStages[currentStage].SetActive(true);


		SetupBtns();
		clear();

	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		timePassed += Time.deltaTime;
		if(erase && !invoked) {
			currentString++;
			if(currentString < gw[currentGW].textToWrite.Count) {


				Invoke("clear", 2.0f);
			} else {
				//TODO: SWITCH TO NEXT PORTION.
				if(firstRun) {
					Invoke("moveStage", 1.5f);
				}
			}
			invoked = true;
			ghostWrite = false;
		} else if(ghostWrite && timePassed > speed) {
			gw[currentGW].text.text = whatToWrite();
			
			if(halfPoint && (counter > (gw[currentGW].textToWrite[currentString].Length / 4))) {
				turnOnOther();
				halfPoint = false;
			}
			timePassed = 0f;
		}

		if(Input.GetMouseButtonDown(0) && firstRun) {
			clickCounter++;
			if(clickCounter > 1) {
				skipBtn.SetActive(true);
			}
		}

	}



	public string whatToWrite() {
		string retVal = "";

		for(int i = 0; i < counter; i++) {
			retVal += gw[currentGW].textToWrite[currentString][i];
		}

		counter++;
		if(counter > gw[currentGW].textToWrite[currentString].Length) {
			erase = true;
		}

		return retVal;
	}

	void moveStage() {
		if(currentStage+1 > (int)TUTSTAGE.RECYCLE){
			BackEndManager.instance.firstRun = false;
			BackEndManager.instance.ChangeState(STATES.MAINMENU);
		} else {
			changeState((TUTSTAGE)currentStage+1);
			currentString = 0;
			currentGW++;
			clear();
		}
	}

	public void skip() {
		if(currentStage == (int)TUTSTAGE.RECYCLE) {
			BackEndManager.instance.ChangeState(STATES.MAINMENU);
			BackEndManager.instance.firstRun = false;
		} else {
			changeState((TUTSTAGE)currentStage + 1);
			clear();
			currentGW = currentStage;
			currentString = 0;
		}

		skipBtn.SetActive(false);
	}

	void turnOnOther() {
		switch(currentGW) {
			case 1: 
				CardDesc();
				break;
			case 3:
				Corruption();
				break;
			case 4:
				PartyCommute();
				break;
			case 2:
				Industry();
				break;
			default:
				break;
		}
	}

	void CardDesc() {
		switch(currentString) {
			case 1:
				foreach(GameObject x in BuildArrows) {
					x.SetActive(true);
				}
				break;
			case 2:
				houseArrow.SetActive(true);
				break;

			case 3:
				happyArrow.SetActive(true);
				break;
			case 4:
				workerArrow.SetActive(true);
				break;
		}
	}


	void Industry() {
		if(currentString == 1) {
			foreach(GameObject ind in indArrow) {
				ind.SetActive(true);
			}
		}
	}

	void Corruption() {
		if(currentString == 2) {
			corruptArrow.SetActive(true);
		}
	}

	void PartyCommute() {
		switch(currentString) {
			case 1:
				modifierArrow[1].SetActive(true);
				break;
			case 2:
				modifierArrow[0].SetActive(true);
				break;
			case 4:
				foreach(GameObject mod in modifierArrow) {
					mod.SetActive(true);
				}
				break;
		}
	}

	public void clear() {
		if(firstRun) {
			foreach(GameObject button in btns) {
				button.SetActive(false);
			}
		}

		ghostWriteArea[currentGW].text = "";
		happyArrow.SetActive(false);
		workerArrow.SetActive(false);
		houseArrow.SetActive(false);
		foreach(GameObject arrow in BuildArrows){
			arrow.SetActive(false);
		}
		corruptArrow.SetActive(false);
		foreach(GameObject mod in modifierArrow) {
			mod.SetActive(false);
		}
		counter = 0;
		erase = false;
		ghostWrite = true;
		invoked = false;
		halfPoint = true;
		foreach(GameObject recycle in recycle_Cards) {
			recycle.SetActive(false);
		}

		foreach(Image i in img_dirt) {
			i.sprite = dirtTiles[0];
		}

		
		foreach(GameObject ind in indArrow) {
			ind.SetActive(true);
		}

		if(currentStage == (int)TUTSTAGE.PARTY && currentString == 4) {
			for(int i = 0; i < img_PnC.Length; i++) {
				img_PnC[i].sprite = card_PartyCommute[i];
			}
		}
		if(currentStage == (int)TUTSTAGE.RECYCLE && currentString == 4) {
			foreach(GameObject recycle in recycle_Cards) {
				recycle.SetActive(true);
			}
		}

		if(currentStage == (int)TUTSTAGE.BOARD && (currentString == 2 || currentString == 3)) {
			foreach(Image i in img_dirt) {
				i.sprite = dirtTiles[1];
			}
		}

		if(currentStage == (int)TUTSTAGE.BOARD && currentString > 3 ) {
			setupBoard();
		}

	}

	public void setupBoard() {
		foreach(Image i in img_dirt) {
			i.sprite = correctTile;
		}

		centerDirt.sprite = tile;

		foreach(GameObject g in incorrectTiles) {
			g.SetActive(true);
		}
	}

	public void change(int nextStage) {
		changeState((TUTSTAGE)currentStage + nextStage);
		SetupBtns();
		clear();
		currentGW = currentStage;
		currentString = 0;
	}

	void SetupBtns() {
		if(currentStage == (int)TUTSTAGE.RECYCLE) {
			btns[1].SetActive(false);
		} else {
			btns[1].SetActive(true);
		}

		if(currentStage == (int)TUTSTAGE.BOARD) {
			btns[0].SetActive(false);
		} else {
			btns[0].SetActive(true);
		}
	}

	public void back() {
		BackEndManager.instance.LeaveCredits();
	}


	

}
