using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILETYPE { COMMERCIAL, RESIDENTIAL, INDUSTRIAL, EVENT }

public class TileInfo : MonoBehaviour {

	public int xCoord, yCoord;
	public TILETYPE type;
	public int tileValue;
	public int buildTime;
	List<GameObject> children = new List<GameObject>();

	void Start() {
		if(gameObject.GetComponent<Flashing>()) {
			gameObject.GetComponent<Flashing>().enabled = false;
		}

		for(int i = 0; i < transform.childCount; i++) {
			children.Add(transform.GetChild(i).gameObject);
		}

		if(type != TILETYPE.EVENT) {
			children[0].SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {
		if(buildTime <= 0 && type == TILETYPE.EVENT){
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			GameManager.instance.currentTiles.Remove(this);
			gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.baseTile;
			Destroy(this);
		}
	}

	public void SetInfo(List<int> coords, int tileNum) {
	
		CardData temp = GameManager.instance.Info(tileNum);	
		
		xCoord = coords[0];
		yCoord = coords[1];
		type = temp.TYPE();
		buildTime = temp.BUILD();
		tileValue = temp.TVALUE();

		if(type == TILETYPE.EVENT) {
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void workersNeeded(bool toggle) {
		children[1].SetActive(toggle);
	}

	public void building(bool toggle) {
		children[0].SetActive(toggle);
	}

	public void unhappy(bool toggle) {
		children[2].SetActive(toggle);
	}

	public void clearTile() {
		GameManager.instance.currentTiles.Remove(this);
		gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.baseTile;
		Destroy(this);
	}
}
