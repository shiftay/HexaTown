using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILETYPE { COMMERCIAL, RESIDENTIAL, INDUSTRIAL, EVENT, SPELL }

public class TileInfo : MonoBehaviour {

	public int xCoord, yCoord;
	public int cardNumber;
	public TILETYPE type;
	public int tileValue;
	public int corruptVal;
	public int buildTime;
	List<GameObject> children = new List<GameObject>();
	public bool scheduledDemo = false;
	public bool turnOffBuild = false;
	public int buildValue = 0;

	void Start() {
		if(gameObject.GetComponent<Flashing>()) {
			gameObject.GetComponent<Flashing>().enabled = false;
		}

		if(children.Count == 0) {
			for(int i = 0; i < transform.childCount; i++) {
				children.Add(transform.GetChild(i).gameObject);
			}
		}

		if(type != TILETYPE.EVENT && buildTime != 0 && !scheduledDemo) {
			children[0].SetActive(true);
		}

		if(corruptVal != 0) {
			children[6].SetActive(true);
		}	
	}

	// Update is called once per frame
	void Update () {
		if(buildTime <= 0 && type == TILETYPE.EVENT){
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			GameManager.instance.currentTiles.Remove(this);
			if(GameManager.instance.gc.industryCheck(xCoord, yCoord)) {
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.gc.indTile;
			} else {
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.baseTile;
			}
			
			Destroy(this);
		}

		if(buildTime <= 0 && scheduledDemo) {
			clearTile();
		}

		if(turnOffBuild) {
			building(false);
			turnOffBuild = false;
		}
	}

	public void SetInfo(List<int> coords, int tileNum) {
		CardData temp = GameManager.instance.Info(tileNum);	
		cardNumber = tileNum;
		xCoord = coords[0];
		yCoord = coords[1];
		type = temp.TYPE();
		buildTime = temp.BUILD();
		tileValue = temp.TVALUE();
		corruptVal = temp.CORRUPT();
		buildValue = bVal();

		if(type == TILETYPE.EVENT) {
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void SetInfo(List<int> coords, int tileNum, int corruptV, int buildVal, int activeChild) {
		for(int i = 0; i < transform.childCount; i++) {
			children.Add(transform.GetChild(i).gameObject);
		}

		CardData temp = GameManager.instance.Info(tileNum);	
		cardNumber = tileNum;
		xCoord = coords[0];
		yCoord = coords[1];
		type = temp.TYPE();
		buildTime = buildVal;
		tileValue = temp.TVALUE();
		corruptVal = corruptV;
		buildValue = bVal();

		if(activeChild > 0) {
			children[activeChild].SetActive(true);
			if(activeChild > 2 && activeChild < 6) {
				scheduledDemo = true;
			}
		}


		if(type == TILETYPE.EVENT) {
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			scheduledDemo = true;
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

	public void demolish(bool toggle) {
		foreach(GameObject go in children) {
			go.SetActive(false);
		}

		children[3].SetActive(toggle);
		scheduledDemo = toggle;
		buildTime = 2;
	}

	public void crime(bool toggle) {
		foreach(GameObject go in children) {
			go.SetActive(false);
		}

		children[4].SetActive(toggle);
		scheduledDemo = toggle;
		buildTime = 1;
	}

	public void bugs(bool toggle) {
		foreach(GameObject go in children) {
			go.SetActive(false);
		}

		children[5].SetActive(toggle);
		scheduledDemo = toggle;
		buildTime = 1;
	}

	public void purify() {
		children[6].SetActive(false);
		corruptVal = 0;
	}


	public void clearTile() {
		GameManager.instance.currentTiles.Remove(this);
		GameManager.instance.gc.removeFromGrid(this.gameObject);
	
		unhappy(false);
		building(false);
		workersNeeded(false);
		demolish(false);
		DestroyImmediate(this);
	}

	public int bVal() {
		int retVal = 0;

		switch(cardNumber) {
			case 8:
				retVal = 1;
				break;
			case 9:
				retVal = 3;
				break;
			case 10:
				retVal = 2;
				break;
			case 26:
				retVal = 2;
				break;
			case 27:
				retVal = 2;
				break;
		}

		return retVal;
	}

	public void buildTile() {
		buildTime = 0;
		building(false);
	}

	public int activeChild() {
		int retVal = -1;

		for(int i = 0 ; i < children.Count; i ++) {
			if(children[i].activeInHierarchy) {
				if(i != 0) {
					retVal = i;
				}
			}
		}



		return retVal;
	}
}
