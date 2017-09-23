using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchUp : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
            transform.parent.GetComponent<playerScript>().touchUp(true);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Wall")
            transform.parent.GetComponent<playerScript>().touchUp(false);
    }
}
