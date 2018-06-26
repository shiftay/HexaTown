using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
	public Sprite[] dirtTerrain;
	public SpriteRenderer[] bgSlots;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		for(int i = 0; i < bgSlots.Length; i++) {
			bgSlots[i].sprite = dirtTerrain[Random.Range(0, dirtTerrain.Length)];
		}
	}

}
