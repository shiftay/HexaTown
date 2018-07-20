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

		GameManager gm = GameManager.instance;

		happy.positionCount = gm.prevHapp.Count;
		for(int i = 0; i < happy.positionCount; i++) {
			happy.SetPosition(i, new Vector3(i,(float)(gm.prevHapp[i]) / 3f, 0));
		}

		objective.positionCount = gm.prevObjec.Count;
		for(int i = 0; i < objective.positionCount; i++) {
			objective.SetPosition(i, new Vector3(i,(float)(gm.prevObjec[i]) / 3f, 0));
		}

		population.positionCount = gm.prevPop.Count;
		for(int i = 0; i < population.positionCount; i++) {
			population.SetPosition(i, new Vector3(i,(float)(gm.prevPop[i]) / 3f, 0));
		}

	}

}
