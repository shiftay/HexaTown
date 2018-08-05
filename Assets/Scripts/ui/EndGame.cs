using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public LineRenderer happy;
	public LineRenderer objective;
	public LineRenderer population;

	public Text title;
	public Text turnNumbers;

	public GameObject[] infoBlocks;
	public int currentChoice = 0;

	public Text[] information;
	public Outline[] btnOutlines;
	GameManager gm;
	public GameObject[] folders;

	void OnEnable()
	{
		if(BackEndManager.instance.gameWon) {
			title.text = "Successful Planning!";
			//TODO ANIMATIONS/ FIREWORKS.
			//		PLAY A SOUND
		} else {
			title.text = "Unsuccessful...";
		}

		gm = GameManager.instance;
		currentChoice = 0;
		SetInfoBlock();
	}


	void SetInfoBlock() {
		happy.gameObject.SetActive(false);
		population.gameObject.SetActive(false);
		objective.gameObject.SetActive(false);
		for(int i = 0; i < infoBlocks.Length; i++) {
			infoBlocks[i].SetActive(false);
		}

		folders[currentChoice].transform.SetAsLastSibling();

		infoBlocks[currentChoice].SetActive(true);

		switch(currentChoice) {
			case 0: // setup graph
				setupGraphs();
				break;
			case 1: // setup info
				setupInfo();
				break;
		}
	}


	void setupGraphs() {
		foreach(Outline ol in btnOutlines) {
			ol.enabled = true;
		}

		happy.gameObject.SetActive(true);
		population.gameObject.SetActive(true);
		objective.gameObject.SetActive(true);

		happy.positionCount = gm.prevHapp.Count;
		for(int i = 0; i < happy.positionCount; i++) {
			happy.SetPosition(i, new Vector3(i,(float)(gm.prevHapp[i]) / 10f, 0));
		}

		objective.positionCount = gm.prevObjec.Count;
		for(int i = 0; i < objective.positionCount; i++) {
			objective.SetPosition(i, new Vector3(i,(float)(gm.prevObjec[i]) / 10f, 0));
		}

		population.positionCount = gm.prevPop.Count;
		for(int i = 0; i < population.positionCount; i++) {
			population.SetPosition(i, new Vector3(i,(float)(gm.prevPop[i]) / 10f, 0));
		}
	}

	void setupInfo() {
		information[0].text = gm.populationVal.ToString();
		information[1].text = gm.happinessVal.ToString();
		information[2].text = gm.objectiveVal.ToString();

		information[3].text = returnLargestGain("Mine");
		information[4].text = returnLargestGain("Happy");
		information[5].text = returnLargestGain("Pop");

		float t = gm.populationVal / (float)gm.currentTurn;
		information[6].text = t.ToString("F2");
		t = gm.happinessVal / (float)gm.currentTurn;
		information[7].text = t.ToString("F2");
		t = gm.objectiveVal / (float)gm.currentTurn;
		information[8].text = t.ToString("F2");
	}


	string returnLargestGain(string name) {
		string retVal = "";
		int temp = 0;
		int hldr = 0;
		switch(name) {
			case "Happy":

				for(int i = 0; i < gm.prevHapp.Count; i++) {
					if(i+1 <= gm.prevHapp.Count-1) {
						temp = gm.prevHapp[i+1] - gm.prevHapp[i]; 
						if(temp > hldr) {
							hldr = temp;
						}
					}
				}

				retVal = hldr.ToString();
				break;

			case "Pop":
								
				for(int i = 0; i < gm.prevPop.Count; i++) {
					if(i+1 <= gm.prevPop.Count-1) {
						temp = gm.prevPop[i+1] - gm.prevPop[i]; 
						if(temp > hldr) {
							hldr = temp;
						}
					}
				}

				retVal = hldr.ToString();
				break;


			case "Mine":

				for(int i = 0; i < gm.prevObjec.Count; i++) {
					if(i+1 <= gm.prevObjec.Count-1) {
						temp = gm.prevObjec[i+1] - gm.prevObjec[i]; 
						if(temp > hldr) {
							hldr = temp;
						}
					}
				}

				retVal = hldr.ToString();
				break;

		}

		return retVal;
	}

	public void changeChoice(int i) {
		currentChoice = i;
		SetInfoBlock();
	}

	public void buttonPress(int i) {
		btnOutlines[i].enabled = !btnOutlines[i].enabled;

		switch(i) {
			case 0:
				happy.gameObject.SetActive(btnOutlines[i].enabled);
				break;
			case 1:
				population.gameObject.SetActive(btnOutlines[i].enabled);
				break;
			case 2:
				objective.gameObject.SetActive(btnOutlines[i].enabled);
				break;
		}
	}

	public void home() {
		BackEndManager.instance.ChangeState(STATES.PREGAME);
	}

}
