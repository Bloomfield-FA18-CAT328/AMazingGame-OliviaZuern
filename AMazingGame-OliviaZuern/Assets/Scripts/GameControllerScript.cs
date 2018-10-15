using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For MonoBehaviours AND all other serialized things (ScriptableObjects, Editors, etc.) 
//DONT use the constructor!!! Use Awake, Start, or OnEnable
public class GameControllerScript : MonoBehaviour {
	//Any fields that are public, or marked [SerializeField] will be serialized
	//All simple types are serializable, as well as all of Unity's types or [System.Serializable]
	//Null is not supported by serialization in Unity
	public MazeScript maze;

	[SerializeField] private byte width = 5;
	[SerializeField] private byte height = 10;

	//[SerializeField] private byte previousWidth;
	//[SerializeField] private byte previousHeight;

	//public void OnValidate()
	/*{

		if (width != previousWidth || height != previousHeight) {
			RecreateMaze();
		}
		previousWidth = width;
		previousHeight = height;
	}*/

	//private MazeScript maze;
	// Use this for initialization
	void Start () {

		// maze =  GetComponent<MazeScript>();
		//maze.MazeGen();
		RecreateMaze();
	}
	
	private void RecreateMaze()
	{
		maze = new MazeScript(width, height);

		maze.DebugMap();
	}

}
