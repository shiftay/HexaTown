using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Sprite[] arrows;
	public Color[] colors;
	public GameObject mainUI;
	public GameObject TurnOVERUI;
	public GameObject EventUI;
	public RNGEvents evnt;
	public CardManager cm;
	public TurnOVER over;
	public List<string> titles;
	public Sprite[] newsPaperImgs;
	public LineRenderer popGraph;
	public LineRenderer happGraph;
	public LineRenderer objectGraph;

	public Text warningTxt;
	public Animation warningAnim;
	public AnimationClip[] warningClips;

	public void fadeIn() {
		warningAnim.clip = warningClips[0];
		warningAnim.Play();
	}

	public void fadeOut() {
		CancelInvoke();
		warningAnim.clip = warningClips[1];
		warningAnim.Play();
		Invoke("resetTxt", 0.75f);
	}

	public void resetTxt() {
		warningTxt.text = "";
	}


	public void TurnOVER() {
		// mainUI.SetActive(false);
		TurnOVERUI.SetActive(true);
		EventUI.SetActive(false);
		for(int i = 0; i < over.cardsPlayed.Length; i++) {
			over.cardsPlayed[i].sprite = newsPaperImgs[GameManager.instance.turnCardPlayed[i]];
			over.imageTitles[i].text = ImgTitle(GameManager.instance.turnCardPlayed[i]);
		}

		GameManager.instance.CalculateTurn();
		// popGraph.positionCount = GameManager.instance.prevPop.Count;
		// popGraph.SetPositions(graphPositions(GameManager.instance.prevPop));


		over.summaryInfo[0].text = GameManager.instance.populationVal.ToString();
		over.summaryInfo[1].text = GameManager.instance.happinessVal.ToString();
		over.summaryInfo[2].text = GameManager.instance.objectiveVal.ToString();

		over.tMods[0].text = (GameManager.instance.populationVal - GameManager.instance.prevPopo).ToString();
		over.modifiers[0].sprite = arrows[ArrowMod(GameManager.instance.populationVal, GameManager.instance.prevPopo)];
		over.modifiers[0].color = colors[ArrowMod(GameManager.instance.populationVal, GameManager.instance.prevPopo)];

		over.tMods[1].text = (GameManager.instance.happinessVal - GameManager.instance.prevHappy).ToString();
		over.modifiers[1].sprite = arrows[ArrowMod(GameManager.instance.happinessVal, GameManager.instance.prevHappy)];
		over.modifiers[1].color = colors[ArrowMod(GameManager.instance.happinessVal, GameManager.instance.prevHappy)];

		over.tMods[2].text = (GameManager.instance.objectiveVal - GameManager.instance.prevObject).ToString();
		over.modifiers[2].sprite = arrows[ArrowMod(GameManager.instance.objectiveVal, GameManager.instance.prevObject)];
		over.modifiers[2].color = colors[ArrowMod(GameManager.instance.objectiveVal, GameManager.instance.prevObject)];

		over.title.text = titles[titleChoice()];

	}

	Vector3[] graphPositions(List<int> positions) {
		Vector3[] retVal = new Vector3[positions.Count];

		for(int i = 0; i < positions.Count; i++) {
			Vector3 temp = new Vector3();

			temp.x = i * 2;
			temp.y = positions[i];
			temp.z = 0;
			
			retVal[i] = temp;
		}

		return retVal;
	}


	public void EventActivate() {
		// mainUI.SetActive(false);
		EventUI.SetActive(true);
		TurnOVERUI.SetActive(false);
	}

	public void TurnStart() {
		// mainUI.SetActive(true);
		EventUI.SetActive(false);
		TurnOVERUI.SetActive(false);
	}


	public int ArrowMod(int x, int y) {
		int retVal = -1;

		if(x > y) {
			retVal = 0;
		} else if (x < y) {
			retVal = 1;
		} else {
			retVal = 2;
		}



		return retVal;
	}



	int titleChoice() {
		int retVal = -1;

		if(GameManager.instance.currentTurn < 3) {
			retVal = 0;
		}


		while(retVal == -1){
			switch(Random.Range(0,3)) {
				case 0:
					if(GameManager.instance.populationVal > GameManager.instance.prevPopo) {
						retVal = 5;
					}
					break;
				
				case 1:
					if (GameManager.instance.objectiveVal > GameManager.instance.prevObject ) {
						retVal = 1;
					} else if ( GameManager.instance.objectiveVal == GameManager.instance.prevObject) {
						retVal = 3;
					}

					break;

				case 2:
					if (GameManager.instance.happinessVal < GameManager.instance.populationVal) {
						retVal = 4;
					} else if (GameManager.instance.happinessVal > GameManager.instance.populationVal) {
						retVal = 3;
					}
					break;
			}
		}

		return retVal;
	}


	string ImgTitle(int num) {
		string retVal = "";

		switch(GameManager.instance.cardData[num].TYPE()) {
			case TILETYPE.COMMERCIAL:
				retVal = "Attractions opening up.";
				break;

			case TILETYPE.RESIDENTIAL:
				retVal = "Neighbours moving in.";
				break;

			case TILETYPE.INDUSTRIAL:
				retVal = "More Jobs coming.";
				break;
		}



		return retVal;
	}

}
