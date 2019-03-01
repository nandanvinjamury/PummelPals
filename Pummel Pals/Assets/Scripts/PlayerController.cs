using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	#region Public Variables

	public float speed;					//the speed at which the player moves
	public float jumpForce;				//the force at which the player jumps
	public float extraJumps;			//how many extra jumps the player has (right now it's going to be a double jump)
	public Transform groundCheck;		//marker attached to player's feet for ground detection
	public Transform attackCheck;		//marker attached to player's attack area for attack detection
	public float groundRadius;			//the radius of the circle checking the area of the ground below the player
	public float attackRadius;			//the radius of the circle checking the area of the player's attack
	public LayerMask groundDetect;		//decides what to detect when checking ground
	public LayerMask attackDetect;		//decides what objects the player can attack
	public LayerMask specialDetect;		//decides what to detect when checking special platforms
	public float timeBtwnAttack;		//sets the attack speed of the player
	public Enemy[] enemy;				//enemies that can be attacked
	public Image healthbar;				//the player's healthbar
	public Image specialbar;			//the player's special bar
	public float specialFillSpeed;		//adjusts the rate at how fast teh player's special regens
	public Color[] characters;			//the characters to switch between
	public GameObject boat;				//Brett's skill
	public GameObject bomb;				//Casey's skill
	public bool boatExists;				//checks if a boat already exists
	public BoxCollider2D crouchCollide; //finds crouch collider
	public GameObject jumpdust;         //the dust that forms when you jump
	public GameObject shadow;           //drop shadow that is below the player
	public BoxCollider2D exit;			//exit box collider

	#endregion

	#region Private Variables

	private Rigidbody2D rb;         //the rigidbody component of the player; messes with physics
	private BoxCollider2D collide;	//the player's collider
	private Vector2 inputVector;    //the input the player movement takes
	private bool isGrounded;        //checks whether the player is on the ground
	private bool isReady;           //checks whether the player is standing on a special platform
	private float numberOfJumps;    //the number of jumps the player has
	private float attackAllowed;    //calculates if attack is allowed
	private Collider2D[] enemies;   //enemies allowed to be attacked
	private Animator animator;      //animator for the player
	private bool crouch;			//a boolean to check if the player is in the crouch state
	private bool attack;			//a boolean to check if the player is in the attack state
	private bool block;             //a boolean to check if the player is in the block state
	private bool climb;             //a boolean to check if the player is in the climb state
	private static float health;    //the player's health
	private static float special;	//the player's special amount
	private GameManager gm;			//the game manager
	private int choice;				//calculates which character to switch to
	private SpriteRenderer sprite;	//for player sprite to switch
	private bool specialAllowed;    //checks if special is allowed
	private bool amelia;
	private bool brett;
	private bool casey;
	private bool dennis;
	private bool isFlying;
	private RaycastHit2D hit;		//checks raycast hit point

	#endregion

	void Start() {	//initialized variables
		rb = GetComponent<Rigidbody2D>();
		collide = gameObject.GetComponent<BoxCollider2D>();
		numberOfJumps = extraJumps;
		groundRadius = 0.05f;
		animator = GetComponent<Animator>();
		crouch = false;
		attack = false;
		block = false;
		climb = false;
		health = 100;
		enemy = GameObject.FindObjectsOfType<Enemy>();
		gm = GameObject.FindObjectOfType<GameManager>();
		transform.position = gm.checkpointPos;
		choice = 0;
		sprite = GetComponent<SpriteRenderer>();
		specialAllowed = true;
		special = 100;
		boatExists = false;
		amelia = true;
		brett = casey = dennis = false;
		isFlying = false;
	}

	private void Update() {
		if (isFlying) {
			special -= 0.75f;
		}
		if (special <= 0) {
			rb.gravityScale = 5;
			isFlying = false;
			Climb();
		}
		if (climb && special > 0f) {
			special -= 0.6f;
		}
		DropShadow();
		if (collide.bounds.Intersects(exit.bounds) && (Input.GetKey(KeyCode.Space) || Input.GetButton("Jump"))) {
				gm.NextLevel();
		}
	}

	void FixedUpdate() {		//functions that run each update (fixedupdate is better for physics)
		GroundCheck();			//checks if the character is on the ground
		Movement();             //moves the character
		Crouch();				//lets the character crouch
		Attack();				//lets the character attack
		Block();                //lets the character block
		UseSpecial();           //lets the character use a special
		Special();				//shows special bar
		AnimationState();       //sets animations
		Health();               //shows health bar
		SwitchCharacter();      //allows characters to switch
	}

	private void GroundCheck() { //checks if the character is on the ground
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundDetect);
	}

	private void MoveInput() {
		if (!attack && !block && !crouch && !climb) {							//assuming the player is not attacking or blocking or crouching or climbing
			inputVector.x = Input.GetAxisRaw("Horizontal") * speed;		//allow him to move left and right
		}
		else {
			inputVector.x = 0;
		}
		if (inputVector.x < 0) {										//if he is moving left
			transform.localScale = new Vector2(-1, 1);					//face him left
		} else if (inputVector.x > 0) {                                 //if he is moving right
			transform.localScale = new Vector2(1, 1);					//face him right
		}
	}

	private void JumpInput() {
		if (!isFlying && !climb) {
			if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Fire1")) && numberOfJumps > 0 && !attack && !block) { //if he has jumps left, presses up input, and is not attacking/blocking
				rb.velocity = new Vector3(rb.velocity.x, 0);
				isGrounded = false;                 //in case isgrounded resets his jumps
				inputVector.y = jumpForce;          //let him jump
				numberOfJumps--;                    //subtract from number of jumps left
				Instantiate(jumpdust, transform.position, Quaternion.identity);
				animator.Play("player_jump", -1, 0f);
			} else if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Fire1")) && numberOfJumps == 0 && !attack && !block && isGrounded) {
				inputVector.y = jumpForce;          //let him jump
			} else {
				inputVector.y = rb.velocity.y;      //if he runs out of jumps, let his current velocity (gravity) take over
			}
			if (isGrounded) {                       //once he touches the ground
				numberOfJumps = extraJumps;         //reset the jumps he has left
			}
		}
		if (climb) {
			if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetButton("Fire1")) && !attack && !block) {
				rb.velocity = new Vector3(rb.velocity.x, 0);
				isGrounded = false;                 //in case isgrounded resets his jumps
				inputVector.y = 2;          //let him climb
			}
			else if((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetButtonUp("Fire1")) && !attack && !block) {
				inputVector.y = 0;
			}
		}
	}

	private void Crouch() {
		if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxisRaw("Vertical") == -1) && isGrounded){
			crouch = true;
			crouchCollide.enabled = true;
			collide.enabled = false;
		} else if((Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetAxisRaw("Vertical") >= 0) || !isGrounded) {
			crouch = false;
			collide.enabled = true;
			crouchCollide.enabled = false;
		}
	}

	private void Movement() {
		MoveInput();					//call move input
		JumpInput();					//call jump input
		rb.velocity = inputVector;		//set velocity of the player based on the player input
	}

	private void Attack() {
		if (attackAllowed <= 0) { //countdown hit zero meaning you're allowed to attack
			if ((Input.GetMouseButton(0) || Input.GetButton("Fire2")) && isGrounded) { //when you click and you're on the ground you attack
				attack = true;
				enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, attackDetect); //checks all enemies in the attack area
				foreach (Collider2D enemy in enemies) {
					enemy.GetComponent<Enemy>().Damage(10);
				}
				attackAllowed = timeBtwnAttack; //as soon as we click we don't allow another attack by setting timer until the timer goes back to zero
			}
		} else {
			attackAllowed -= Time.deltaTime;    //countdown the attack reset timer
			if(Input.GetMouseButtonUp(0) || !isGrounded || Input.GetButtonUp("Fire2")) {	//if the player lets go of the mouse
				attack = false;								//the player is not attacking
			}
		}
		if (Input.GetMouseButtonUp(0) || !isGrounded || Input.GetButtonUp("Fire2")) { //if the player lets go of the mouse
			attack = false;                             //the player is not attacking
		}
	}

	public void Block() {
		if ((Input.GetMouseButton(1) || Input.GetButton("Fire3")) && isGrounded) {
			block = true;
			enemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius, attackDetect);	//same area and radius as where the player attacks
			foreach (Collider2D enemy in enemies) {
				/*if (enemy.GetComponent<Enemy>().attackAllowed <= 0 && enemy.GetComponent<Enemy>().playerCollider && block) {
					rb.AddForce(new Vector2(-1000 * transform.localScale.x, 0));
				}*/
			}

		} else if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("Fire3")) {
			block = false;
		}
	}

	private void UseSpecial() {
		if (specialAllowed && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))) {
			if (choice == 0 && amelia) {
				Climb();
			} else if (choice == 1 && brett) {
				special = 0;
				if (transform.localScale.x == 1 && !boatExists) {
					Instantiate(boat, transform.position + new Vector3(3f, 0, 0), Quaternion.identity);
					boatExists = true;
				}
			} else if (choice == 2 && casey) {
				special = 0;
				if (transform.localScale.x == 1) {
					Instantiate(bomb, transform.position + new Vector3(1.2f, 0, 0), Quaternion.identity);
				} else {
					Instantiate(bomb, transform.position + new Vector3(-1.2f, 0, 0), Quaternion.identity);
				}
			} else if (choice == 3 && dennis) {
				isFlying = true;
				rb.AddForce(Vector2.up * 10000);
				rb.gravityScale = 0;
				Instantiate(jumpdust, transform.position, Quaternion.identity);
			}
		}
	}

	private void Climb() {
		climb = !climb;
	}


	private void AnimationState() {     //sets animation states for the player

		if (!block && !attack && !crouch && climb) {            //if the player is climbing
			animator.SetInteger("state", 6);
		} else if (isGrounded && !block && !attack && crouch) {			//if the player is on the ground and crouching
			animator.SetInteger("state", 5);
		} else if (isGrounded && !crouch && !attack && block) {		//if the player is on the ground and blocking
			animator.SetInteger("state", 4);
		} else if (isGrounded && !crouch && !block && attack) {		//if the player is on the ground and attacking
			animator.SetInteger("state", 3);
		} else if (!isGrounded && !isFlying) {						//if the player is not on the ground (jumping)
			animator.SetInteger("state", 2);
		} else if (isGrounded && inputVector.x != 0 && !attack && !block && !crouch) {		//if the player is moving but on the ground
			animator.SetInteger("state", 1);
		} else {													//if the player is idle
			animator.SetInteger("state", 0);
		}
	}

	private void SwitchCharacter() {
		if((Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetButtonDown("SwitchRight")) && choice < characters.Length-1) {
			choice++;
		} else if((Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetButtonDown("SwitchLeft")) && choice > 0) {
			choice--;
		}
		sprite.color = characters[choice];
	}

	private void Health() {
		healthbar.fillAmount = health / 100f;

		for(int i = 0; i < enemy.Length; i++) {
			if (enemy[i].attackAllowed <= 0 && enemy[i].playerCollider && !block) {
				health -= 10;
			}
		}

		if(health <= 0) {
			gm.ReloadLevel();
		}
	}

	private void Special() {
		specialbar.fillAmount = special / 100f;
		isReady = Physics2D.OverlapCircle(groundCheck.position, groundRadius, specialDetect);
		if (specialbar.fillAmount == 1 && isReady) {
			specialAllowed = true;
		} else {
			specialAllowed = false;
		}
		if (special < 100f) {
			special += Time.deltaTime * specialFillSpeed;
		}
	}

	private void DropShadow() {
		hit = Physics2D.Raycast(transform.position, Vector2.down, 4f, groundDetect);
		if (hit) {
			shadow.SetActive(true);
			float dist = Vector3.Distance(transform.position, hit.point);
			shadow.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
		} else {
			shadow.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("spikes")) {
			health -= 100;
		}
		if (collision.gameObject.tag.Equals("enemy")) {
			health -= 10;
		}
		if (collision.gameObject.tag.Equals("bPlatform")) {
			rb.AddForce(new Vector2(0, 1500));
		}
		if (collision.gameObject.tag.Equals("Respawn")) {
			gm.ReloadLevel();
		}
		if (collision.gameObject.tag.Equals("a")) {
			amelia = true;
			brett = casey = dennis = false;
		}
		if (collision.gameObject.tag.Equals("b")) {
			brett = true;
			amelia = casey = dennis = false;
		}
		if (collision.gameObject.tag.Equals("c")) {
			casey = true;
			brett = amelia = dennis = false;
		}
		if (collision.gameObject.tag.Equals("d")) {
			dennis = true;
			brett = casey = amelia = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag.Equals("Respawn")) {
			gm.ReloadLevel();
		} else if (collision.gameObject.tag.Equals("checkpoint")) {
			gm.checkpointPos = collision.gameObject.transform.position;
		}
		if (collision.gameObject.tag.Equals("coin")) {
			gm.AddScore();
			if(collision.gameObject.transform.position == gm.coin1pos) {
				GameManager.coin1 = false;
			} else if (collision.gameObject.transform.position == gm.coin2pos) {
				GameManager.coin2 = false;
			} else if (collision.gameObject.transform.position == gm.coin3pos) {
				GameManager.coin3 = false;
			}
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.tag.Equals("spikes")) {
			health -= 100;
		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if(collision.gameObject.tag.Equals("lPlatform") && climb){
			rb.gravityScale = 0;
		}
		if (collision.gameObject.tag.Equals("Finish") && (Input.GetKey(KeyCode.Space) || Input.GetButton("Jump"))) {
			gm.NextLevel();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag.Equals("lPlatform") && climb) {
			rb.gravityScale = 5;
			Climb();
		}
	}

}
