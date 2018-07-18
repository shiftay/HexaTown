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

	void OnEnable()
	{
		if(BackEndManager.instance.gameWon) {
			title.text = "YOU WIN! YAY";
			//TODO ANIMATIONS/ FIREWORKS.
			//		PLAY A SOUND
		} else {
			title.text = "Unsuccessful";
		}
	}

}
