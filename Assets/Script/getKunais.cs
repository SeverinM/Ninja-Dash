using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getKunais : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Text>().text = " X " + System.Convert.ToString(DAO.getPieces());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
