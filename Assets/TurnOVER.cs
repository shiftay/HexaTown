using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOVER : MonoBehaviour {

	public Image[] cardsPlayed;
 
	public Text[] summaryInfo;

	public void completeTurn() {
		GameManager.instance.resolveTurn();
	}
}
