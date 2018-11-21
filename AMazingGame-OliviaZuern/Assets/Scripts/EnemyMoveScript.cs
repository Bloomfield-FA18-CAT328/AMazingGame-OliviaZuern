using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour {
	private DirectionDebugScript draw;

	//current tile location.
	public int tileX;
	public int tileY;

	public int mazeY;
	//	public int mazeX;

	public int direction;
	// 0=up, 1=r, 2=down, 3=l

	// same as above, but 5 = no input. 
	public int keyInput;


	public bool isMoving = false;
	// is it moving?

	public bool aligned;
	// is aligned to grid? (this is for conviniance, mostly)

	private MazeScript map;
	public GameControllerScript gM;

	private int step;
	private Vector2 prevPos;
	public int enemyID;

	public bool aS = false;

	void Start()
	{
		gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		enemyID = gM.enemyCount;
		draw = gameObject.GetComponent<DirectionDebugScript>();
		keyInput = 5;
		map = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().maze;
		
		mazeY = map.mapHeight;
		InvokeRepeating("Move", 0, 0.1f);
	}
	void Update()
	{
		// state / direction changer. check if aligned. X

		// set tile x and y. this is the current tile. 
		tileX = Mathf.RoundToInt(gameObject.transform.position.x);
		tileY = Mathf.Abs(Mathf.RoundToInt(gameObject.transform.position.z));
		

		//is aligned to grid
		if (gameObject.transform.position.x == Mathf.Floor(gameObject.transform.position.x) && gameObject.transform.position.z == Mathf.Floor(gameObject.transform.position.z)) {
			aligned = true;
		} else {
			aligned = false;
		}

		

		// if is aligned and axys = 0, set not moving.
		if (aligned && keyInput == 5) {
			isMoving = false;
		}

		//if is aligned and axys isn't 0 check walls in that direction.
		// if no walls, set direction. move = true. 
		if (aligned && keyInput != 5) {
			TileCheck();
		}
	}
	// Update is called once per frame
	void Move()
	{
		if(prevPos == new Vector2(gameObject.transform.position.x, gameObject.transform.position.z)) {
			isMoving = true;
		}

		// if is moving,  

		if (isMoving == true) {
			gM.enemyPos[enemyID] = new Vector2(tileX, tileY);
			draw.x = direction;
			prevPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
			step += 1;
			switch (direction) {
				case 0:
					//gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.5f, (step * 0.1f) - gameObject.transform.position.y);
					gameObject.transform.Translate(new Vector3(0, 0, -0.2f));

					break;
				case 1:

					//gameObject.transform.position = new Vector3((step * 0.1f) + gameObject.transform.position.x, 0.5f,  -gameObject.transform.position.y);
					gameObject.transform.Translate(new Vector3(0.2f, 0, 0));

					break;
				case 2:

					//gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.5f, -(step * 0.1f)- gameObject.transform.position.y);
					gameObject.transform.Translate(new Vector3(0, 0, 0.2f));

					break;
				case 3:

					//gameObject.transform.position = new Vector3(-(step * 0.1f) + gameObject.transform.position.x, 0.05f, -gameObject.transform.position.y);
					gameObject.transform.Translate(new Vector3(-0.2f, 0, 0));

					break;
			}

			

			if (step == 5) {
				step = 0;
				isMoving = false;
				gameObject.transform.position = new Vector3(Mathf.Round(gameObject.transform.position.x), 0.5f, Mathf.Round(gameObject.transform.position.z));
			
				isMoving = false;

				if (aS == true) {gameObject.GetComponent<AStarAI>().NextDirection(tileX, tileY);}
			}

		} else {
			step = 0;
			prevPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
		}
	}

	public void TileCheck() {
		byte dirX = (byte)tileX;
		byte dirY = (byte)tileY;
		switch (keyInput) {
			case 0:
				dirY += 1;
				break;
			case 1:
				dirX += 1;
				break;
			case 2:
				dirY -= 1;
				break;
			case 3:
				dirX -= 1;
				break;
		}
		if (map.IsClear(dirX, dirY) == true) {
			isMoving = true;
			direction = keyInput;
		}

	}
}

