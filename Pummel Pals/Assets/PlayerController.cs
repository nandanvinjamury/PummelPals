using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	private Vector2 inputVector;
	public float speed;
	public float jumpForce;
	private bool isGrounded;
	public Transform groundCheck;
	private float checkRadius;
	public LayerMask groundDetect;
	private float numberOfJumps;
	public float extraJumps;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		numberOfJumps = extraJumps;
		checkRadius = 0.05f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundDetect);

		inputVector.x = Input.GetAxisRaw("Horizontal")*speed;
		
		if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && numberOfJumps > 0) {
			inputVector.y = jumpForce;
			numberOfJumps--;
		} else if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && numberOfJumps == 0 && isGrounded) {
			inputVector.y = jumpForce;
		} else {
			inputVector.y = rb.velocity.y;
		}

		if (isGrounded) {
			numberOfJumps = extraJumps;
		}


		rb.velocity = inputVector;
		if(inputVector.x < 0) {
			transform.localScale = new Vector2(-1,1);
		} else if (inputVector.x > 0) {
			transform.localScale = new Vector2(1, 1);
		}

	}
}
