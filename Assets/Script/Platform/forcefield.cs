using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forcefield : MonoBehaviour {

    public float strenght;
    private Vector2 strenghtvec;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            strenghtvec = new Vector2(coll.gameObject.transform.position.x - transform.position.x, coll.gameObject.transform.position.y - transform.position.y);
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(strenghtvec.normalized * ((strenght * 10000) / strenghtvec.magnitude));
        }

    }
}
