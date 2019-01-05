﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoostScript2D : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"){
			other.gameObject.GetComponent<PlayerController_2D>().AttackBoostPU();
			Destroy (gameObject);
		}
	}
}