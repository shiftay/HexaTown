using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public LineRenderer happy;
	public LineRenderer objective;
	public LineRenderer population;

	public Text title;

	public GameObject[] infoBlocks;
	public int currentChoice = 0;

	public Text[] information;
	public Outline[] btnOutlines;
	GameManager gm;
	public GameObject[] folders;

	public Text[] turnNums;
	public GameObject back;
	public GameObject next;

	int currentPage = 0;
	List<int> turnEnds = new List<int>();
	

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
		setTURNS();
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

		setTURNS();
		happy.gameObject.SetActive(true);
		population.gameObject.SetActive(true);
		objective.gameObject.SetActive(true);
		enableBtns();
		turnText();
		


		happy.positionCount = turnEnds[currentPage];
		for(int i = currentPage * 30; i < happy.positionCount; i++) {
			happy.SetPosition(i, new Vector3(i,(float)(gm.prevHapp[i]) / 10f, 0));
		}

		objective.positionCount = turnEnds[currentPage];
		for(int i = currentPage * 30; i < objective.positionCount; i++) {
			objective.SetPosition(i, new Vector3(i,(float)(gm.prevObjec[i]) / 10f, 0));
		}

		population.positionCount = turnEnds[currentPage];
		for(int i = currentPage * 30; i < population.positionCount; i++) {
			population.SetPosition(i, new Vector3(i,(float)(gm.prevPop[i]) / 10f, 0));
		}
	}

	void enableBtns() {
		if(turns() > 1) {
			back.SetActive(true);
			next.SetActive(true);

		} else {
			//turn off
			back.SetActive(false);
			next.SetActive(false);
		}
	}

	void pageBtns() {
		if(currentPage == 0) {
			//forward on
			//back off
			back.SetActive(false);
			next.SetActive(true);
		} 


		if(currentPage == Mathf.Round(turns())) {
			//forward off
			//back on

			back.SetActive(true);
			next.SetActive(false);
		}
	}

	float turns() {
		return gm.prevHapp.Count / 30f;
	}

	void setTURNS() {
		int x = gm.prevHapp.Count;
		int count = 1;
		do {

			if(x - 30 > 0) {
				turnEnds.Add(count * 30);
				count++;
			} else {
				turnEnds.Add(x);
				x -= x;
			}

		} while(x > 0);
	}

	void turnText() {

		int x = currentPage * 30;
		for(int i = 0; i < turnNums.Length; i++) {
			if(x > turnEnds[currentPage]) {
				turnNums[i].gameObject.SetActive(false);
			} else {
				turnNums[i].text = x.ToString();
				x += 5;
			}	
		}

	}

	public void forward() {
		currentPage++;
		setupGraphs();
	}

	public void backward() {
		currentPage--;
		setupGraphs();
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
