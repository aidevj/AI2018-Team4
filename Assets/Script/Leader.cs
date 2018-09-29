using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour {

    protected Vector3 desired;
    protected Vector3 temp;
    protected Vector3 steer;
    public Vector3 velocity;
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
        
        followPoint = (followDist * Vector3.Normalize(-velocity)) + transform.position; // Get the point behind the leader for the flockers to follow
        Debug.Log(FollowPoint);
    }
}
