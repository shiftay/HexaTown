using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTracker : MonoBehaviour {
	public GameObject bus;
	public GameObject party;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update() {
		bus.SetActive(GameManager.instance.commuters);
		party.SetActive(GameManager.instance.party);
	}


}
