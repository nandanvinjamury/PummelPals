using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	private Rigidbody2D rb;
	private float speed;
	private GameObject player;
	public float attackAllowed;
	private float timeBtwnAttack;
	public bool playerCollider;
	public float attackRadius;
	public LayerMask attackDetect;
	public Transform attackCheck;
	private Animator anim;
	public float health;
	private SpriteRenderer sprite;

	// Use this for initialization
	void Start() {
		rb = GetComponent<Rigidbody2D>();
		speed = -5;
		player = GameObject.Find("player");
		timeBtwnAttack = 1f;
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (Vector2.Distance(player.transform.position, transform.position) > 1.5f) {
			rb.velocity = new Vector2(speed, 0);
			anim.SetInteger("attack", 0);
		} else if(Vector2.Distance(player.transform.position, transform.position) <= 1.5f && health > 0) {
			rb.velocity = new Vector2(0, 0);
			Attack();
		}

		if (rb.velocity.x < 0) {
			transform.localScale = new Vector3(2, 2);
		} else if (rb.velocity.x > 0) {
			transform.localScale = new Vector3(-2, 2);
		} else {
			if (player.transform.position.x < transform.position.x) {
				transform.localScale = new Vector3(2, 2);
			} else if (player.transform.position.x > transform.position.x) {
				transform.localScale = new Vector3(-2, 2);
			}
		}

	}

	public void Damage(int subtract) {
		health -= subtract;
		sprite.color = Color.red;
		Invoke("Flash", 0.05f);
		if(health <= 0) {
			anim.SetInteger("attack", 2);
			Destroy(gameObject, 0.6f);        //damage enemy after 1 second (to allow attack animation to play)
		}
	}

	private void Flash() {
		sprite.color = Color.white;
	}

	private void Attack() {
		if (attackAllowed <= 0) { //countdown hit zero meaning you're allowed to attack
			playerCollider = Physics2D.OverlapCircle(attackCheck.position, attackRadius, attackDetect); //checks if player is in the attack area
			attackAllowed = timeBtwnAttack; //as soon as we click we don't allow another attack by setting timer until the timer goes back to zero
			anim.SetInteger("attack", 1);
		} else {
			attackAllowed -= Time.deltaTime;    //countdown the attack reset timer
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("turn"))
			speed = -speed;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag.Equals("turn"))
			speed = -speed;
	}
}
