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
	private Vector2 lastMove;
	private bool isAttacking;
	private bool isMoving;
	protected Coroutine attackRoutine;

	// Use this for initialization
	void Start () {
		anim_2d = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {

		isMoving = false;
		isAttacking = false;
		float V = Input.GetAxis ("Vertical");
		float H = Input.GetAxis ("Horizontal");

		if (Input.GetAxis ("Horizontal") > 0.2f || Input.GetAxis ("Horizontal") < -0.2f) {
			transform.Translate (new Vector3 (H * moveSpeed * Time.deltaTime, 0.0f, 0.0f));
			isMoving = true;
			lastMove = new Vector2 (H, 0.0f);
		}

		if (Input.GetAxis ("Vertical") > 0.2f || Input.GetAxis ("Vertical") < -0.2f) {
			transform.Translate (new Vector3 (V * moveSpeed * Time.deltaTime, 0.0f, 0.0f));
			isMoving = true;
			lastMove = new Vector2 (0.0f,V);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			attackRoutine = StartCoroutine(Attack ());
			isAttacking = true;
			anim_2d.SetBool ("IsAttacking", isAttacking);
		}

		anim_2d.SetFloat ("Horizontal", H);
		anim_2d.SetFloat ("Vertical",V);
		anim_2d.SetFloat ("LastHorizontal", lastMove.x);
		anim_2d.SetFloat ("LastVertical", lastMove.y);
		anim_2d.SetBool ("IsMoving", isMoving);

	}

	/*public void Move()
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
	}*/

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
								