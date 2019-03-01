using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour {

	private GameManager gm;
	public Vector3 checkpointPos;
	[SerializeField] private GameObject coin;
	public static bool coin1, coin2, coin3;
	public Vector3 coin1pos, coin2pos, coin3pos;
	public static int score;
	public GameObject pauseMenu;
	private PlayerController pc;


	public void Awake() {

		gm = GameObject.FindObjectOfType<GameManager>();
	}

	

	public void NextLevel() {
		gm.NextLevel();
	}

	public void PrevLevel() {
		gm.PreviousLevel();
	}


}
