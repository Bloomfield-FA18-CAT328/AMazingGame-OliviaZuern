using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAI : MonoBehaviour {
	public Color col;
	public GameObject prefabNode;

	private EnemyMoveScript mover;
	private MazeScript maze;

	private byte targetX;
	private byte targetY;

	public NodeScript current;
	private NodeScript misc;

	private NodeScript[,] nodeMap;

	//already evaluated
	//private List<NodeScript> closed;
	private List<Vector2> closed;

	//not evaluated
	private List<NodeScript> open;
	private List<Vector2> openXY;

	private List<NodeScript> children;
	// Use this for initialization

	public List<Vector2> pathNodes;

	private List<Vector2Int> dirCheck;
	void Start () {
		mover = gameObject.GetComponent<EnemyMoveScript>();
		mover.aS = true;
		maze = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>().maze;

		//Calc path to player. 
		//InvokeRepeating("AStar", 0, 0.5f);
		AStar();
		dirCheck = new List<Vector2Int>();
		dirCheck.Add(new Vector2Int(0, 1));
		dirCheck.Add(new Vector2Int(0, -1));
		dirCheck.Add(new Vector2Int(-1, 0));
		dirCheck.Add(new Vector2Int(1, 0));

	}



	private void AStar(){ // calc new path
		
			open = new List<NodeScript>();
			openXY = new List<Vector2>();
			closed = new List<Vector2>();
			nodeMap = new NodeScript[maze.mapWidth, maze.mapHeight];
			targetX = (byte) mover.gM.playerPos.x;
			targetY = (byte)mover.gM.playerPos.y;

			//add start to list
			current = new NodeScript(mover.tileX, ((Mathf.Abs(targetX - mover.tileX) ^ 2) + (Mathf.Abs(targetY - mover.tileY) ^ 2)), mover.tileX, mover.tileY);
			open.Add(current);
			openXY.Add(current.XY);
			nodeMap[(int)current.XY.x, (int)current.XY.y] = current;

			
			while(open.Count > 0 ) {
				//find Current
				int fcost = 0;
				int list = 0;
				for (int y = 0; y < open.Count; y++) {
					if (open[y].f< fcost) {
						current = open[y];
						fcost = open[y].f;
						list = y;
					}

				}

				//move to closed
				//closed.Add(open[list]);
				closed.Add(open[list].XY);
				open.RemoveAt(list);
				openXY.RemoveAt(list);


				//if current = goal, backtrack to goal.
				if (current.XY == new Vector2(targetX, targetY)) {
					misc = new NodeScript(mover.tileX, (((targetX) ^ 2) + ((targetY) ^ 2)), (int)targetX, (int)targetY);
					misc.parent = current.XY;
					nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;
					open.Clear();
					FindPath();
				} else {

					// Check and gen. children
				for(int y = 0; y < 3; y++)
					if (maze.WallCheck((byte)(current.XY.x + dirCheck[y].x), (byte)(current.XY.y + dirCheck[y].y)) == true && !closed.Contains(new Vector2((byte)(current.XY.x + dirCheck[y].x), (byte)(current.XY.y + dirCheck[y].y)))) {
						misc = new NodeScript(mover.tileX, ((Mathf.Abs(targetX - (int)current.XY.x) ^ 2) + (Mathf.Abs(targetY - (int)current.XY.y) ^ 2)), (int)(current.XY.x + dirCheck[y].x), (int)(current.XY.y + dirCheck[y].y));
						misc.parent = current.XY;
						//if on openlist
						if (openXY.Contains(new Vector2(current.XY.x + dirCheck[y].x, current.XY.y + dirCheck[y].y))) {
							list = openXY.IndexOf(new Vector2(current.XY.x, current.XY.y + 1));

							//if path is better, use that instead. 
							if (open[list].g > misc.g) {
								open[list] = misc;
								nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;
							}

						} else {
							//add to list
							open.Add(misc);
							openXY.Add(misc.XY);
							nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;


						}


					}

				}

			}


		}

	private void FindPath()
	{
		Vector2 here = new Vector2(targetX, targetY);
		pathNodes = new List<Vector2>();
		pathNodes.Insert(0, here);

		while (here != new Vector2(mover.tileX, mover.tileY)) {
			here = nodeMap[(int)here.x, (int)here.y].parent;
			pathNodes.Insert(0, here);
		}
	}

	public void NextDirection()
	{
		Vector2 calc = pathNodes[0] - pathNodes[1];
		if (calc == new Vector2(0f, -1f)) {
			mover.direction = 0;
		}

		if (calc == new Vector2(1f, 0f)) {
			mover.direction = 1;
		}

		if (calc == new Vector2(0f, 1f)) {
			mover.direction = 2;
		}

		if (calc == new Vector2(-1f, 0f)) {
			mover.direction = 3;
		}
		pathNodes.RemoveAt(0);
	}


		/*	private void OnDrawGizmos() { DrawRays(); }
			private void OnDrawGizmosSelected() { DrawRays(); }

			void DrawRays()
			{
				float targetX = transform.position.x;
				float targetY = transform.position.z;
				switch (x) {
					case 0:
						targetY -= 1;
						break;
					case 1:
						targetX += 1;
						break;
					case 2:
						targetY += 1;
						break;
					case 3:
						targetX -= 1;
						break;
				}
				Gizmos.color = col;
				Gizmos.DrawLine(transform.position, new Vector3(targetX, gameObject.transform.position.y, targetY));


			}
			*/


	}
