using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Node : MonoBehaviour
{

	public bool walkable;
	public Vector3 worldPosition;

	public int x;
	public int y;
	public int g;  // the cost of the path from the start node to n
	public int h;  // a heuristic function that estimates the cost of the cheapest path from n to the goal 
	public Node parent;

	public void setNode(bool _walkable, Vector3 _worldPosition, int _index_x, int _index_y)
	{
		// set the 	Node position and walkable bool during grid generating
		walkable = _walkable;
		worldPosition = _worldPosition;
		x = _index_x;
		y = _index_y;
	}

	public int f
	{
		// total cost by g  +  h
		get { return g + h; }
	}
}
