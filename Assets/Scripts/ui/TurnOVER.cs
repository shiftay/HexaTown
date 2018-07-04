using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOVER : MonoBehaviour {

	public Image[] cardsPlayed;
 
	public Text[] summaryInfo;
	public Image[] modifiers;
	public Text[] tMods;
	public Text title;
	public Text[] imageTitles;

	public void completeTurn() {
	 	GameManager.instance.resolveTurn();
	}
}
