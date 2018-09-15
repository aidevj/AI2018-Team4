using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwtich : MonoBehaviour {

	// Use this for initialization
	private GameObject MainCamera;
	private GameObject CharacterCamera;
	private bool main_camera_up = true;
	void Start () {
		MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		CharacterCamera = GameObject.FindGameObjectWithTag("CharacterCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("SightSwitch"))
		{
			if (main_camera_up)
			{
				CharacterCamera.SetActive(true);
				MainCamera.SetActive(false);
				main_camera_up = false;
			}
			else
			{
				CharacterCamera.SetActive(false);
				MainCamera.SetActive(true);
				main_camera_up = true;
			}
		}
		
	}
}
