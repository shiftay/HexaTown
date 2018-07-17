using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public GameObject[] hexCells;

	public GameObject[,] grid;

	public int[,] gameplayObj;
	
	public int rows;
	public int cols;

	bool firstRun = true;


	// Use this for initialization
	void Start () {
		if(firstRun) {
			grid = new GameObject[rows,cols];
			gameplayObj = new int[rows,cols];
			int currentRow = 0;
			int currentCol = 0;

			for(int i = 0; i < hexCells.Length; i++) {
				if( i != 0 && i % cols == 0) {
					currentRow++;
					currentCol = 0;
				}
				gameplayObj[currentRow, currentCol] = -1;
				grid[currentRow, currentCol] = hexCells[i];

				currentCol++;
			}
			firstRun = false;
		}
	}
	



	// Update is called once per frame
	void Update () {
		// Debug.Log("Hello");
	}

	// EVEN = [-1,-1] [-1, 0] [0, -1] [0, +1] [+1, 0] [+1, -1] 
	// ODD 	= [-1, 0] [-1, +1] [0, -1] [0, +1] [+1, 0]  [+1, +1]

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

	public void removeFromGrid(GameObject tile) {
		for(int i = 0; i < rows; i++) {
			for(int j = 0; j < cols; j++) {
				if(grid[i,j] == tile) {
					gameplayObj[i,j] = -1;
				}
			}
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


				grid[currentRow,currentCol].AddComponent<TileInfo>().SetInfo( temp ,saved[i], states[currentState], states[currentState + 1], states[currentState + 2]);
				GameManager.instance.currentTiles.Add(grid[currentRow,currentCol].GetComponent<TileInfo>());
				grid[currentRow,currentCol].GetComponent<SpriteRenderer>().sprite = GameManager.instance.hc.cm.tiles[saved[i]];

				currentState += 3;
			}

			currentCol++;
		}


	}
}
