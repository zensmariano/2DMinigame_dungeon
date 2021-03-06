﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum PlayerID{
	ONE = 1,
	TWO
}
enum PlayerDungeonStatus
{
	Normal,
}

public class PlayerController_2D : MonoBehaviour
{

	[HideInInspector] public PlayerID ID;
	private float moveSpeed;
	public float normalMoveSpeed;

	private bool isImune;
	
	public float V;	
	public float H;
	public Animator anim_2d;
	public LifeUI lifeUI;

	private float timerPlayerDestroy;

	public float playerDestroyCooldown;

	public DungeonManager.SubDungeon subdungeon;
	private Vector2 moveDirection;
	private Vector2 lastMove;
	private bool isAttacking;
	private bool isMoving;
	protected Coroutine attackRoutine;

	private float attackTimer = 0;
	private float atkCooldown = .15f;

	public int normalAttackDamage;
	private int attackDamage;
	public int attackRadius;

	private Vector2 attackDirection;
	public int maxHealth;

	private int health;
	
	private int roomCount;



	// Use this for initialization
	void Start ()
	{
		anim_2d = GetComponent<Animator> ();
		lifeUI = GameObject.FindGameObjectWithTag("LifeUI").GetComponent<LifeUI>();
		health = maxHealth;
		moveSpeed = normalMoveSpeed;
		attackDamage = normalAttackDamage;
		isImune = false;
		lifeUI.SetHealth(health);
		lifeUI.Init();
        Camera.main.GetComponent<CameraDungeon>().setTarget(gameObject.transform);
	}

	// Update is called once per frame
	void Update ()
	{
		
	
		
		isMoving = false;
		V = Input.GetAxis ("Vertical");
		H = Input.GetAxis ("Horizontal");

		//lifeUI.SetHealth(health);

		if (Input.GetAxis ("Horizontal") > 0.2f || Input.GetAxis ("Horizontal") < -0.2f) {
			if (H > 0.2f)
				H = 1;
			if (H < -0.2f)
				H = -1;
			transform.position += (new Vector3 (H * moveSpeed * Time.deltaTime, 0.0f, 0.0f));
			isMoving = true;
			lastMove = new Vector2 (H, 0.0f);
		}

		if (Input.GetAxis ("Vertical") > 0.2f || Input.GetAxis ("Vertical") < -0.2f) {
			if (V > 0.2f)
				V = 1;
			if (V < -0.2f)
				V = -1;
			transform.position += (new Vector3 (0.0f, V * moveSpeed * Time.deltaTime, 0.0f));
			isMoving = true;
			lastMove = new Vector2 (0.0f, V);
		}

		if (Input.GetKeyDown (KeyCode.Space) && !isAttacking) {
			isAttacking = true;
			anim_2d.SetBool ("IsAttacking", isAttacking);
			attackTimer = atkCooldown;
		} 

		if (isAttacking) {
			if (attackTimer > 0) {
				attackTimer -= Time.deltaTime;
			} else {
				isAttacking = false;
				anim_2d.SetBool ("IsAttacking", isAttacking);
			}

		}
		
		if(health <= 0)
		{
			timerPlayerDestroy += Time.fixedDeltaTime;
			PlayerIsDead();
		}

		anim_2d.SetFloat ("Horizontal", H);
		anim_2d.SetFloat ("Vertical", V);
		anim_2d.SetFloat ("LastHorizontal", lastMove.x);
		anim_2d.SetFloat ("LastVertical", lastMove.y);
		anim_2d.SetBool ("IsMoving", isMoving);
		lifeUI.SetHealth(health);
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("SmallCoin")) {

			ScoreScript2D.biriCoinCount += 1;
			Destroy (other.gameObject);
		}
		if (other.gameObject.CompareTag ("MediumCoin")) {

			ScoreScript2D.biriCoinCount += 5;
			Destroy (other.gameObject);
		}
		if (other.gameObject.CompareTag ("BigCoin")) {

			ScoreScript2D.biriCoinCount += 10;
			Destroy (other.gameObject);
		}
	}

	public void SufferDamage(int damage){
		health -= damage;
		Debug.Log(health);
	}

    public void DealDamage(List<EnemyController> enemiesInRange){
		foreach( EnemyController e in enemiesInRange)
		{
			if(e.health > 0)
			e.SufferDamage(attackDamage);
		}
		
    }

	
    void StartAttack()
    {
		print("attack");

		Vector2 hv = new Vector2(H,V);
		attackDirection = (Vector2)transform.position + hv;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
		List<EnemyController> enemiesInRange = new List<EnemyController>();
		foreach (Collider2D c in hitColliders)
		{
			float attackAngle = Vector2.Angle(c.transform.position - this.transform.position, hv);
			if((attackAngle > 45 || attackAngle > -45) && c.gameObject.CompareTag("Enemy"))
			{
				enemiesInRange.Add(c.gameObject.GetComponent<EnemyController>());
			}

		}
		if(enemiesInRange.Count > 0)
		{
			Debug.Log("dealdamage");
			DealDamage(enemiesInRange);
		}
		
	}
            
	public void AttackBoostPU(){
		StartCoroutine("AttackBoost", 5f);
	}

	private IEnumerator AttackBoost(float waitTime){
		attackDamage = normalAttackDamage * 2;
		yield return new WaitForSeconds(waitTime);
		attackDamage = normalAttackDamage;
	}

	public void ShieldPU(){
		StartCoroutine("Shield", 5f);
	}

	private IEnumerator Shield(float waitTime){
		isImune = true;
		yield return new WaitForSeconds(waitTime);
		isImune = false;
	}

	public void SpeedPU(){
		StartCoroutine("Speed", 5f);
	}

	private IEnumerator Speed(float waitTime){
		moveSpeed = normalMoveSpeed * 2;
		yield return new WaitForSeconds(waitTime);
		moveSpeed = normalMoveSpeed;
	}

	public void HealPU(){
		health = maxHealth;
	}

	public void	PlayerIsDead()
	{
		

		//AlphaOut ();
		GameObject.FindGameObjectWithTag("Died").GetComponent<Text>().CrossFadeAlpha(1,0,true);

		if (timerPlayerDestroy > playerDestroyCooldown) {
			Destroy (this.gameObject);
			Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
		}
	}

	

}
								