using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

	private GameManager gm;

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
