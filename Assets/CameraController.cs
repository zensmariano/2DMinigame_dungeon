using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float inputHorizontal;
    private float inputVertical;

    public float speed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
        transform.position += inputHorizontal * Vector3.right * speed * Time.deltaTime;
        transform.position += inputVertical * Vector3.up * speed * Time.deltaTime;
    }
}
