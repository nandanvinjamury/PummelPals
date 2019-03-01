﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

	private static GameManager instance;
	public Vector3 checkpointPos;
	[SerializeField] private GameObject coin;
	public static bool coin1, coin2, coin3;
	public Vector3 coin1pos, coin2pos, coin3pos;
	public static int score;
	public GameObject pauseMenu;

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(instance);
		} else {
			Destroy(gameObject);
		}

		if (pauseMenu != null) {
			pauseMenu.SetActive(false);
		}

		if (coin1) {
			Instantiate(coin, coin1pos, Quaternion.identity);
		}
		if (coin2) {
			Instantiate(coin, coin2pos, Quaternion.identity);
		}
		if (coin3) {
			Instantiate(coin, coin3pos, Quaternion.identity);
		}
	}

	private void Start() {

		if (pauseMenu != null) {
			pauseMenu.SetActive(false);
		}
		coin1 = true;
		coin2 = true;
		coin3 = true;
		Debug.Log("coin1: " + coin1);
		Debug.Log("coin2: " + coin2);
		Debug.Log("coin3: " + coin3);
		if (coin1) {
			Instantiate(coin, coin1pos, Quaternion.identity);
		}
		if (coin2) {
			Instantiate(coin, coin2pos, Quaternion.identity);
		}
		if (coin3) {
			Instantiate(coin, coin3pos, Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update () {

		if(pauseMenu != null && (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause"))) {
			pauseMenu.SetActive(!pauseMenu.activeSelf);
			if (Time.timeScale == 0) {
				Time.timeScale = 1;
			} else {
				Time.timeScale = 0;
			}
		}

		if(Time.timeScale == 0 && (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Menu"))) {
			SceneManager.LoadScene(0);
		}

	}

	public void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void NextLevel() {
		checkpointPos = new Vector3(0, 0, 0);
		score = 0;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void PreviousLevel() {
		checkpointPos = new Vector3(0, 0, 0);
		score = 0;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void AddScore() {
		score++;
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireSphere(coin1pos, 0.25f);
		Gizmos.DrawWireSphere(coin2pos, 0.25f);
		Gizmos.DrawWireSphere(coin3pos, 0.25f);
	}

}