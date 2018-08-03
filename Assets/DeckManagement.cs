using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagement : MonoBehaviour {
	public bool discard;
	public GameObject[] cards;

	public void setCards(int amt) {
		foreach(GameObject go in cards) {
			go.SetActive(false);
		}

		for(int i = 0; i < amt; i++) {
			cards[i].SetActive(true);
		}
	}	

}
