using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTracker : MonoBehaviour {
	public GameObject bus;
	public GameObject party;
	public Animation partyChip;
	public AnimationClip[] pSlides;
	public Animation commChip;
	public AnimationClip[] cSlides;
	public Animation fundChip;
	public AnimationClip[] fSlides;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update() {
		// bus.SetActive(GameManager.instance.commuters);
		// party.SetActive(GameManager.instance.party);
	}


	public void PARTY(bool val) {
		if(val) {
			partyChip.clip = pSlides[0];
			partyChip.Play();
		} else {
			partyChip.clip = pSlides[1];
			partyChip.Play();
		}
	}

	public void FUNDING(bool val) {
		if(val) {
			fundChip.clip = fSlides[0];
			fundChip.Play();
		} else {
			fundChip.clip = fSlides[1];
			fundChip.Play();
		}
	}

	public void COMMUTE(bool val) {
		if(val) {
			commChip.clip = cSlides[0];
			commChip.Play();
		} else {
			commChip.clip = cSlides[1];
			commChip.Play();
		}
	}
}
