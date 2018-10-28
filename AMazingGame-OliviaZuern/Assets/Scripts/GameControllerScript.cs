﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//For MonoBehaviours AND all other serialized things (ScriptableObjects, Editors, etc.) 
//DONT use the constructor!!! Use Awake, Start, or OnEnable
public class GameControllerScript : MonoBehaviour {
	//Any fields that are public, or marked [SerializeField] will be serialized
	//All simple types are serializable, as well as all of Unity's types or [System.Serializable]
	//Null is not supported by serialization in Unity
	public MazeScript maze;

	public GameObject cube;
	
	[SerializeField] private byte width = 5;
	[SerializeField] private byte height = 10;

	private GameObject cam;
	private GameObject floor;

	private GameObject genBTN;
	private GameObject inputX;
	private GameObject inputY;

	public GameObject Player;
	private int cubeCount;
	//public Button genBtn;

	private List<Vector2> wallclear;
	//private MazeScript maze;
	// Use this for initialization
	void Start () {
	
		Button genBtn = GameObject.FindGameObjectWithTag("GenBTN").GetComponent<Button>();
		genBtn.onClick.AddListener(RecreateMaze);
		
		

		cam = GameObject.FindGameObjectWithTag("MainCamera");
	//	cube = GameObject.FindGameObjectWithTag("Cube");
		
		floor = GameObject.FindGameObjectWithTag("Floor");

		// maze =  GetComponent<MazeScript>();
		//maze.MazeGen();
		RecreateMaze();
	}
	
	private void RecreateMaze()
	{
		Destroy(GameObject.FindGameObjectWithTag("Player"));
		InputField inpX = GameObject.FindGameObjectWithTag("X").GetComponent<InputField>();
		InputField inpY = GameObject.FindGameObjectWithTag("Y").GetComponent<InputField>();

		//clear maze
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
		for (int c = 0; c < cubes.Length; c++) {
			GameObject.Destroy(cubes[c]);
		}

		//cube = GameObject.FindGameObjectWithTag("Cube");
		//cube.transform.position = new Vector3(3, -1, 3);
		
		// max limit
		int w = 0;
		int h = 0;
		int.TryParse(inpX.text, out w);
		int.TryParse(inpY.text, out h);
		if (w > 255) { w = 255; }
		if (h > 255) { h = 255; }
		width = (byte)w;
		height = (byte)h;

	
		// actually generating maze.
		maze = new MazeScript(width, height);
		WallRemove();
		
		width = maze.mapWidth;
		height = maze.mapHeight;

		inpX.text = "" + width;
		inpY.text = "" + height;


		for (int y = 0; y < maze.mapHeight; y++) {
			for (int x = 0; x < maze.mapWidth; x++) {
				maze.mapString += maze.mapArray[x, y];
			}

			 maze.mapString += System.Environment.NewLine; //"\n"
													 //https://stackoverflow.com/questions/12826760/printing-2d-array-in-matrix-format
		}
		Debug.Log(maze.mapString);



	floor.transform.localScale = new Vector3(maze.mapWidth, 1, maze.mapHeight);
		floor.transform.position = new Vector3(maze.mapWidth / 2, -0.5f, -maze.mapHeight / 2);

		cam.transform.position = new Vector3(maze.mapWidth / 2, 20, -maze.mapHeight / 2);
		//cam.transform.position = new Vector3(maze.mapWidth / 2, maze.mapHeight, ((maze.mapHeight / 2) - (maze.mapHeight*(Mathf.Tan(15*Mathf.Deg2Rad)*Mathf.Rad2Deg)) ) );
		//cam.transform.position = new Vector3(maze.mapWidth / 2, maze.mapHeight, ((maze.mapHeight / 2) - (maze.mapHeight/5) ));
		if (maze.mapHeight > maze.mapWidth) {
			cam.GetComponent<Camera>().orthographicSize = (maze.mapHeight / 2) + 1;
		} else {
			cam.GetComponent<Camera>().orthographicSize = (maze.mapWidth / 2) + 1;
		}


		cubeCount = 0;
		for (int y = 0; y < maze.mapHeight; y++) {
			for (int x = 0; x < maze.mapWidth; x++) {
				if (maze.mapArray[x,y] != 0) {
					Instantiate(cube, new Vector3(x, 0.5f,- y), Quaternion.identity);
					cubeCount += 1;
				}

			}
		}

		
		if( cubeCount >= (maze.mapWidth * maze.mapHeight)-5) {
			RecreateMaze();
			Debug.Log ("FullMaze Fixed");
		}
		SpawnPlayer();

	}
	private void SpawnPlayer()
	{
		GameObject[] plist = GameObject.FindGameObjectsWithTag("Player");
		for (int c = 0; c < plist.Length; c++) {
			GameObject.Destroy(plist[c]);
		}

		byte spawnX =(byte) Mathf.FloorToInt( Random.Range(1, maze.mapWidth));
		byte spawnY = (byte) Mathf.FloorToInt( Random.Range(1, maze.mapHeight));
		
		if (maze.mapArray[spawnX, spawnY] == 0) {
		//	Debug.Log(""+spawnX+ spawnY);
			Instantiate(Player, new Vector3(spawnX, 0.5f, -spawnY), Quaternion.identity);
		} else { SpawnPlayer(); }
	}


	public void WallRemove()
	{
		// removes a few walls here and there. 
		wallclear = new List<Vector2>();
		for (int y = 1; y < maze.mapHeight - 1; y++) {
			for (int x = 1; x < maze.mapWidth - 1; x++) {
				if (maze.mapArray[x, y] != 0) {
					wallclear.Insert(0, new Vector2(x, y));
				}
			}
		}
		int toClear = (int)((Mathf.Sqrt(maze.mapHeight - 2) * Mathf.Sqrt(maze.mapWidth - 2))/1.75f);
		Debug.Log(toClear + "," + wallclear.Count);
		while (toClear > 0 && wallclear.Count > 0) {
			int w = (int)Random.Range(0, wallclear.Count);
			byte cellX = (byte)wallclear[w].x;
			byte cellY = (byte)wallclear[w].y;
			if ((maze.mapArray[cellX + 1, cellY] == 0 && maze.mapArray[cellX - 1, cellY] == 0 && maze.mapArray[cellX, cellY + 1] != 0 && maze.mapArray[cellX, cellY - 1] != 0) || (maze.mapArray[cellX, cellY + 1] == 0 && maze.mapArray[cellX, cellY - 1] == 0 && maze.mapArray[cellX + 1, cellY] != 0 && maze.mapArray[cellX - 1, cellY] != 0)) {
				maze.mapArray[cellX, cellY] = 0;
				toClear -= 1;
				Debug.Log("X");
			}
			wallclear.RemoveAt(w);
		}
	}

}
