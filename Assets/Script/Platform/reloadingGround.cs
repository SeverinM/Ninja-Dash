using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadingGround : MonoBehaviour {

    public float reloadingRate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<playerScript>().SetCharge(reloadingRate);
            coll.gameObject.GetComponent<playerScript>().groundTouch();
            coll.gameObject.GetComponent<playerScript>().rotating = false;
            coll.gameObject.GetComponent<playerScript>().ontheGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            coll.gameObject.GetComponent<playerScript>().ontheGround = false;
    }
}
