using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class AStar : MonoBehaviour {

    // needs to hold seeker script
	[SerializeField] private GridGenerator grid;
	[SerializeField] private Material highlight;
	
    private PriorityItem nodeToSeek;
    private MeshRenderer[] nodeRenderedPath;
    //private List<PriorityItem> pathList;
	private int diagonal_cost = 14;
	private int herizontal_cost = 10;// the vertical cost is the same.

	
	void Start ()
	{
	}

	void Update()
	{
		
	}

	public List<GameObject> Pathfind(Vector3 startPos, Vector3 endPos)
	{
		grid.wipePath();
		// to split the path if it need across the portal
		// final path = path before portal + portal to endNode.
	   var startNode = grid.GetNodeFromPosition(startPos).GetComponent<Node>();
	   var endNode = grid.GetNodeFromPosition(endPos).GetComponent<Node>();
		if (startNode.x >= 26 && endNode.x <= 20)
		{
			Node portal_a = grid.GetNodeByCoor(20,20).GetComponent<Node>();
			Node portal_b = grid.GetNodeByCoor(26, 25).GetComponent<Node>();
			A_star(startNode,portal_b);
			A_star(portal_a,endNode);
			portal_a.parent = portal_b;
		}else if (endNode.x >= 26 && startNode.x <= 20)
		{
			Node portal_a = grid.GetNodeByCoor(20,20).GetComponent<Node>();
			Node portal_b = grid.GetNodeByCoor(26, 25).GetComponent<Node>();
			A_star(startNode,portal_a);
			A_star(portal_b,endNode);
			portal_b.parent = portal_a;
		}
		else
		{
			A_star(startNode,endNode);
		}
		return generatePath(endNode);
	}

	public void A_star(Node startNode, Node endNode)
	{
		// A* implementation 
		PriorityQueue openList = new PriorityQueue(); // auto-sort, hold for the nodes need to check
		HashSet<Node> closedList = new HashSet<Node>(); // fast check contain, hold the nodes that finished check
		openList.Add(startNode);// add start point into openlist
		startNode.h = computeDistance(startNode, endNode);
		startNode.g = 0; // start point
		while (openList.Count() > 0)
		{
			Node currentNode = openList.Pop();
			if (currentNode == endNode)
			{
				// path found
				path_Highlight(endNode);
				openList =  new PriorityQueue();
				break;
			}
			closedList.Add(currentNode);
			foreach (GameObject neighbour in grid.GetNodeNeighbours(currentNode))
			{
				Node neighbour_node = neighbour.GetComponent<Node>();
				if (closedList.Contains(neighbour_node))
				{
					// no need for un-walkable for check, done that in generating neighbour list.
					continue;
				}
				int updated_move_cost = computeDistance(currentNode, neighbour_node);
				updated_move_cost += currentNode.g;
				if (!openList.Contains(neighbour_node)|| neighbour_node.g >updated_move_cost )
				{
					// found node
					neighbour_node.parent = currentNode; // update the parent node to current
					neighbour_node.h = computeDistance(neighbour_node, endNode);  // calculate the estimate distance 
					neighbour_node.g = updated_move_cost;  // update the actual cost to new cost, keep recording smallest
					if (!openList.Contains(neighbour_node))
					{
						openList.Add(neighbour_node);
					}
				}
			}
		}
		if (endNode.parent == null)
		{
			Debug.Log("the path find algo failed.");
		}
	}

	public void path_Highlight(Node endNode)
	{
		Node temp = endNode;
		while (temp.parent != null)
		{
			GameObject node = grid.GetNodeByCoor(temp.x,temp.y);
			node.GetComponent<MeshRenderer>().material = highlight;
			temp = temp.parent;
		}
	}

	private List<GameObject> generatePath(Node endNode)
	{
		Node temp = endNode;
		List<GameObject> path = new List<GameObject>();
		while (temp.parent != null)
		{
			path.Add(grid.GetNodeByCoor(temp.x,temp.y));
			temp = temp.parent;
		}
		path.Reverse();
		return path;
	}

	private int computeDistance(Node start, Node end)
    {
	    // calculate the distance bewtween two node
	    int distance_x = Mathf.Abs(start.x - end.x);
	    int distance_y = Mathf.Abs(start.y - end.y);
	    if (distance_x > distance_y)
	    {
		return distance_y * diagonal_cost + (distance_x - distance_y) * herizontal_cost;
	    }
		return distance_x * diagonal_cost + (distance_y - distance_x) * herizontal_cost;

    }

	/// <summary>
    /// Runs A* Algorithm
    /// </summary>
//    List<PriorityItem> Pathfind(PriorityItem startPoint, PriorityItem goal)
//    {
//    }
}
