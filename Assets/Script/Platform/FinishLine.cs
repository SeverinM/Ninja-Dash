using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

	private static GameObject instance;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this.gameObject;
		else
			Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
