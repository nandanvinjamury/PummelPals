using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	public Vector2[] board;
	public GameObject darksquare, lightsquare;
	private int counter = 0;
	private bool isLight;

	// Use this for initialization
	void Start () {
		isLight = true;
		board = new Vector2[64];
		board[0] = new Vector2(0, 0);
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				board[counter] = new Vector2(j, i);
				counter++;
			}
		}

		for(int i=0; i<64; i++) {
			if (isLight)
				Instantiate(lightsquare, board[i], Quaternion.identity);
			if (!isLight)
				Instantiate(darksquare, board[i], Quaternion.identity);
			if(i%8 != 7) {
				isLight = !isLight;
			}
		}

	}
}
