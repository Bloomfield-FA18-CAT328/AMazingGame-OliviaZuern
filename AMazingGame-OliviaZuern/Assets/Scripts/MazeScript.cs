using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeScript
{
	// using primm's algorythm
	public byte[,] mapArray; //maze map.
	public List<Vector2> wallList;
	public int wall;
	public byte mapWidth; //[0, 255]
	public byte mapHeight;

	public string mapString;
	public bool check;

	// private int i;
	//private int j;

	private int XX;
	public byte cellX;
	public byte cellY;

	//Constructor - constructs a new instance of this thing, and gives you back a reference to it
	public MazeScript(byte width, byte height) {
		mapWidth = width; //You could sanitize these, you might want to
		mapHeight = height;
		ResizeMap();
		mapArray = new byte[mapWidth, mapHeight];
		wallList = new List<Vector2>(); //Capacity -- This is a List thing
		
		// note: 0=path, 1=wall, 2=untouched

		//1)Full grid of walls (false)
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				mapArray[x, y] = 2;
			}
		}
		// DebugMap();

		Random.Range(0f, 100f); //0.0f to 100.0f
		Random.Range(0, 100); //0 to 99
		//2)Pick cell, mark as part of maze, add cell walls to list
		cellX = (byte) Random.Range(1, mapWidth - 1);
		cellY = (byte) Random.Range(1, mapHeight - 1);

		mapArray[cellX, cellY] = 0;

		//Skipped
		if (cellY >= 1) { wallList.Add(new Vector2(cellX, cellY - 1)); }
		if (cellY <= mapHeight - 1) { wallList.Add(new Vector2(cellX, cellY + 1)); }
		if (cellX >= 1) { wallList.Add(new Vector2(cellX - 1, cellY)); }
		if (cellX <= mapWidth - 1) { wallList.Add(new Vector2(cellX + 1, cellY)); }

		//  Debug.Log(mapArray[1, 0]);
		//3) while there are 3+ walls on list:
		while (wallList.Count > 3) {

			// pick random wall from list. if only one of the two cells it divides is visited (0):
			// remove wall (true) and mark unvisited as part of maze  

			wall = Random.Range(0, wallList.Count);

			cellX = (byte) wallList[wall].x;
			cellY = (byte) wallList[wall].y;
			//int visited = 0;
			check = false;
			if (check == false && cellX <= mapWidth - 2 && cellX >= 1) {
				if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] == 0 && mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] != 0) {
					check = true;
					mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
					mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] = 1;
				}
			}
			if (check == false && cellX >= 1 && cellX <= mapWidth - 2) {
				if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] == 0 && mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] != 0) {
					check = true;
					mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
					mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] = 1;
				}
			}
			if (check == false && cellY >= 1 && cellY <= mapHeight - 2) {
				if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] == 0 && mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] != 0) {
					check = true;
					mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
					mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] = 1;
				}
			}
			if (check == false && cellY <= mapHeight - 2 && cellY >= 1) {
				if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] == 0 && mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] != 0) {
					check = true;
					mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
					mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] = 1;
				}
			}
			// add neighboring walls of cell to list
			if (check == true) {
				if (cellX >= 1 && cellX <= mapWidth - 2) {
					if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] == 2) { wallList.Add(new Vector2(cellX + 1, cellY)); }
					if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] == 2) { wallList.Add(new Vector2(cellX - 1, cellY)); }
				}
				if (cellY >= 1 && cellY <= mapHeight - 2) {
					if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] == 2) { wallList.Add(new Vector2(cellX, cellY - 1)); }
					if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] == 2) { wallList.Add(new Vector2(cellX, cellY + 1)); }
				}
			}


			/*  if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY + 1)] == 0) { visited += 1; }
              if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY + 1)] == 0) { visited += 1; }
              if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY - 1)] == 0) { visited += 1; }
              if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY - 1)] == 0) { visited += 1; }


              if (visited <= 1) {

                  mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;

               if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY + 1)] != 0)
                        { mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY + 1)] = 1; }

                  if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY + 1)] != 0)
                        { mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY + 1)] = 1; }

                  if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY - 1)] != 0)
                        { mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY - 1)] = 1; }

                  if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY - 1)] != 0)
                         { mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY - 1)] = 1; }

      */


			// remove wall from list
			wallList.Remove(wallList[wall]);
		}
	}

	public void ResizeMap()
	{
		//throw new System.Exception("This is the error message");
		// height/width limits: Must be odd. (divisible by 2.) Min. 4x4
		if (Mathf.FloorToInt(mapHeight / 2) * 2 == mapHeight) {
			mapHeight += 1;
		}
		if (Mathf.FloorToInt(mapWidth / 2) * 2 == mapWidth) {
			mapWidth += 1;
		}
		if (mapHeight < 7) {
			mapHeight = 7;
		}
		if (mapWidth < 7) {
			mapWidth = 7;
		}


	}
	public void DebugMap() {

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				mapString += mapArray[x, y];
			}

			mapString += System.Environment.NewLine; //"\n"
			//https://stackoverflow.com/questions/12826760/printing-2d-array-in-matrix-format
		}
		Debug.Log(mapString);

	}


}
