using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicBackground : MonoBehaviour {

    [Range(0, 30)]
    public float offsetModifier;
    public bool followX = true;
    public bool followY = true;
    Vector3 previousPos;

    Vector2 offset = Vector3.zero;

	// Use this for initialization
	void Start () {
        previousPos = Camera.main.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        offset -= (Vector2)((Camera.main.transform.position - previousPos).normalized * Time.deltaTime * offsetModifier * 0.25f);
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0) + (new Vector3((followX ? offset.x : 0), (followY ? offset.y : 0), 0));
        previousPos = Camera.main.transform.position;
    }
}
