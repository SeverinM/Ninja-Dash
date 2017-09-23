using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchDown : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
            transform.parent.GetComponent<playerScript>().touchdown(true);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
            transform.parent.GetComponent<playerScript>().touchdown(false);
    }
}
