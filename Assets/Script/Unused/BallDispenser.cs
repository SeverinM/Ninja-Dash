using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDispenser : MonoBehaviour {

	private int cadenceIndex;
	public List<float> cadenceList;
    public float speed;
    public Vector2 dir;
    private float time;
	public GameObject ball;
	private GameObject instanciated;
	public float sizeModifier = 3; 

	// Use this for initialization
	void Start () {
		cadenceIndex = 0;
		time = Time.timeSinceLevelLoad + cadenceList[cadenceIndex];
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad - time > cadenceList[cadenceIndex]) {
			UpdateNode ();
			instanciated = Instantiate (ball, transform.position, new Quaternion ()) as GameObject;
            if (instanciated.GetComponent<movingPlatform>() != null)
                instanciated.GetComponent<movingPlatform>().enabled = true;
			instanciated.transform.localScale *= sizeModifier;
            instanciated.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            instanciated.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
		}
	}

	void UpdateNode(){
		cadenceIndex += 1;
		if (cadenceIndex > cadenceList.Count - 1)
			cadenceIndex = 0;
		time = Time.timeSinceLevelLoad + cadenceList[cadenceIndex];
	}
}
