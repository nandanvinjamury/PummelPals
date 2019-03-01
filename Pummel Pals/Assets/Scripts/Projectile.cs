using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private Rigidbody2D rb;
	public Vector3 direction;
	public float speed;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Invoke("DestroyObject", 3f);
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = direction * speed * Time.deltaTime;
	}

	void DestroyObject() {
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("platform") || collision.gameObject.tag.Equals("bPlatform") || collision.gameObject.tag.Equals("mPlatform") || collision.gameObject.tag.Equals("tPlatform") || collision.gameObject.tag.Equals("fPlatform")) {
			Destroy(gameObject);
		}
	}
}
