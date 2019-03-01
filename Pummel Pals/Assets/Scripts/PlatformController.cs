using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlatformController : MonoBehaviour
{

	Rigidbody2D rb;
	private float speed;
	public Vector3[] targets;
	private float distBetween;
	private float movePercent;
	private int fromPoint;
	private int toPoint;
	private Vector3 newPosition;
	public bool cycle;

	// Use this for initialization
	void Start() {
		rb = GetComponent<Rigidbody2D>();
		speed = 5;
		distBetween = 1;
		fromPoint = 0;
		movePercent = 0;
	}

	// Update is called once per frame
	void Update() {
		if (gameObject.tag.Equals("mPlatform")) {
			MovingPlatforms();
			for (int i = 0; i < targets.Length; i++) {
				if (cycle) {
					DottedLine.Instance.DrawDottedLine(targets[i], targets[(i + 1) % targets.Length]);
				} else if(!cycle && i < targets.Length-1) {
					DottedLine.Instance.DrawDottedLine(targets[i], targets[i + 1]);
				}
			}
		}
	}

	private void MovingPlatforms() {
		fromPoint = fromPoint % targets.Length;
		toPoint = (fromPoint + 1) % targets.Length;
		distBetween = Vector3.Distance(targets[fromPoint], targets[toPoint]);
		movePercent += Time.deltaTime * speed / distBetween;
		movePercent = Mathf.Clamp01(movePercent);
		newPosition = Vector3.Lerp(targets[fromPoint], targets[toPoint], movePercent);
		if (movePercent >= 1) {
			movePercent = 0;
			fromPoint++;
			if (!cycle && fromPoint == targets.Length - 1) {
				System.Array.Reverse(targets);
				fromPoint = 0;
			}
		}
		transform.Translate(newPosition - transform.position);

	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			if (gameObject.tag.Equals("fPlatform")) {
				rb.velocity = new Vector2(0, -5);
			}
			if (gameObject.tag.Equals("mPlatform")) {
				collision.collider.transform.SetParent(transform);
			}

		} else {
			if (gameObject.tag.Equals("fPlatform")) {
				Destroy(gameObject);
			}
		}

	}

	private void OnCollisionStay2D(Collision2D collision) {
		if (gameObject.tag.Equals("mPlatform")) {
			collision.collider.transform.SetParent(transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			if (gameObject.tag.Equals("mPlatform")) {
				collision.collider.transform.SetParent(null);
			}

		}
	}

	private void OnDrawGizmosSelected() {
		for (int i = 0; i < targets.Length; i++)
			Gizmos.DrawWireSphere(targets[i], 0.5f);
	}


}
