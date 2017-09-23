using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {

	private Vector3 direction;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		transform.position = transform.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, 0.015f);
		transform.position = transform.parent.position;
		direction = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		direction -= transform.parent.position;
		direction = new Vector3 (direction.x, direction.y, 0);
		transform.up = direction;

	}

	public void Init(){
		Start ();
	}
}
