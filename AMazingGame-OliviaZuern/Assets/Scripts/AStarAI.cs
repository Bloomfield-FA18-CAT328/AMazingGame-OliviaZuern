using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAI : MonoBehaviour {
	#region Static Section
	private static readonly Vector2Int[] AdjacentDirections = new Vector2Int[] {
		new Vector2Int(0, 1),
		new Vector2Int(1, 0),
		new Vector2Int(0, -1),
		new Vector2Int(-1, 0)
	};
	#endregion

	public Color col;
	//public GameObject prefabNode;

	private MazeScript maze;
	private EnemyMoveScript mover;
	private GameControllerScript gM;

	private int loopCount; // eternaal loop prevention, for testing purposes. Compare to total maze size.

	private byte targetX;
	private byte targetY;

	public NodeScript current;
	private NodeScript misc;

	private NodeScript[,] nodeMap;

	//already evaluated
	//private List<NodeScript> closed;
	private List<Vector2> closed;

	//not evaluated
	private List<NodeScript> openList;
	private List<Vector2> openXY;

	private List<NodeScript> children;

	public List<Vector2> pathNodes;

	void Start() {
		mover = gameObject.GetComponent<EnemyMoveScript>();
		gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
		mover.aS = true;
		maze = gM.maze;

		//Calc path to player. 
		//InvokeRepeating("AStar", 0, 0.5f);

		mover.direction = 7;
		AStar();
	}



	private void AStar() { // calc new path
		loopCount = 0;
		openList = new List<NodeScript>();
		openXY = new List<Vector2>();
		closed = new List<Vector2>();
		nodeMap = new NodeScript[maze.mapWidth, maze.mapHeight];
		targetX = (byte)gM.playerPos.x;
		targetY = (byte)gM.playerPos.y;

		int hereX = Mathf.RoundToInt(gameObject.transform.position.x);
		int hereY = Mathf.Abs(Mathf.RoundToInt(gameObject.transform.position.z));
		int miscH = (int)((Mathf.Pow(Mathf.Abs(targetX - hereX), 2f)) + (Mathf.Pow(Mathf.Abs(targetY - hereY), 2)));
		int deltaX = targetX - hereX;
		int deltaY = targetY - hereY;

		//add start to list
		current = new NodeScript(0, (deltaX * deltaX) + (deltaY * deltaY), hereX, hereY);
		openList.Add(current);
		openXY.Add(current.XY);
		nodeMap[(int)current.XY.x, (int)current.XY.y] = current;

		while (openList.Count > 0 && loopCount<=(maze.mapHeight*maze.mapWidth)) {
			loopCount++;
			//find Current
			int indexOfLowest = GetIndexOfLowestCost(openList);
			current = openList[indexOfLowest];

			//move to closed
			closed.Add(openList[indexOfLowest].XY);
			openList.RemoveAt(indexOfLowest);
			openXY.RemoveAt(indexOfLowest);


			//if current = goal, backtrack from goal.
			if (current.XY == new Vector2(targetX, targetY)) {
			//	int miscPC = nodeMap[(int)current.XY.x, (int)current.XY.y].g; //Path cost

				misc = new NodeScript(current.g, 0, targetX, targetY);
				misc.parent = current.XY;
				nodeMap[(int)current.XY.x, (int)current.XY.y] = current;
				openList.Clear();
				FindPath();
				return;
			} else {


				// Check and gen. children
				for (int dir = 0; dir <= 3; dir++) {
					//if within maze size (this was a problem, not sure how exactly) //Actually..
					if ((current.XY.x + AdjacentDirections[dir].x) <= maze.mapWidth - 1 && (current.XY.y + AdjacentDirections[dir].y) <= maze.mapWidth - 1 && (current.XY.x + AdjacentDirections[dir].x) >= 0 && (current.XY.y + AdjacentDirections[dir].y) >= 0) {
						

						// if not on closed and not a wall
						if (maze.IsClear((byte)(current.XY.x + AdjacentDirections[dir].x), (byte)(current.XY.y + AdjacentDirections[dir].y)) == true && !closed.Contains(new Vector2((byte)(current.XY.x + AdjacentDirections[dir].x), (byte)(current.XY.y + AdjacentDirections[dir].y)))) {

							int miscPC = nodeMap[(int)current.XY.x, (int)current.XY.y].pathCost;
							miscH = (int)((Mathf.Pow(Mathf.Abs(targetX - (current.XY.x + AdjacentDirections[dir].x)), 2f)) + (Mathf.Pow(Mathf.Abs(targetY - (current.XY.y + AdjacentDirections[dir].y)), 2)));
							misc = new NodeScript(miscPC, miscH, (int)(current.XY.x + AdjacentDirections[dir].x), (int)(current.XY.y + AdjacentDirections[dir].y));
							misc.parent = current.XY;

							//if on openlist
							if (openXY.Contains(new Vector2(current.XY.x + AdjacentDirections[dir].x, current.XY.y + AdjacentDirections[dir].y))) {
								indexOfLowest = openXY.IndexOf(new Vector2(current.XY.x + AdjacentDirections[dir].x, current.XY.y + AdjacentDirections[dir].y));


								//if path is better, use that instead. 
								if (openList[indexOfLowest].g >= misc.g) {
									openList[indexOfLowest] = misc;
									nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;
								}// else { Debug.Log("Nope"); }

							} else {
								//add to list
								openList.Add(misc);
								openXY.Add(misc.XY);
								nodeMap[(int)misc.XY.x, (int)misc.XY.y] = misc;

							}
						}
					} //else { throw new System.Exception("Why!~?!?" + current.XY); }
				}
			}
		}
	}

	private int GetIndexOfLowestCost(List<NodeScript> list) {
		int indexOfLowest = 0;
		for (int i = 1; i < list.Count; i++) {
			if (list[i].f < list[indexOfLowest].f) {
				indexOfLowest = i;
			}
		}
		return indexOfLowest;
	}

	private void FindPath() {
		Vector2 here = new Vector2(targetX, targetY);
		pathNodes = new List<Vector2>();
		pathNodes.Insert(0, here);

		while (here != new Vector2(mover.tileX, mover.tileY)) {
			here = nodeMap[(int)here.x, (int)here.y].parent;
			pathNodes.Insert(0, here);
		}
		pathNodes.Remove(new Vector2(0, 0));
		//pathNodes.RemoveAt(0);// remove first step (starting point.)
	}

	public void NextDirection(int x, int y) {
		AStar();
		Vector2 calc = pathNodes[0] - pathNodes[1];
		//Vector2 calc = new Vector2(x,y) - pathNodes[0];
		if (calc == new Vector2(0f, -1f)) {
			mover.direction = 0;
		}

		if (calc == new Vector2(1f, 0f)) {
			mover.direction = 3;
		}

		if (calc == new Vector2(0f, 1f)) {
			mover.direction = 2;
		}

		if (calc == new Vector2(-1f, 0f)) {
			mover.direction = 1;
		}
		pathNodes.RemoveAt(0);
		
	}


		private void OnDrawGizmos() { DrawRays(); }
		private void OnDrawGizmosSelected() { DrawRays(); }

		void DrawRays()
		{
			Gizmos.color = col;
			//float hereX = transform.position.x;
			//float hereY = transform.position.z;
			
			Gizmos.DrawLine(transform.position, new Vector3(pathNodes[0].x, 0.5f, -pathNodes[0].y));
			
			for (int i = 0; i < pathNodes.Count-1; i++) {
				Gizmos.DrawLine(new Vector3(pathNodes[i].x, 0.5f, -pathNodes[i].y), new Vector3(pathNodes[i + 1].x, 0.5f, -pathNodes[i + 1].y));
			}
			Gizmos.DrawLine(new Vector3(pathNodes[pathNodes.Count-1].x, 0.5f, -pathNodes[pathNodes.Count - 1].y), new Vector3(gM.playerPos.x,0.5f,-gM.playerPos.y));
			//Gizmos.color = Color.red;
			//Gizmos.DrawLine(new Vector3(hereX, 0.5f, hereY), new Vector3(gM.playerPos.x, 0.5f, -gM.playerPos.y));
	}
		


}
