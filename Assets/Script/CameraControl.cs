using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W)) transform.position += new Vector3(0, 0, 1);
        if (Input.GetKey(KeyCode.A)) transform.position += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.S)) transform.position += new Vector3(0, 0, -1);
        if (Input.GetKey(KeyCode.D)) transform.position += new Vector3(1, 0, 0);
        if (Input.GetKey(KeyCode.E)) transform.position += new Vector3(0, -1, 0);
        if (Input.GetKey(KeyCode.Q)) transform.position += new Vector3(0, 1, 0);
    }
}
