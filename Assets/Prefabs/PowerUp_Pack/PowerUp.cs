using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public bool speedBoost = false;
	public bool shieldBoost = false;
	public bool healBoost = false;
	public bool attackBoost = false;

	public float powerUpDuration;
	private PowerUpManager powerUpManager;

	void Start()
	{
		powerUpManager = FindObjectOfType<PowerUpManager> ();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == ("Player")) {
			print ("collider_pickup");
			powerUpManager.ActivatePowerup (speedBoost, shieldBoost, healBoost, attackBoost, powerUpDuration);
		}
		gameObject.SetActive (false);
	}

}