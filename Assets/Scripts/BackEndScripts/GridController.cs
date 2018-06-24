using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

	public GameObject[] hexCells;

	public GameObject[,] grid;
	
	public int rows;
	public int cols;


	// Use this for initialization
	void Start () {
		grid = new GameObject[rows,cols];
		int currentRow = 0;
		int currentCol = 0;

		for(int i = 0; i < hexCells.Length; i++) {
			if( i != 0 && i % cols == 0) {
				currentRow++;
				currentCol = 0;
			}

			grid[currentRow, currentCol] = hexCells[i];

			currentCol++;
		}

	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
