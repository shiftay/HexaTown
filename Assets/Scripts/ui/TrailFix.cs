using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFix : MonoBehaviour {
	public TrailRenderer[] tRenderers;
	public SpriteRenderer sprite;
	public Sprite[] sprites;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		tRenderers = GetComponentsInChildren<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeObject() {
		int r = UnityEngine.Random.Range(0,sprites.Length);

		sprite.sprite = sprites[r];

		foreach(TrailRenderer tr in tRenderers) {
			float alpha = 1.0f;
			Gradient gradient = new Gradient();

			switch(r) {
				case 0:
							gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.white, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
				);
					break;
				case 1:
							gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(Color.gray, 0.0f), new GradientColorKey(Color.white, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
				);
					break;
				case 2:
							gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(Color.black, 0.0f), new GradientColorKey(Color.white, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
				);
					break;
			}


			tr.colorGradient = gradient;

		}
	}
}
