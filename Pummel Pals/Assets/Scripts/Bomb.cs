using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	private float timer;
	public float aoeRadius;
	public LayerMask whatToExplode;
	private Collider2D[] destructibles;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		timer = 2; 
	}
	
	// Update is called once per frame
	void Update () {
		if(timer <= 0.5f) {
			anim.SetBool("explosion", true);
		}
		if(timer <= 0) {
			Explode();
		} else {
			timer -= Time.deltaTime;
		}
	}

	private void Explode() {
		destructibles = Physics2D.OverlapCircleAll(transform.position, aoeRadius, whatToExplode);
		foreach (Collider2D destructible in destructibles) {
			Destroy(destructible.gameObject);
		}
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(transform.position, aoeRadius);
	}

}
