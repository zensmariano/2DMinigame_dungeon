using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiedTextController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetComponent<Text>().CrossFadeAlpha(0,0,true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
