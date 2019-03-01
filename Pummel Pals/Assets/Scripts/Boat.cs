using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {

	public float speed;
	private PlayerController pc;
	public bool boatMoving;
	[SerializeField] private BoxCollider2D box, box2;
	public bool boatCheck;

	// Use this for initialization
	void Start () {
		pc = GameObject.Find("player").GetComponent<PlayerController>();
		boatMoving = false;
		box = GetComponent<BoxCollider2D>();
		boatCheck = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (boatMoving) {
			transform.Translate(speed * Time.deltaTime * Vector3.right);
		} else {
			transform.Translate(Vector3.zero);
		}
		if (box.bounds.Intersects(box2.bounds)) {
			boatMoving = false;
			pc.boatExists = false;
			pc.gameObject.transform.SetParent(null);
			Destroy(gameObject, 1f);
		}

		if (gameObject.transform.childCount == 0) {
			boatCheck = true;
			Invoke("CheckChild", 3f);
		}
		
		if(gameObject.transform.childCount > 0) {
			boatCheck = false;
			CancelInvoke();
		}

	}

	private void CheckChild() {
		if(boatCheck) {
			boatMoving = false;
			pc.boatExists = false;
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("Player") && pc.boatExists) {
			boatMoving = true;
			collision.collider.transform.SetParent(transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			//pc.boatExists = false;
			//boatMoving = false;
			collision.collider.transform.SetParent(null);
			//Destroy(gameObject, 0.5f);
		}
	}
}
