using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Text>().text = chrono.strTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
