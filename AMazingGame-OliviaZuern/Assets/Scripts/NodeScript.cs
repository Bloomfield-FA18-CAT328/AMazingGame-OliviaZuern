using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript {
	public Vector2 XY; // x&y. LEAVE IT ALONE, CARLOS.
	/// </summary>
	public Vector2 parent; //parent node of this object.
	public int pathCost;

	public int f; //f = g+h
	public int g;
	public int h;
	//:/ </3
	public NodeScript(int g,int h, int cellX, int cellY) {
		this.g = g;
		this.h = h;
		f = this.h + this.g;
		XY = new Vector2(cellX, cellY);
		pathCost = g + 1;
	//	Debug.Log("" + f + "=" + g + "+" + h + " " + XY);
	}
}
