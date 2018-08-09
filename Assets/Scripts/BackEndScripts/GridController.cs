using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Coords {
	public int x, y;
	public Coords(int c, int v) {
		x = c;
		y = v;
	}
}
public class GridController : MonoBehaviour {

	public GameObject[] hexCells;
	public GameObject[,] grid;
	public int[,] gameplayObj;
	public int rows;
	public int cols;
	List<Coords> industryArea = new List<Coords>();

	bool firstRun = true;
	public GameObject industryGrid;
	public SpriteRenderer[] indGsprite;
	SpriteRenderer[,] spriteGrid;
	public int testX = 0;
	public int testY = 0;
	public Sprite indTile;
	public Heatmap hm;

	public List<int> INDX {
		get{
			List<int> temp = new List<int>();

			for(int i = 0; i< industryArea.Count; i++) {
				temp.Add(industryArea[i].x);
			}

			return temp;
		}
	}

	public List<int> INDY {
		get{
			List<int> temp = new List<int>();

			for(int i = 0; i< industryArea.Count; i++) {
				temp.Add(industryArea[i].y);
			}

			return temp;
		}
	}


	// Use this for initialization
	void Start () {
		if(firstRun) {
			grid = new GameObject[rows,cols];
			gameplayObj = new int[rows,cols];
			spriteGrid = new SpriteRenderer[rows,cols];
			int currentRow = 0;
			int currentCol = 0;

			for(int i = 0; i < hexCells.Length; i++) {
				if( i != 0 && i % cols == 0) {
					currentRow++;
					currentCol = 0;
				}
				gameplayObj[currentRow, currentCol] = -1;
				grid[currentRow, currentCol] = hexCells[i];
				spriteGrid[currentRow, currentCol] = indGsprite[i];
				currentCol++;
			}
			firstRun = false;
		}
		if(!BackEndManager.instance.resume) {
			ClearTileInfo();
		}
 		
		 
	}
	

	public void SetupCoord(int x, int y) {
		industryArea.Add(new Coords(x,y));
	}

	// Update is called once per frame
	void Update () {
		// Debug.Log("Hello");
	}

	public void ClearTileInfo() {
		foreach(GameObject go in hexCells) {
			if(go.GetComponent<TileInfo>()) {
				Destroy(go.GetComponent<TileInfo>());
			}
		}
	}
	
	public void setupIndustryTiles() {
		Start();
		industryArea.Clear();
		// -2 = industry
		// -1 = nothing;
		int x = Random.Range(0, rows);
		int y = Random.Range(0, cols);
		// int x = testX;
		// int y = testY;
		bool oddN = (x + 1) % 2 == 0;
		int amt = 0;

		if((x == 0 && y == 0) || (x == 5 && y == 8)) {
			amt = 7;
		} else if ((x == 5 && y == 0) || (x == 0 && y == 8)){
			amt = 8;
		} else {
			amt = Random.Range(8, 13);
		}

		temp(x,y);

		if(oddN) {
			temp(x-1,y);
			temp(x-1,y+1);
			temp(x,y-1);
			temp(x,y+1);
			temp(x+1,y);
			temp(x+1,y+1);
		} else {

			temp(x-1,y-1);
			temp(x-1,y);
			temp(x,y-1);
			temp(x,y+1);
			temp(x+1,y);
			temp(x+1,y-1);
		}

		while(industryArea.Count < amt) {
			if(oddN) {
				industEVEN(x,y);
				
			} else {
				industODD(x,y);
			}
		}

		IndustrySprites();
		hm.Setup(this);
	}

	public void IndustrySprites() {		
		foreach(Coords c in industryArea) {
			// spriteGrid[c.x,c.y].sprite = GameManager.instance.industryArea;
			if(!grid[c.x,c.y].GetComponent<TileInfo>()) {
				grid[c.x,c.y].GetComponent<SpriteRenderer>().sprite = indTile;
			}
		}
		industryGrid.SetActive(false);
	}

	void temp(int xC, int yC) {
		if(inZone(xC,yC)) {
			Coords temp = new Coords(xC, yC);
			bool flag = false;

			for(int i = 0; i < industryArea.Count; i++) {
				if(industryArea[i].x == xC && industryArea[i].y == yC){
					flag = true;
				}
			}

			if(!flag) {
				industryArea.Add(temp);
			}
		}
	}


	void industODD(int x, int y) {
		switch(Random.Range(0,12)) {
			case 0:
				temp(x,y-2);
				break;
			case 1:
				temp(x,y+2);
				break;
			case 2:
				temp(x+1,y+1);
				break;
			case 3:
				temp(x+1,y-1);
				break;
			case 4:
				temp(x-1,y-2);
				break;
			case 5:
				temp(x-1,y+1);
				break;
			case 6:
				temp(x-2,y+1);
				break;
			case 7:
				temp(x-2,y);
				break;
			case 8:
				temp(x-2,y-1);
				break;
			case 9:
				temp(x+2,y+1);
				break;
			case 10:
				temp(x+2,y);
				break;
			case 11:
				temp(x+2,y-1);
				break;
		}
	}

