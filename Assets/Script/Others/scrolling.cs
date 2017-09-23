using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allow to make custom scrolling with a camera, this script won't do anything if "followX" and "followY" are enabled on playerScript
/// </summary>
public class scrolling : MonoBehaviour {

	public float speedX = 0; //Speed on the horizontal axis
	public float speedY = 0; //Spped on the vertical axis
	private bool stop = false; //Camera only move when set to false
	private bool sameDirectionX; //Is the player going in the same direction (left / right) than the scrolling ?
	private bool sameDirectionY; //Is the player going in the same direction (up / down) than the scrolling ?
	private Rigidbody2D playerSpeed; //Rigidbody component of player
	public Vector3 stopPos; //When this position is reached the camera won't move anymore 
    public GameObject player;

	[Tooltip("The higher is number is the further the camera will go if you go on the same direction than the scroll")]
	[Range(0,1)]
	public float forwardFactorX = 0;

	[Tooltip("The higher is number is the further the camera will go if you go on the same direction than the scroll")]
	[Range(0,1)]
	public float forwardFactorY = 0;

	// Use this for initialization
	void Start () {
        if (player == null)
		    player = GameObject.Find ("Player");
		playerSpeed = player.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		sameDirectionX = false;
		sameDirectionY = true;
		if (playerSpeed != null) {
			if (speedX * playerSpeed.velocity.x >= 0)
				sameDirectionX = true;
			if (speedY * playerSpeed.velocity.y >= 0)
				sameDirectionY = true;
		}
		if (!(stop))
			transform.position += new Vector3 (speedX * Time.fixedDeltaTime * Time.timeScale, speedY * Time.fixedDeltaTime * Time.timeScale, 0);

		if (!(player.GetComponent<playerScript> ().FollowX) && sameDirectionX && !(stop))
			transform.position += new Vector3 ((playerSpeed.velocity.x * Time.fixedDeltaTime * forwardFactorX), 0, 0);

		if (!(player.GetComponent<playerScript> ().FollowY) && sameDirectionY && !(stop))
			transform.position += new Vector3 (0, (playerSpeed.velocity.y * Time.fixedDeltaTime * forwardFactorY), 0);	
	}
}
