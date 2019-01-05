using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum PlayerID{
	ONE = 1,
	TWO
}
enum PlayerDungeonStatus
{
	Normal,
}

public class PlayerController_2D : NetworkBehaviour
{

	[HideInInspector] public PlayerID ID;
	public float moveSpeed;
	
	public float V;	
	public float H;
	public Animator anim_2d;
	public LifeUI lifeUI;
	private Vector2 moveDirection;
	private Vector2 lastMove;
	private bool isAttacking;
	private bool isMoving;
	protected Coroutine attackRoutine;

	private float attackTimer = 0;
	private float atkCooldown = .15f;

	public int attackDamage;
	public int attackRadius;

	private Vector2 attackDirection;
	public int maxHealth;

	private int health;


	// Use this for initialization
	void Start ()
	{
		anim_2d = GetComponent<Animator> ();
		lifeUI = GetComponent<LifeUI>();
		health = maxHealth;
	}

	// Update is called once per frame
	void Update ()
	{

		
		if (!isLocalPlayer)
			return;
		
		isMoving = false;
		V = Input.GetAxis ("Vertical");
		H = Input.GetAxis ("Horizontal");

		lifeUI.SetHealth(health);

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

		anim_2d.SetFloat ("Horizontal", H);
		anim_2d.SetFloat ("Vertical", V);
		anim_2d.SetFloat ("LastHorizontal", lastMove.x);
		anim_2d.SetFloat ("LastVertical", lastMove.y);
		anim_2d.SetBool ("IsMoving", isMoving);
	}

	public override void OnStartLocalPlayer()
     {
         Camera.main.GetComponent<CameraDungeon>().setTarget(gameObject.transform);
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
            



    /*public void Move()
	{
		transform.Translate (moveDirection * moveSpeed * Time.deltaTime);
	}

	private void GetInput()
	{
		float V = Input.GetAxis ("Vertical");s
		float H = Input.GetAxis ("Horizontal");
		moveDirection = Vector2.zero;

		if (Input.GetAxis ("Horizontal") > 0.2f || Input.GetAxis ("Horizontal") > -0.2f) {
			moveDirection += Vector2.right;
			anim_2d.SetFloat ("Horizontal", H);
		}
		if (Input.GetAxis ("Horizontal") < 0.0f) {
			moveDirection += Vector2.left;
			anim_2d.SetFloat ("Horizontal", H);
		}
		if (Input.GetAxis ("Vertical") > 0.0f) {
			moveDirection += Vector2.up;
			anim_2d.SetFloat ("Vertical", V);
		}
		if (Input.GetAxis ("Vertical") < 0.0f) {
			moveDirection += Vector2.down;
			anim_2d.SetFloat ("Vertical", V);
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			attackRoutine = StartCoroutine(Attack ());
		}
	}
	*/


}
								