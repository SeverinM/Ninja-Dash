using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public GameObject playerInstance;
	private static GameObject instance;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this.gameObject;
		else
			Destroy (this.gameObject);
		Instantiate (playerInstance, transform.position, new Quaternion ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
