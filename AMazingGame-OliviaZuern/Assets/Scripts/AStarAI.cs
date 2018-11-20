using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAI : MonoBehaviour {
	public Color col;
	//public GameObject prefabNode;

	private MazeScript maze;
	private EnemyMoveScript mover;
	private GameControllerScript gM;
	

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
	private bool found;
	void Start () {
		mover = gameObject.GetComponent<EnemyMoveScript>();
		gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
	//	mover.aS = true;
		maze = gM.maze;

		//Calc path to player. 
		//InvokeRepeating("AStar", 0, 0.5f);
		
		dirCheck = new List<Vector2Int>();
		dirCheck.Add(new Vector2Int(0, 1));
		dirCheck.Add(new Vector2Int(1, 0));
		dirCheck.Add(new Vector2Int(0,-1));
		dirCheck.Add(new Vector2Int(-1,0));

		mover.direction = 7;
		AStar();
	}



	private void AStar(){ // calc new path
		found = false;
			open = new List<NodeScript>();
			openXY = new List<Vector2>();
			closed = new List<Vector2>();
			nodeMap = new NodeScript[maze.mapWidth, maze.mapHeight];
			targetX = (byte) gM.playerPos.x;
			targetY = (byte) gM.playerPos.y;

		int hereX = Mathf.RoundToInt(gameObject.transform.position.x);
		int hereY = Mathf.Abs(Mathf.RoundToInt(gameObject.transform.position.z));
		int miscH = (int)((Mathf.Pow(Mathf.Abs(targetX - hereX), 2f)) + (Mathf.Pow(Mathf.Abs(targetY - hereY), 2)));
		//Debug.Log(miscH);
			//add start to list
		current = new NodeScript(0,0, hereX, hereY);
			open.Add(current);
			openXY.Add(current.XY);
			nodeMap[(int)current.XY.x, (int)current.XY.y] = current;

			
			while(open.Count > 0 && found == false ) {
				//find Current
				int fcost = open[0].f;
				int list = 0;
				for (int y = 0; y <= open.Count-1; y++) {
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
				found = true;
				int miscPC = nodeMap[(int)current.XY.x, (int)current.XY.y].g;
				misc = new NodeScript(0, miscPC, (int)targetX, (int)targetY);
					misc.parent = current.XY;
					nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;
					open.Clear();
					FindPath();

				} else {

					// Check and gen. children
				for(int y = 0; y <= 3; y++) {
					Debug.Log(y);
					//if within maze size (this was a problem, not sure how exactly)
					if ((current.XY.x + dirCheck[y].x) <= maze.mapWidth - 1 && (current.XY.y + dirCheck[y].y) <= maze.mapWidth-1 && (current.XY.x + dirCheck[y].x) >= 0 && (current.XY.y + dirCheck[y].y) >= 0) {
						
						// if not on closed and not a wall
						if (maze.WallCheck((byte)(current.XY.x + dirCheck[y].x), (byte)(current.XY.y + dirCheck[y].y)) == true && !closed.Contains(new Vector2((byte)(current.XY.x + dirCheck[y].x), (byte)(current.XY.y + dirCheck[y].y)))) {

							int miscPC = nodeMap[(int)current.XY.x, (int)current.XY.y].pathCost;
							miscH = (int)((Mathf.Pow(Mathf.Abs(targetX - (current.XY.x + dirCheck[y].x)), 2f)) + (Mathf.Pow(Mathf.Abs(targetY - (current.XY.y + dirCheck[y].y)), 2)));
							misc = new NodeScript(miscPC,miscH, (int)(current.XY.x + dirCheck[y].x), (int)(current.XY.y + dirCheck[y].y));
							misc.parent = current.XY;

							//if on openlist
							if (openXY.Contains(new Vector2(current.XY.x + dirCheck[y].x, current.XY.y + dirCheck[y].y))) {
								list = openXY.IndexOf(new Vector2(current.XY.x + dirCheck[y].x, current.XY.y + dirCheck[y].y));
								//Debug.Log("list:" + list+ " size "+openXY.Count+" "+open.Count);
						
								//if path is better, use that instead. 
								if (open[list].g > misc.g) {
									open[list] = misc;
									nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;
								} else { Debug.Log("Nope"); }

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
