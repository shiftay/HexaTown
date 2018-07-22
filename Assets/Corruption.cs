using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corruption : MonoBehaviour {

	public List<string> ghostWriteTxt;
	public Text ghostWriteArea;

	public int currentGW;
	public int counter = 0;
	bool ghostWrite = false;
	bool erase = false;
	float timePassed;
	public float speed;
	bool halfPoint = true;
	bool invoked = false;


	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
