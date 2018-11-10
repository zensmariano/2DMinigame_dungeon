using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	public Transform camera;
	public Transform board;

	// Use this for initialization
	void Start () {
		
		camera.LookAt(board);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
