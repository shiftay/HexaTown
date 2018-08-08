using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour {

	public GameObject[] helpStates;
	public GameObject[] icons;
	public GameObject[] cardLayout;
	int currentState = 0;
	int maxStates;
	
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		maxStates = helpStates.Length;
		changeState(0);
	}


	public void forward() {
		icons[0].SetActive(false);
		icons[1].SetActive(true);
	}

	public void backward() {
		icons[0].SetActive(true);
		icons[1].SetActive(false);		
	}

	public void cardFor() {
		cardLayout[0].SetActive(false);
		cardLayout[1].SetActive(true);
	}

	public void cardBack() {
		cardLayout[0].SetActive(true);
		cardLayout[1].SetActive(false);
	}

	public void changeState(int state) {
		foreach(GameObject go in helpStates) {
			go.SetActive(false);
		}
		icons[1].SetActive(false);
		cardLayout[1].SetActive(false);

		helpStates[state].SetActive(true);
	}

	public void back() {
		BackEndManager.instance.LeaveCredits();
	}

}
