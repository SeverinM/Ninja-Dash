using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		col.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 500f));
	}
}