	void industEVEN(int x, int y) {
		switch(Random.Range(0,12)) {
			case 0:
				temp(x,y-2);
				break;
			case 1:
				temp(x,y+2);
				break;
			case 2:
				temp(x-1,y-1);
				break;
			case 3:
				temp(x-1,y+2);
				break;
			case 4:
				temp(x+1,y-1);
				break;
			case 5:
				temp(x+1,y+2);
				break;
			case 6:
				temp(x-2,y+1);
				break;
			case 7:
				temp(x-2,y);
				break;
			case 8:
				temp(x-2,y-1);
				break;
			case 9:
				temp(x+2,y+1);
				break;
			case 10:
				temp(x+2,y);
				break;
			case 11:
				temp(x+2,y-1);
				break;
		}
	}


	bool inZone(int x, int y) {
		return (x >= 0 && x < rows) && (y >= 0 && y < cols);
	}


	public List<int> updateGRID(GameObject pos2UPD, int newVal) {
		List<int> coords = new List<int>();
		for(int i = 0; i < rows; i++) {
			for(int j = 0; j < cols; j++) {
				if(grid[i,j] == pos2UPD) {
					gameplayObj[i,j] = newVal;
					coords.Add(i);
					coords.Add(j);
				}
			}
		}

		return coords;
	}

	public List<int> COORDS(GameObject pos2UPD) {
		List<int> coords = new List<int>();
		
		for(int i = 0; i < rows; i++) {
			for(int j = 0; j < cols; j++) {
				if(grid[i,j] == pos2UPD) {
					coords.Add(i);
					coords.Add(j);
				}
			}
		}

		return coords;
	}

	public void removeFromGrid(GameObject tile) {
		bool flag = false;

		// TODO: CHECK POSITION AGAINST THE NEW STORED COORDS
		for(int i = 0; i < rows; i++) {
			for(int j = 0; j < cols; j++) {
				if(grid[i,j] == tile) {
					if(industryCheck(i,j)) {
						flag = true;
					}
					
					gameplayObj[i,j] = -1;
				}
			}
		}

		if(flag) {
			tile.GetComponent<SpriteRenderer>().sprite = indTile;
		} else {
			tile.GetComponent<SpriteRenderer>().sprite = GameManager.instance.baseTile;
		}
	}



	public void ResumeGrid(List<int> saved, List<int> states) {
		Start();
		int currentRow = 0;
		int currentCol = 0;
		int currentState = 0;

		for(int i = 0; i < saved.Count; i++) {
				if( i != 0 && i % cols == 0) {
					currentRow++;
					currentCol = 0;
				}
			gameplayObj[currentRow, currentCol] = saved[i];
			if(saved[i] != -1) {
				List<int> temp = new List<int>();
				temp.Add(currentRow);
				temp.Add(currentCol);

				Debug.Log("COORDS : " + currentRow + " / " + currentCol);
				grid[currentRow,currentCol].AddComponent<TileInfo>();
				if(saved[i] == -3) {
					grid[currentRow,currentCol].GetComponent<SpriteRenderer>().sprite = GameManager.instance.water;
					grid[currentRow,currentCol].GetComponent<TileInfo>().SetInfo(temp ,saved[i], states[currentState], states[currentState + 1], states[currentState + 2]);
				} else {
					grid[currentRow,currentCol].GetComponent<TileInfo>().SetInfo(temp ,saved[i], states[currentState], states[currentState + 1], states[currentState + 2]);
					grid[currentRow,currentCol].GetComponent<SpriteRenderer>().sprite = GameManager.instance.hc.cm.tiles[saved[i]];
				}
				grid[currentRow,currentCol].GetComponent<TileInfo>().SetInfo(temp ,saved[i], states[currentState], states[currentState + 1], states[currentState + 2]);
				GameManager.instance.currentTiles.Add(grid[currentRow,currentCol].GetComponent<TileInfo>());


				currentState += 3;
			}

			currentCol++;
		}


	}

	public void resetTiles(Sprite tile) {
		foreach(GameObject x in hexCells) {
			x.GetComponent<SpriteRenderer>().sprite = tile;
			for(int i = 0; i < x.transform.childCount; i++) {
				x.transform.GetChild(i).gameObject.SetActive(false);
			}

		}

		int currentRow = 0;
		int currentCol = 0;

		for(int i = 0; i < hexCells.Length; i++) {
			if( i != 0 && i % cols == 0) {
				currentRow++;
				currentCol = 0;
			}
			gameplayObj[currentRow, currentCol] = -1;

			currentCol++;
		}
	}

