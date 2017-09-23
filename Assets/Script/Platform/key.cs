using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour {

    private List<GameObject> listeblock = new List<GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Add(GameObject gob)
    {
        listeblock.Add(gob);
    }

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            foreach (GameObject gob in listeblock)
                gob.GetComponent<blockKey>().Notif();
            Destroy(this.gameObject);
        }
            
    }
}
