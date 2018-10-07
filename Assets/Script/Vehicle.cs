using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script contains operations and variables that all the scene's vehicles share

// The vehicle class is a parent class for all vehicle objects
abstract public class Vehicle : MonoBehaviour
{
    protected GameManager gm; // Accessor for the GameManager script
    protected Vector3 acceleration; // Create the variables for movement (v,a,vd,F)
    protected Vector3 velocity;
    public Vector3 Velocity // Property so that vehicles can make decisions based on each others' velocity
    {
        get { return velocity; }
    }
    protected Vector3 desired;
    protected Vector3 steer;
    private Vector3 vecToCenter; // Vector for distance in obstacle avoidance calculation

    private Rigidbody rigidB;
    RaycastHit hitInfo; // Stores info on which object is hit
    bool rayHit; // Is it in front of a trigger?
    Ray ray; 
    int layerMask = 1 << 8; // Bit shift to only cast against layer 8
    Vector3[] sightlines; // Ray directions in front of vehicle

    public float maxSpeed; // Limiting variables, should be initialized in each vehicle child
    public float maxForce;
    public float radius;
    public float mass;
    protected bool autoRotate; // Variable to tell if vehicle should automatically rotate towards its direction of travel, set in child classes
    // No relevant gravitational force in the section of space the scene takes place in

    abstract protected void CalcSteeringForces(); // Require vehicle children to implement a steering method


    virtual public void Start()
    {
        acceleration = Vector3.zero; // No initial movement
        velocity = transform.forward;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get access to the GameManager script
        rigidB = this.gameObject.GetComponent<Rigidbody>();
    }

    protected void Update() // Update movement variables based on forces, called once per frame
    {
        CalcSteeringForces(); // Get the overall acceleration needed to do what the vehicle wants

        rigidB.velocity += acceleration * Time.deltaTime; // Apply the acceleration to the velocity, correcting for frame rate
        rigidB.velocity = Vector3.ClampMagnitude(rigidB.velocity, maxSpeed); // Limit the velocity
        velocity = rigidB.velocity;

        if (autoRotate && velocity != Vector3.zero) // This rotates the vehicle to face its new direction (if the vehicle should do this)
        {
            transform.forward = new Vector3(velocity.normalized.x, 0, velocity.normalized.y);
        }

        acceleration = Vector3.zero; // Reset acceleration when done for the frame
    }

    protected void ApplyForce(Vector3 steeringForce) // Takes in a force and applies it to the acceleration
    {
        acceleration += steeringForce / mass;
        acceleration.y = 0;
    }

    protected Vector3 Seek(Vector3 targetPos) // Returns a basic seeking force towards the inputted target
    {
        desired = targetPos - transform.position; // Get the vector between there and here
        desired.Normalize();
        desired = desired * maxSpeed; // Scale the vector to maxSpeed
        steer = desired - velocity; // Take the desired velocity we just calculated and subtract the current velocity
        return steer; // Return this calculated steering force
    }
    protected Vector3 AvoidObstacle() // Takes an obstacle and determines what force is needed to avoid it
    {
        sightlines = new Vector3[] { rigidB.velocity, Quaternion.AngleAxis(45, Vector3.up) * rigidB.velocity, Quaternion.AngleAxis(-45, Vector3.up) * rigidB.velocity };
        desired = Vector3.zero; // Reset desired velocity

        foreach (Vector3 line in sightlines)
        {
            ray = new Ray(transform.position, transform.forward); // Get position and direction of the camera
            rayHit = Physics.Raycast(ray, out hitInfo, 30, layerMask, QueryTriggerInteraction.Ignore); // Raycast against layer 8, 5 spaces in front of camera

            if (rayHit)
            {
                desired -= hitInfo.point - transform.position; // Avoid the obstacle
            }
        }

        Debug.DrawLine(transform.position, transform.position + (transform.forward * 5), Color.blue);


        return desired;
    }
}
