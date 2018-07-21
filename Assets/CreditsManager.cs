using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostWrites {
	public List<Text> texts;

	public List<string> textToWrite;

	public GhostWrites(List<string> text, List<Text> txt) {
		texts = txt;
		textToWrite = text;
	}
}


public class CreditsManager : MonoBehaviour {

	public Text txt;
	public List<string> firstGhostWrite;
	bool ghostWrite = false;
	bool erase = false;
	int counter = 0;
	int typeCount = 0;
	int writePhase = 0;
	public float timePassed = 0f;
	public float speed;

	
	public List<Text> firstWrite;
	List<GhostWrites> gw = new List<GhostWrites>();
	bool calledFade = false;
	public List<Text> secondGW;
	public List<string> secondGWs;
	public List<Text> thirdGW;
	public List<string> thirdGWs;
	public List<Text> fourthGW;
	public List<string> fourthGWs;




	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		gw.Add(new GhostWrites(firstGhostWrite, firstWrite));
		gw.Add(new GhostWrites(fourthGWs, fourthGW));
		gw.Add(new GhostWrites(secondGWs, secondGW));
		gw.Add(new GhostWrites(thirdGWs, thirdGW));
		OnEnable();
	}



	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		writePhase = -1;
		counter = 0;
		clearPrev();
		writePhase = 0;
		typeCount = 0;
		resetVars();
	}




	void Update () {
		timePassed += Time.deltaTime;

		if(typeCount >= gw[writePhase].texts.Count) {
			ghostWrite = false;
			erase = true;
			
			//mydl =  new delegate;
		}




		if(erase && !calledFade) {
			
			black();

		} else if(timePassed > speed && ghostWrite) {



			gw[writePhase].texts[typeCount].text = firstGhost();
			timePassed = 0f;
		}





	}


	string firstGhost() {
		string retVal = "";

		for(int i = 0; i < counter; i++) {
			retVal += gw[writePhase].textToWrite[typeCount][i];
		}
	
		counter++;
		if(counter > gw[writePhase].textToWrite[typeCount].Length) {
			typeCount++;
			counter = 0;
		}
		return retVal;
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable()
	{
		ghostWrite = false;
	}

	void clearPrev() {
		for(int i = 0; i < gw.Count; i++) {
			if(writePhase != i) {
				for(int x = 0; x < gw[i].texts.Count; x++){
					gw[i].texts[x].text = "";
				}
			}
		}
	}


	void black() {

		writePhase++;
		calledFade = true;
		if(writePhase >= gw.Count){
			writePhase = 0;
		} else {
			BackEndManager.instance.fo.creditsFade();
			counter = 0;
			timePassed = 0f;
			
			typeCount = 0;
			Invoke("clearPrev", 2.0f);
			Invoke("resetVars", 2.5f);
		}
	}

	void resetVars() {
		erase = false;
		calledFade = false;
		ghostWrite = true;
		
	}

	public void leaveCredits() {
		BackEndManager.instance.LeaveCredits();
	}

}
