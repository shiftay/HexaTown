using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDesc : MonoBehaviour {

	public Text ghostWriteArea;
	public List<string> ghostWriteTxt;
	public int currentGW;
	public int counter = 0;
	public GameObject[] BuildArrows;
	public GameObject happyArrow;
	public GameObject workerArrow;
	public GameObject houseArrow;

	bool ghostWrite = false;
	bool erase = false;
	float timePassed;
	public float speed;
	bool halfPoint = true;
	bool invoked = false;


	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		currentGW = 0;
		counter = 0;

		clear();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		timePassed += Time.deltaTime;
		if(erase && !invoked) {
			currentGW++;
			if(currentGW < ghostWriteTxt.Count) {
				Invoke("clear", 2.0f);
			} else {
				//TODO: SWITCH TO NEXT PORTION.
			}
			invoked = true;
			ghostWrite = false;
		} else if(ghostWrite && timePassed > speed) {
			ghostWriteArea.text = whatToWrite();
			Debug.Log((counter > (ghostWriteTxt[currentGW].Length / 3)));
			if(halfPoint && (counter > (ghostWriteTxt[currentGW].Length / 4))) {
				turnOnOther();
				halfPoint = false;
			}
			timePassed = 0f;
		}

	}



	public string whatToWrite() {
		string retVal = "";

		for(int i = 0; i < counter; i++) {
			retVal += ghostWriteTxt[currentGW][i];
		}

		counter++;
		if(counter > ghostWriteTxt[currentGW].Length) {
			erase = true;
		}

		return retVal;
	}


	void turnOnOther() {
		switch(currentGW) {
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

			default:
				break;
		}
	}

	public void clear() {
		ghostWriteArea.text = "";
		happyArrow.SetActive(false);
		workerArrow.SetActive(false);
		houseArrow.SetActive(false);
		foreach(GameObject arrow in BuildArrows){
			arrow.SetActive(false);
		}
		counter = 0;
		erase = false;
		ghostWrite = true;
		invoked = false;
		halfPoint = true;
	}

}