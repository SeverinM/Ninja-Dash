using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enable : MonoBehaviour {

	public int lvl;

	// Use this for initialization
	void Start () {
		GetComponent<UnityEngine.UI.Button> ().interactable = (GameObject.Find ("GameManager").GetComponent<GestionJeu> ().getSoft () >= lvl);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
