using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class MenuManager : MonoBehaviour {

	public GameObject resumeBtn;
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()	{
		if( BackEndManager.instance) {
			if(BackEndManager.instance.sGame != null) {
				resumeBtn.SetActive(true);
			}
		}
	}


	void Start() {
		OnEnable();
	}

	public void Resume() {
		BackEndManager.instance.resume = true;
		BackEndManager.instance.ChangeState(STATES.GAME);
	}

	public void Play() {
		BackEndManager.instance.ChangeState(STATES.PREGAME);
	}

	public void Options() {
		BackEndManager.instance.ChangeState(STATES.OPTIONS);
	}
}
