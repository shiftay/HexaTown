using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RequestAd : MonoBehaviour {

	bool loaded = false;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(!loaded && BackEndManager.instance.doneLoading()) {
			loaded = true;
			BackEndManager.instance.fo.outtro();
			Invoke("callAd", 1f);
		}
	}

	void callAd() {
		BackEndManager.instance.loadAd();
	}

}
