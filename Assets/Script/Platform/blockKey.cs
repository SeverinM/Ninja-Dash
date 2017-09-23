using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockKey : MonoBehaviour {

    public GameObject key;

    [Range(0,10)]
    public float duree;

    private bool started = false;
    private float time; //time when the gameobject will disappear

	// Use this for initialization
	void Start () {
        if (key != null)
            key.gameObject.GetComponent<key>().Add(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (started && time < Time.timeSinceLevelLoad)
            GetComponent<Animator>().SetTrigger("start");
	}

    public void Notif()
    {
        started = true;
        time = Time.timeSinceLevelLoad + duree;
    }
}
