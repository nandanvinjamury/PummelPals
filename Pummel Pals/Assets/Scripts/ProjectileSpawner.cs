using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

	public GameObject projectile;

	void Start () {
		InvokeRepeating("Spawn", 2f, 2f);
	}

	private void Spawn() {
		Instantiate(projectile, transform.position, Quaternion.identity);
	}

}
