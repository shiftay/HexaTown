using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public GameObject mainUI;
	public GameObject TurnOVERUI;
	
	// Update is called once per frame
	void Update () {
		
	}


	public void TurnOVER() {
		mainUI.SetActive(false);
		TurnOVERUI.SetActive(true);
	}

	public void TurnStart() {
		mainUI.SetActive(true);
		TurnOVERUI.SetActive(false);
	}

}
