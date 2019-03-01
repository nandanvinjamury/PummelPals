using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {

	public TextMeshProUGUI scoreValue;
	
	// Update is called once per frame
	void Update () {
		DisplayScore();
	}

	public void DisplayScore() {
		scoreValue.text = "Coins Found: " + GameManager.score.ToString() + "/3";
	}
}
