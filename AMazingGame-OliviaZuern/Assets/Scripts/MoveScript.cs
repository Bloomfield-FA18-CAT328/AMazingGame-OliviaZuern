using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	//current tile location.
	public int tileX;
	public int tileY;

	public int mazeY;
//	public int mazeX;

	private int direction;
	// 0=up, 1=r, 2=down, 3=l

		// same as above, but 5 = no input. 
	private int keyInput;


	private bool isMoving = false;
	// is it moving?

	private bool aligned;
	// is aligned to grid? (this is for conviniance, mostly)

	private MazeScript map;
	private GameControllerScript gM;

	private int step;

	private int Stuckfix;
	void Start () {
		gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		map = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().maze;
		// pick random tile, spawn player. or add this to maze gen. 
		mazeY = map.mapHeight;
		InvokeRepeating("Move", 0, 0.1f);
	}
	void Update()
	{
		for (int e = 0; e < gM.enemyPos.Length; e++) {
			if (gM.enemyPos[e] == gM.playerPos) { gM.RecreateMaze(); } 
		}
		// state / direction changer. check if aligned. X

		// set tile x and y. this is the current tile. 
		tileX = Mathf.RoundToInt(gameObject.transform.position.x);
		tileY = Mathf.Abs(Mathf.RoundToInt(gameObject.transform.position.z));
		gM.playerPos = new Vector2(tileX, tileY);

		//is aligned to grid
		if (gameObject.transform.position.x == Mathf.Floor(gameObject.transform.position.x) && gameObject.transform.position.z == Mathf.Floor(gameObject.transform.position.z)) {
			aligned = true;
		} else {
			aligned = false;
		}
		

		// input value
		if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0) {
			//keyInput = 2 + Mathf.RoundToInt(Input.GetAxis("Horizontal"));
			if(Input.GetAxisRaw("Horizontal") == 1) { keyInput = 3; } else { keyInput = 1; }
		}
		if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") != 0) {
			//keyInput = 1 + Mathf.RoundToInt(Input.GetAxis("Vertical"));
			if (Input.GetAxisRaw("Vertical") == 1) { keyInput = 0; } else { keyInput = 2; }
		}
		if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
			keyInput = 5;
		}


		// if is aligned and axys = 0, set not moving.
		if (aligned && keyInput == 5) {
			isMoving = false;
		}

		//if is aligned and axys isn't 0 check walls in that direction.
		// if no walls, set direction. move = true. 
		if (aligned && keyInput != 5) {
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
			if (map.WallCheck(dirX, dirY) == true) {
				isMoving = true;
				direction = keyInput;
			}
			//if(map.WallCheck(keyInput, tileX, tileY)) {

		}
	}
	// Update is called once per frame
	void Move () {

		// if is moving,  

		if (isMoving == true) {
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
			}
		} else { step = 0; }
	}

}
