﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript {
	public Vector2 XY; // x&y 
	/// </summary>
	public Vector2 parent; //parent coordonate of this object.
	public int pathCost;

	public int f; //f = g+h
	public int g;
	public int h;
	public NodeScript(int b,int c, int t, int u) {
		g = b+1;
		h = c;
		f = h + g;
		XY = new Vector2(t, u);
	//	pathCost = pc + 1;
	//	Debug.Log("" + f + "=" + g + "+" + h + " " + XY);
	}
}
