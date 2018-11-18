using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
	public Transform player;
	public Transform Blu;

	public Transform reciever;

	private bool playerIsOverlapping = false;
	private bool bluIsOverlapping = false;
	// Update is called once per frame
	void Update () {
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
			if (dotProduct < 0f)
			{
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 0;
				player.Rotate(Vector3.up,rotationDiff);

				Vector3 postionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = reciever.position + postionOffset;
				playerIsOverlapping = false;
			}
		}else if(bluIsOverlapping)
		{
			Vector3 portalToBlu = Blu.position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToBlu);
			if (dotProduct < 0f)
			{
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 0;
				Blu.Rotate(Vector3.up,rotationDiff);
				Vector3 postionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToBlu;
				Blu.position = reciever.position + postionOffset;
				bluIsOverlapping = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
		else if (other.tag == "Blu")
		{
			bluIsOverlapping = true;
		}
		else if (other.tag == "Flocker" )
        {
            Vector3 portalToCapsule = other.gameObject.transform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToCapsule);
            if (dotProduct < 0f)
            {
                float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
                rotationDiff += 0;
                other.gameObject.transform.Rotate(Vector3.up, rotationDiff);
                Vector3 postionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToCapsule;
                other.gameObject.transform.position = reciever.position + postionOffset;
            }
        }
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}else if (other.tag == "Blu")
		{
			bluIsOverlapping = false;
		}
	}
}
