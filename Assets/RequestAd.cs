using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RequestAd : MonoBehaviour {

	bool loaded = false;
	InterstitialAd inter;
	AdRequest req;
	bool firstRun = false;
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		inter = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
		firstRun = BackEndManager.instance.firstRun;
		req = new AdRequest.Builder().Build();
		inter.LoadAd(req);

		inter.OnAdClosed += HandleOnAdClosed;
		inter.OnAdFailedToLoad += adFailed;
		// BackEndManager.instance.fo.creditsFade();
		
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(!loaded && inter.IsLoaded()) {
			loaded = true;
			BackEndManager.instance.fo.outtro();
			Invoke("callAd", 1f);
		}
	}

	public void HandleOnAdClosed(object sender, EventArgs args)	{
		if(firstRun) {
			BackEndManager.instance.ChangeState(STATES.TUTORIAL);
		} else {
			BackEndManager.instance.ChangeState(STATES.MAINMENU);
		}
		inter.Destroy();
	}

	public void adFailed(object sender, EventArgs args)	{
		if(firstRun) {
			BackEndManager.instance.ChangeState(STATES.TUTORIAL);
		} else {
			BackEndManager.instance.ChangeState(STATES.MAINMENU);
		}
		inter.Destroy();
	}

	void callAd() {
		inter.Show();
	}

}
