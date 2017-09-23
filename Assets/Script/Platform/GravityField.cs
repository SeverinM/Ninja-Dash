using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour {

    public string gravityModifier;
    public float gravityScale = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
			coll.gameObject.GetComponent<playerScript>().enterGravityField(gravityModifier, gravityScale);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
			coll.gameObject.GetComponent<playerScript>().exitGravityField(gravityModifier);
    }
}
