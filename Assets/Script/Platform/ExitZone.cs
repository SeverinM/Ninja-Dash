using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<playerScript>().Kill();
        }

        if (coll.tag == "Undashable" ||coll.tag == "Ball" || coll.tag == "Ennemy")
            Destroy(coll.gameObject);
    }
}
