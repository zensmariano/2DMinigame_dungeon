using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform child  in transform)
		{
			if(child.GetComponent<EnemyController>().health <= 0)
			{
				GameObject.Destroy(child.gameObject);
			}
		} 
	}
}
