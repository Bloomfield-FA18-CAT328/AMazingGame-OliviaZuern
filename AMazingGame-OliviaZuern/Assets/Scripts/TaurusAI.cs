using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaurusAI : MonoBehaviour {

	private EnemyMoveScript mover;
	private int behind;

	private byte targetX;
	private byte targetY;

	private int xx;

	private MazeScript map;

	private int tileX;
	private int TileY;
	private void Start()
	{
		mover = gameObject.GetComponent<EnemyMoveScript>();
		behind = 4;
		map = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().maze;
	}

	// Update is called once per frame
	void Update()
	{
		if (mover.isMoving == false) {
			xx = 0;
			DumbDirection(mover.direction);
		}
	}
	private void DumbDirection(int x)
	{


		targetX = (byte)Mathf.RoundToInt(gameObject.transform.position.x);
		targetY = (byte)Mathf.Abs(Mathf.RoundToInt(gameObject.transform.position.z));



		switch (x) {
			case 0:
				targetY += 1;
				break;
			case 1:
				targetX += 1;
				break;
			case 2:
				targetY -= 1;
				break;
			case 3:
				targetX -= 1;
				break;
		}

		if (map.IsClear(targetX, targetY) == true && x != behind) {
			mover.keyInput = x;
			behind = mover.keyInput + 2;
			if (behind >= 4) { behind -= 4; }

		} else {

			if (x < 3) {
				x += 1;
			} else {
				x = 0;
			}

			xx += 1;
			if (xx < 4) {
				DumbDirection(x);
			} else {
				DumbBack(behind);
			}

		}

	}

	private void DumbBack(int x)
	{

		mover.keyInput = x;
		behind = mover.keyInput + 2;
		if (behind >= 4) { behind -= 4; }
	}


}
