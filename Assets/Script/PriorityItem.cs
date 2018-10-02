using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item in the priority queue used for A*
public class PriorityItem : MonoBehaviour {

    private float distanceFromStart;
    private float heuristic; // estimated total distance to the endpoint
    private Dictionary<PriorityItem, float> neighborNodes = new Dictionary<PriorityItem, float>(); // nodes neighboring this one and their distance
    
	void Start () {
        distanceFromStart = 0;
        heuristic = 0;
	}

}
