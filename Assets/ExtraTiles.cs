using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTiles : MonoBehaviour {

	public SpriteRenderer[] tiles;
	public Sprite[] choices;

	void OnEnable()	{
		foreach(SpriteRenderer t in tiles) {
			t.sprite = choices[Random.Range(0, choices.Length)];
		}
	}
}
