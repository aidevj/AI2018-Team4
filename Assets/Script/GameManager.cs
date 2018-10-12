
using UnityEngine;
using System.Collections;

//add using System.Collections.Generic; to use the generic array format
using System.Collections.Generic;

// This script manages overall variables and operations otherwise outside the scope of the vehicle script


public class GameManager : MonoBehaviour
{
    [HideInInspector] public GameObject leader; // Leader of the flock
    private GameObject centroid; // Vector and property for the position of the center point of the flock
    private GameObject player;
    public GameObject Centroid
    {
        get { return centroid; }
    }
    private GameObject[] flock; // Variable and property for the array that makes up the flock
    public GameObject[] Flock
    {
        get { return flock; }
    }

    void Start() // Initialize the scene
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        centroid = GameObject.FindGameObjectWithTag("Centroid"); // Get the centroid

        leader = GameObject.FindGameObjectWithTag("Blu"); // Get the Leader from the scene

        flock = GameObject.FindGameObjectsWithTag("Flocker"); // Get the Flockers from the scene. Order doesn't matter.
    }

    void Update() // Update the scene variables, called once per frame
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            List<GameObject> path = leader.GetComponent<AStar>().Pathfind(leader.transform.position, player.transform.position);
            if (path.Count != 0) leader.GetComponent<Leader>().setpath(path);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().followWeight--;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().followWeight++;
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().alignWeight--;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().alignWeight++;
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().cohereWeight--;
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().cohereWeight++;
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().separateWeight--;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().separateWeight++;
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().avoidWeight--;
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (GameObject flocker in flock)
            {
                flocker.GetComponent<Flocker>().avoidWeight++;
            }
        }
        for (int i = 0; i < flock.Length; i++) // Update the flocking variables
        {
            centroid.transform.forward += flock[i].transform.forward; // Add all the Daleks' directions and positions,
            centroid.transform.position += flock[i].transform.position;
        }
        centroid.transform.forward = centroid.transform.forward / flock.Length; // Then divide them by the number of Daleks to compute the average
        centroid.transform.position = centroid.transform.position / flock.Length;
    }
}
