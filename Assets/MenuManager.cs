using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public GameObject resumeBtn;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		//TODO: Check if there is a saved game ot continue
		if( BackEndManager.instance) {
			if(BackEndManager.instance.sGame != null) {
				resumeBtn.SetActive(true);
			}
		}

	}



	public void Resume() {

	}

	public void Play() {
		BackEndManager.instance.ChangeState(STATES.PREGAME);
	}

	public void Options() {
		BackEndManager.instance.ChangeState(STATES.OPTIONS);
	}
}
