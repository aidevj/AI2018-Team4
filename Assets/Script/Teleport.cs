using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	public Transform reciever;  // transform of the spot where the agent should teleport to
    public int recieverID;  // side number this teleporter goes to
  
    // if any unit goes into the portal, teleport
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" || other.tag == "Flocker" || other.tag == "Blu")
		{
            if (other.tag == "Blu")
            {
                other.gameObject.GetComponent<Leader>().mapSideLocation = recieverID;
            }
            else if (other.tag == "Flocker")
            {
                other.gameObject.GetComponent<Flocker>().mapSideLocation = recieverID;
            }
            other.gameObject.transform.position = reciever.position;

		}
	}
    
}
