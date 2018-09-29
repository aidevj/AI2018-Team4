﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	public Transform player;

	public Transform reciever;

	private bool playerIsOverlapping = false;
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
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
        else if (other.tag == "Flocker" || other.tag == "Blu")
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
		}
	}
}
