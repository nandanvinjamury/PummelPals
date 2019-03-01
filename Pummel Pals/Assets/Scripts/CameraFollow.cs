using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private Vector3 targetPosition;		//position of the player
	private GameObject player;          //the player
	Vector3 velocity = Vector3.zero;	//no additional velocity
	[Range(0f,1f)]						//keeping the float within a small range
	public float cameraSlow;            //a small value that determines how long it takes for the camera to move
	public float minXClamp, maxXClamp, minYClamp, maxYClamp;	//values that determine where the camera should clamp to (depending on the level)

	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<PlayerController>().gameObject;		//find the player
		targetPosition = player.transform.position;                                 //set camera target as the player's position to follow the player
	}

	// Update is called once per frame
	private void Update() {
		targetPosition = player.transform.position;									//update camera target's position
	}


	private void LateUpdate() {

		transform.position = Vector3.SmoothDamp(transform.position, targetPosition + Vector3.forward * -10, ref velocity, cameraSlow);  //let the camera follow a little later for a smoother effect
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXClamp, maxXClamp), Mathf.Clamp(transform.position.y, minYClamp, maxYClamp), transform.position.z);
		//(-40,-5.5f) to (160, 17.5f)
	}
}
