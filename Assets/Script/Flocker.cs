using UnityEngine;
using System.Collections;

// This script contains variables and logic for flocking
// Implements seperation, cohesion, alignment, leader following

public class Flocker : Vehicle
{
    private Leader leaderScript; // Need to access leader's velocity
    public float followWeight; // Weights for leader following, alignment, cohesion, seperation, and obstacle avoidance
    public float alignWeight;
    public float cohereWeight;
    public float separateWeight;
    public float arrivalDist; // Radius and square for the arrival area around the follow point
    private float arrivalDistSq;
    private float desiredSeparation; // Separation variables
    private float sepDist; // I also use this one for arrival
    private Vector3 sPartial; // Vectors for separation
    private Vector3 sTotal;
    private Vector3 steeringForce; // Force for steering
    private Vector3 separateForce; // Force for separation
    public float avoidWeight;

    override public void Start() // Call Inherited Start and then do our own and initialize the object
    {
        base.Start(); // Call parent's start
        autoRotate = true; // Always face direction of travel
        steeringForce = Vector3.zero; // Initialize the steering force
        desiredSeparation = radius * 2f;  // Separation based on size
        sPartial = Vector3.zero;
        sTotal = Vector3.zero;
        leaderScript = GameObject.FindGameObjectWithTag("Blu").GetComponent<Leader>(); // Access leader's script
        arrivalDistSq = Mathf.Pow(arrivalDist, 2f); // Square once and never again
    }

    protected override void CalcSteeringForces() // Calculate the forces necessary to steer the Dalek to its desired destination (flocking algorithms, obstacle avoidance, etc.)
    {
        steeringForce = Vector3.zero; // Reset the steering force
        desired = leaderScript.FollowPoint - transform.position; // Use arrival instead of Seek() to not run into the leader
        sepDist = desired.sqrMagnitude; // Get the comparative distance to the point
        desired.Normalize();

        if (sepDist <= arrivalDistSq) // When encroaching on the set area around the point,
        {
            desired = desired * (maxSpeed * (Mathf.Sqrt(sepDist) / arrivalDist)); // Scale the desired vector by the distance mapped to speed (equation = map(sD,0,aD,0,mS))
        }
        else
        {
            desired = desired * maxSpeed; // If not encroaching, scale the vector to maxSpeed
        }
        steer = desired - velocity; // Take the desired velocity we just calculated and subtract the current velocity
        steeringForce = followWeight * steer; // Add this total arrival/follow force to the total steering force

        steeringForce += alignWeight * Seek(gm.Centroid.transform.forward); // Alignment, move with the flock (seek the average direction)
        steeringForce += cohereWeight * Seek(gm.Centroid.transform.position); // Cohesion, stay with the flock (seek the average/center position)

        sTotal = Vector3.zero; // Separation, space out so the flock isn't just a tight bunch
        int count = 0; // Create a counter for averaging the force vectors
        for (int i = 0; i < gm.Flock.Length; i++) // Separation code, check against all flockers to see which ones need separaing from
        {
            sepDist = Vector3.Distance(this.transform.position, gm.Flock[i].transform.position); // Get the distance between vehicles
            if ((sepDist > 0) && (sepDist < desiredSeparation)) // If distance between them is more than 0 (same vehicle) and less than the desired distance,
            {
                sPartial = this.transform.position - gm.Flock[i].transform.position; // Then get a flee force,
                sPartial.Normalize();
                sPartial = sPartial / sepDist; // Scale it,
                sTotal += sPartial; // And add it to the total
                count++; // Indicate that one more vehicle has to be separated from
            }

            if (count > 0) // Only do extra calculations if there are vehicles to avoid
            {
                sTotal = sTotal / count; // Average the force and scale to max speed
                sTotal.Normalize();
                sTotal = sTotal * maxSpeed;
                sTotal -= velocity;
                steeringForce += separateWeight * sTotal; // Apply to the steering force (I messed this up last time, I was applying directly to the acceleration)
            }
        }

        steeringForce += avoidWeight * AvoidObstacle();

        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce); // Limit the 1 steering force (ultimate force)
        ApplyForce(steeringForce); // Apply all forces to the acceleration as 1 force (ultimate force) in ApplyForce()
    }
}
