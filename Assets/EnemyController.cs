using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour {

	public int starting_health;

	[HideInInspector]
	public int health;

    public bool can_attack;

    public float stopBeforeRepathing;
    public float attackCooldown;

    Node behaviourTree;
	Context behaviourState;

    private Vector3 lastPosition;

    private PlayerController_2D current_attack_target;

    private float stopTimer;
    private float attackTimer;

    public int attackDamage;
	void Start () {
		health = starting_health;
        lastPosition = transform.position;
		behaviourTree = CreateBehaviourTree();
    	behaviourState = new Context(this);

        stopTimer = 0;
        attackTimer = 0;
	}

	void FixedUpdate() {
    	behaviourTree.Behave(behaviourState);
        stopTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
	}

    private void LateUpdate() {
        lastPosition = transform.position;
    }
	
	Node CreateBehaviourTree()
    {   
        Sequence lifeOrDeath = new Sequence("lifeOrDeath",
            new IsAlive());

        Sequence randomWalk = new Sequence("randomWalk",
            new SetRandomDestination(),
            new Inverter(new Succeeder(new Move())));

        Selector chooseEnemy = new Selector("chooseEnemy",
            new TargetClosestEnemy(5),
            randomWalk);

        Sequence moveTowardsEnemy = new Sequence("moveTowardsEnemy",
            new SetMoveTargetToEnemy(),
            new Inverter(new Succeeder(new Move())));

        Sequence attackEnemy = new Sequence("attackEnemy",
        new CanAttackEnemy(),
        new AttackEnemy());
/*
        Sequence moveTowardsEnemy = new Sequence("moveTowardsEnemy",
            new HasEnemy(),
            new SetMoveTargetToEnemy(),
            new Inverter(new CanAttackEnemy()),
            new Inverter(new Succeeder(new Move())));

        Sequence attackEnemy = new Sequence("attackEnemy",
            new HasEnemy(),
            new CanAttackEnemy(),
            new StopMoving(),
            new AttackEnemy());

        Sequence needHeal = new Sequence("needHeal",
            new Inverter(new AmIHurt(15)),
            new AmIHurt(35),
            new FindClosestHeal(30),
            new Move());

        Selector chooseEnemy = new Selector("chooseEnemy",
            new TargetNemesis(),
            new TargetClosestEnemy(30));

        Sequence collectPowerup = new Sequence("collectPowerup",
            new FindClosestPowerup(50),
            new Move());
*/
        Selector fightOrFlight = new Selector("fightOrFlight",
            new Inverter(lifeOrDeath),
            new Inverter(new Succeeder (chooseEnemy)),
            moveTowardsEnemy,
            attackEnemy);

        Repeater repeater = new Repeater(fightOrFlight);

        return repeater;
    }

	public void SufferDamage(int damage){
		health -= damage;
	}

    public void DealDamage(){
        
        if(can_attack)
        {
            current_attack_target.SufferDamage(attackDamage);
            SetCanAttackFalse();
        }
        
    }

    void SetCanAttackFalse()
    {
        can_attack = false;
    }

    public void SetCanAttackTrue(PlayerController_2D player)
    {
        current_attack_target = player;
        can_attack = true;
    }

	public float DistanceTo(Vector3 position){
		return Vector3.Distance(this.transform.position, position);
	}

    public Vector3 GetLastPosition(){
        return lastPosition;
    }


    public bool HasWaitedBeforeRepathing(){

        if (stopTimer > stopBeforeRepathing){
            stopTimer = 0;
            return true;
        }
        
        return false;
    }

    public void LinkMovementWithAnimator(Vector3 moveTarget){
        Vector3 movementVector = (moveTarget - transform.position).normalized;
        if(movementVector != Vector3.zero){
            transform.GetComponent<Animator>().SetBool("IsMoving", true);
    
            transform.GetComponent<Animator>().SetFloat("Horizontal", movementVector.x);
            transform.GetComponent<Animator>().SetFloat("Vertical", movementVector.y);
        }else{
            
            transform.GetComponent<Animator>().SetBool("IsMoving", false);
    
            transform.GetComponent<Animator>().SetFloat("Horizontal", 0f);
            transform.GetComponent<Animator>().SetFloat("Vertical", 0f);
        }
    }

    public void LinkAttackWithAnimator(){
        transform.GetComponent<Animator>().SetBool("IsMoving", false);

        if (attackTimer > attackCooldown){
            transform.GetComponent<Animator>().SetBool("IsAttacking", true);
            attackTimer = 0;
        }else{
            transform.GetComponent<Animator>().SetBool("IsAttacking", false);
        }
    }




/*
    public bool IsCollidingWithWall(Vector3 moveTarget){
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, (Vector2)moveTarget - (Vector2)transform.position, 2);
        Debug.DrawRay((Vector2)transform.position, (Vector2)moveTarget - (Vector2)transform.position);

    if(hit){
        if(hit.collider.gameObject.tag == "Wall"){
            Debug.DrawRay((Vector2)transform.position, (Vector2)moveTarget - (Vector2)transform.position, Color.red);
            Debug.Log("hit wall");
            return true;
        }
    }
            
        
         else if (this.GetComponent<CircleCollider2D>().IsTouchingLayers(8))
        {
            Debug.Log("hit wall");
            return true;
        }
        
        return false;
    }
*/

}