	// EVEN = [-1,-1] [-1, 0] [0, -1] [0, +1] [+1, 0] [+1, -1] 
	// ODD 	= [-1, 0] [-1, +1] [0, -1] [0, +1] [+1, 0]  [+1, +1]
	public bool placeTile(List<int> coords, TILETYPE tileType, bool first, bool completeFirst) {
		bool retVal = false;
		GameManager gm = GameManager.instance;
		int x = coords[0]; // row
		int y = coords[1]; // col
		bool oddN = (x + 1) % 2 == 0;
		List<bool> results = new List<bool>();
		if(!completeFirst) {
			if(oddN) {
				results.Add(TileCheck(x-1,y,tileType, first));
				results.Add(TileCheck(x-1,y+1,tileType, first));
				results.Add(TileCheck(x,y-1,tileType, first));
				results.Add(TileCheck(x,y+1,tileType, first));
				results.Add(TileCheck(x+1,y,tileType, first));
				results.Add(TileCheck(x+1,y+1,tileType, first));
			} else {

				results.Add(TileCheck(x-1,y-1,tileType, first));
				results.Add(TileCheck(x-1,y,tileType, first));
				results.Add(TileCheck(x,y-1,tileType, first));
				results.Add(TileCheck(x,y+1,tileType, first));
				results.Add(TileCheck(x+1,y,tileType, first));
				results.Add(TileCheck(x+1,y-1,tileType, first));
			}

			int count = 0;
			for(int i = 0; i < results.Count; i++) {
				if(results[i]) {
					count++;
				}
			}

			if(count > 0) {
				retVal = true;
			}
		} else {
			retVal = true;
		}

		return retVal;
	}

	public bool industryCheck(List<int> pos) {
		bool retVal = false;

		for(int i = 0; i < industryArea.Count; i++) {
			if(industryArea[i].x == pos[0] && industryArea[i].y == pos[1]){
				retVal = true;
			}
		}

		return retVal;
	}

	public bool industryCheck(int x, int y) {
		bool retVal = false;

		for(int i = 0; i < industryArea.Count; i++) {
			if(industryArea[i].x == x && industryArea[i].y == y){
				retVal = true;
			}
		}

		return retVal;
	}

	public bool TileCheck(int x, int y, TILETYPE type, bool test) {
		bool retVal = false;

		if(test) {
			if((x >= 0 && x < rows) && (y >= 0 && y < cols)) {
				if(gameplayObj[x,y] >= 0) {
					retVal = true;
				}
			}
		} else {
			if((x >= 0 && x < rows) && (y >= 0 && y < cols)) {
				if(gameplayObj[x,y] >= 0) {
					retVal = GameManager.instance.cardData[gameplayObj[x,y]].TYPE() == type;
				}
			}
		}

		return retVal;
	}

	public bool surrounded(TILETYPE t) {
		bool retVal = false;

		List<TileInfo> to = new List<TileInfo>();

		foreach(TileInfo tile in GameManager.instance.currentTiles) {
			if(tile.type == t) {
				to.Add(tile);
			}
		}

		List<bool> surrounded = new List<bool>();
		int count = 0;
		if(to.Count > 0) {
			bool oddN;
			foreach(TileInfo x in to) {
				int temp = 0;
				oddN = (x.xCoord + 1) % 2 == 0;
				if(oddN) {
					surrounded.Add(surroundCheck(x.xCoord-1,x.yCoord,t));
					surrounded.Add(surroundCheck(x.xCoord-1,x.yCoord+1,t));
					surrounded.Add(surroundCheck(x.xCoord,x.yCoord-1,t));
					surrounded.Add(surroundCheck(x.xCoord,x.yCoord+1,t));
					surrounded.Add(surroundCheck(x.xCoord+1,x.yCoord,t));
					surrounded.Add(surroundCheck(x.xCoord+1,x.yCoord+1,t));
				} else {

					surrounded.Add(surroundCheck(x.xCoord-1,x.yCoord-1,t));
					surrounded.Add(surroundCheck(x.xCoord-1,x.yCoord,t));
					surrounded.Add(surroundCheck(x.xCoord,x.yCoord-1,t));
					surrounded.Add(surroundCheck(x.xCoord,x.yCoord+1,t));
					surrounded.Add(surroundCheck(x.xCoord+1,x.yCoord,t));
					surrounded.Add(surroundCheck(x.xCoord+1,x.yCoord-1,t));
				}

				for(int i = 0; i < surrounded.Count; i++) {
					if(surrounded[i]) {
						temp++;
					}
				}

				if(temp == surrounded.Count) {
					count++;
				}

			}
		}

		if(count == to.Count) {
			retVal = true;
		}

		return retVal;
	}



	public bool surroundCheck(int x, int y, TILETYPE type) {
		bool retVal = false;

		if((x >= 0 && x < rows) && (y >= 0 && y < cols)) {
			if(gameplayObj[x,y] != -1) {
				retVal = GameManager.instance.cardData[gameplayObj[x,y]].TYPE() != type;
			}
		} else {
			retVal = true;
		}
	
		return retVal;
	}


	public void ShowGrid(bool toggle) {
		industryGrid.SetActive(toggle);
	}
}
