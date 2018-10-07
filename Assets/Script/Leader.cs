using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour {

    protected Vector3 desired;
    protected Vector3 temp;
    protected Vector3 steer;
    public Vector3 velocity;
    private List<GameObject> path;
    private GameObject heading;
    private Vector3 direction;
	private int node_index;
	[SerializeField] private Material walkeable;
    public int mapSideLocation = 0;    // default side is side 0, other is 1


    public Vector3 Velocity
    {
        get { return velocity; }
    }
    public Vector3 acceleration;
    public float maxSpeed;
    private Vector3 followPoint; // Point behind the leader that flockers follow
    public Vector3 FollowPoint
    {
        get { return followPoint; }
    }
    public float followDist; // Distance behind the leader the point is at


    // Use this for initialization
    void Start ()
    {
        acceleration = Vector3.zero;

    }
	
	// Update is called once per frame
	void Update () {
		if (heading != null)
		{
			// if there  is node to heading
			transform.position = Vector3.MoveTowards (transform.position, heading.transform.position, 3.0f * Time.deltaTime);
			// set the speed to 3f for now, change if need.
			// set speed , move to heading direction
			if (Vector3.Distance(transform.position, heading.transform.position) <= 2f)
			{
				// if arrived the heading node
				var render = heading.GetComponent<MeshRenderer>();
				render.material = walkeable; 
				// change the color of cube back to yellow
				if (node_index > path.Count - 1)
				{
					heading = null; // end of path
					node_index = 1;
					// path finding finish
				}
				else
				{
					// heading to next node
					heading = path[node_index];
					node_index++;
				}
			}
		}
		
	    followPoint = (followDist * Vector3.Normalize(-velocity)) + transform.position; // Get the point behind the leader for the flockers to follow
        //Debug.Log(FollowPoint);
    }

    public void setpath(List<GameObject> _path)
    {
	    node_index = 1;
        path = _path;
	    heading = path[0];
    }
}
