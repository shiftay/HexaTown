using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

	public Text objective;
	public Text discard;
	public Text remaining;
	
	// Update is called once per frame
	void Update () {
		objective.text = GameManager.instance.objectiveVal.ToString();		
	}
}
