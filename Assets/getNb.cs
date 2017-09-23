using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getNb : MonoBehaviour {

    int nb = 0;
    int nbTaken = 0;

	// Use this for initialization
	void Start () {
        foreach (Kunai kun in GameObject.FindObjectsOfType<Kunai>())
        {
            nb += 1;
            if (DAO.estPrise(kun.id))
                nbTaken += 1;
        }

        GetComponent<UnityEngine.UI.Text>().text = System.Convert.ToString(nbTaken) + " / " + System.Convert.ToString(nb);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
