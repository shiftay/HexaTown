using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {

	public Image img;
	public int currentImage;
	public Sprite[] stockImages;
	public string dName;
	public InputField iField;

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()	{
		currentImage = 0;
		dName = "";
		iField.text = "";
		if(BackEndManager.instance.editDeck) {
			currentImage = BackEndManager.instance.decks[BackEndManager.instance.deckToEdit].imageNumber;
			dName = BackEndManager.instance.decks[BackEndManager.instance.deckToEdit].name;
			iField.text = dName;
		}

		img.sprite = stockImages[currentImage];
	}


	public void Next() {
		currentImage++;

		if(currentImage > stockImages.Length) {
			currentImage = 0;
		}

		img.sprite = stockImages[currentImage];
	}

	public void Back() {
		currentImage--;

		if(currentImage < 0) {
			currentImage = stockImages.Length - 1;
		}

		img.sprite = stockImages[currentImage];
	}


	public void EditDeckName() {
		dName = iField.text;
	}

}
