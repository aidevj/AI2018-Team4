using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public enum Team : int
{
	None,
	Red,
	Green
}

public class Node : MonoBehaviour
{

	public bool walkable;
	public Vector3 worldPosition;

	public int x;
	public int y;
	public int g;  // the cost of the path from the start node to n
	public int h;  // a heuristic function that estimates the cost of the cheapest path from n to the goal 
	public Node parent;
    public int containedUnit = 0;
	[SerializeField]private Color none;
	[SerializeField]private Color red;
	[SerializeField]private Color green;
	public Team currentTeam;

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

	public void Refresh()
	{
		var temp = GetComponentInChildren<TextMesh>();
		temp.text = containedUnit.ToString();
		switch (currentTeam)
		{
				case Team.None:
					GetComponent<MeshRenderer>().material.color = none;
					break;
				case Team.Green:
					var colorG = new Color(green.r,green.g,green.b,containedUnit*0.1f);
					GetComponent<MeshRenderer>().material.color = colorG;
					break;
			    case Team.Red:
				    var colorR = new Color(red.r,red.g,red.b,containedUnit*0.1f);
				    GetComponent<MeshRenderer>().material.color = colorR;
				    break;
		}
	}
	

	public void addInfluence(int cur, Team curTeam)
	{
		
		if (currentTeam != curTeam)
		{
			containedUnit -= cur;
		}else{
			containedUnit += cur;
		}

		if (containedUnit > 0)
		{
			
		}
		else if(containedUnit < 0 )
		{
			containedUnit = Mathf.Abs(containedUnit);
			currentTeam = curTeam;
			// stay the same
		}
		else
		{
			currentTeam = Team.None;
		}
		Refresh();
	}
}
