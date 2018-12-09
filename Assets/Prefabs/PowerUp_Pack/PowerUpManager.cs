using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

	public bool speedBoost;
	public bool shieldBoost;
	public bool healBoost;
	public bool attackBoost;
	public float powerUpDuration;
	public float multiplier;

	private bool powerUpActive;
	private float powerUpDurationCounter;
	private PlayerController_2D playerController;

	// Use this for initialization
	void Start () {
		playerController = FindObjectOfType<PlayerController_2D>();
	}
	
	// Update is called once per frame
	void Update () {

		if (powerUpActive) {

			powerUpDurationCounter -= Time.deltaTime;

			if (speedBoost) {
				print ("speed");
				playerController.moveSpeed = playerController.moveSpeed * multiplier /* Time.deltaTime*/; 
			}

			if (shieldBoost) {
				//n perde vida
			}
			if (healBoost) {
				//playerController.heal = playerController.heal * multiplier * Time.deltaTime;
			}

			if (attackBoost) {
				//playerController.damage = playerController.damage * multiplier * Time.deltaTime;
			}

			if (powerUpDurationCounter <= 0) {
				playerController.moveSpeed = playerController.moveSpeed / multiplier;
				powerUpActive = false;
			}
		}	
	}

	public void ActivatePowerup(bool speed, bool shield,bool heal, bool attack, float time)
	{
		speedBoost = speed;
		shieldBoost = shield;
		healBoost = heal;
		attackBoost = attack;
		powerUpDuration = time;

		powerUpActive = true;
	}

}
