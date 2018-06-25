using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public GameObject[] hexCells;

	public GameObject[,] grid;

	public int[,] gameplayObj;
	
	public int rows;
	public int cols;


	// Use this for initialization
	void Start () {
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
	}
	



	// Update is called once per frame
	void Update () {
		Debug.Log("Hello");
	}





	void CheckAround(bool rowOdd) {
		if(rowOdd) {
			




		} else {



		}



	}
	// EVEN = [-1,-1] [-1, 0] [0, -1] [0, +1] [+1, 0] [+1, -1] 
	// ODD 	= [-1, 0] [-1, +1] [0, -1] [0, +1] [+1, 0]  [+1, +1]


	public void updateGRID(GameObject pos2UPD, int newVal) {

		for(int i = 0; i < rows; i++) {
			for(int j = 0; j < cols; j++) {
				if(grid[i,j] == pos2UPD) {
					gameplayObj[i,j] = newVal;
				}
			}
		}

	}



}
