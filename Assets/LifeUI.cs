using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeUI : MonoBehaviour {

	private int player_health;
	public GameObject heart;
	public void SetHealth(int health)
	{
		player_health = health;
	}

	public void Init()
	{
		for(int i = 0; i < player_health; i++)
		{
			GameObject instance =  Instantiate(heart,transform.position + new Vector3(i * 40,0,0), Quaternion.identity);
			instance.transform.parent = transform;
		}
	}

	void Update() {
		int lives = player_health;
		foreach(Transform child in transform){
			if(lives > 0){
				child.gameObject.SetActive(true);
				lives--;
			}else{
				child.gameObject.SetActive(false);
			}
		}
		
	}
}
