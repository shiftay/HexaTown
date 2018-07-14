using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {
	BackEndManager bm;
	public Slider volume;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		if(BackEndManager.instance) {
			bm = BackEndManager.instance;

			volume.value = bm.currentVolume;



		}
	}




	public void back() {
		bm.RevertState();
	}

}
