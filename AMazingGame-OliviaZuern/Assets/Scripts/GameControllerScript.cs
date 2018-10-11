using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public MazeScript maze = new MazeScript();
    //private MazeScript maze;
    // Use this for initialization
    void Start () {
       
     // maze =  GetComponent<MazeScript>();
       
        maze.MazeGen();

        maze.DebugMap();
	}
	

}
