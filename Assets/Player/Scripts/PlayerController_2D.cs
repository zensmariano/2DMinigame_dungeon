using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerDungeonStatus
{
	Normal,
}

public class PlayerController_2D : MonoBehaviour {

	public float moveSpeed;
	public Animator anim_2d;
	private Vector2 moveDirection;
	private bool isAttacking = false;
	protected Coroutine attackRoutine;

	// Use this for initialization
	void Start () {
		anim_2d = GetComponent<Animator> ();
		moveDirection = Vector2.up;
	}

	// Update is called once per frame
	void Update () {
		GetInput ();
		Move ();
	}

	public void Move()
	{
		transform.Translate (moveDirection * moveSpeed * Time.deltaTime);
	}

	private void GetInput()
	{
		float V = Input.GetAxis ("Vertical");
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

	private IEnumerator Attack()
	{
		if (!isAttacking) {
			
			isAttacking = true;
			Debug.Log ("attack");
			yield return new WaitForSeconds (2);

			StopAttack ();
		}
	}

	public void StopAttack()
	{
		if (attackRoutine != null) {
			StopCoroutine (attackRoutine);
			isAttacking = false;
		}
	}
}
								