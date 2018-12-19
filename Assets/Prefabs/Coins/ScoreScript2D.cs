using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript2D : MonoBehaviour {

	Text scoreText;
	public static int biriCoinCount;

	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text> ();
		biriCoinCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		scoreText.text = biriCoinCount.ToString ();
	}
}
