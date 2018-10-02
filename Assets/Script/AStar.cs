using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    // needs to hold seeker script

    [SerializeField] private PriorityItem nodeToSeek;
    private MeshRenderer[] nodeRenderedPath;
    private List<PriorityItem> pathList;


	void Start () {
		
	}
	
	void Update () {
		
	}

    /// <summary>
    /// Runs A* Algorithm
    /// </summary>
    List<PriorityItem> Pathfind(PriorityItem startPoint, PriorityItem goal)
    {
        return;
    }
}
