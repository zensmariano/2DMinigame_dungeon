using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerDungeonStatus
{
	Normal,
}

public class PlayerController_2D : MonoBehaviour {

	public float moveSpeed;
	private Vector2 moveDirection;
	private bool isAttacking = false;
	protected Coroutine attackRoutine;

	// Use this for initialization
	void Start () {
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
		moveDirection = Vector2.zero;

		if (Input.GetAxis ("Horizontal") > 0.0f) {
			moveDirection += Vector2.right;
		}
		if (Input.GetAxis ("Horizontal") < 0.0f) {
			moveDirection += Vector2.left;
		}
		if (Input.GetAxis ("Vertical") > 0.0f) {
			moveDirection += Vector2.up;
		}
		if (Input.GetAxis ("Vertical") < 0.0f) {
			moveDirection += Vector2.down;
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
								