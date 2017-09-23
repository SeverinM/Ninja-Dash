using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularSaw : MonoBehaviour {

    public float speed;
    public bool rotatingLeft;

	// Use this for initialization
	void Start () {
        speed /= 100;
	}
	
	// Update is called once per frame
	void Update () {
		try{
        transform.Rotate(Vector3.forward * (rotatingLeft ? -1 : 1) * Time.deltaTime * (speed * 500));
		}
		catch{
		}
	}
}
