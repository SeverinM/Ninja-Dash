using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    Transform target;
    public string name = "Player"; 

	// Use this for initialization
	void Start () {
        target = GameObject.Find(name).transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.up = target.position - transform.position;
            transform.up = new Vector3(transform.up.x, transform.up.y, 0);
        }
        
	}
}
