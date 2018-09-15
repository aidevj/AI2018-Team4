using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    
    public float speed = 6f;

	public float rotateSpeed = 6f;

	public float gravity = 20f;
	
	public float jumpSpeed = 8f;


	private Vector3 moveDirection = Vector3.zero;

	private CharacterController controller;

	private int jump;
	
	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller.isGrounded)
		{
			moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButtonDown("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		transform.Rotate(0,2*Input.GetAxis("Horizontal"),0);
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
