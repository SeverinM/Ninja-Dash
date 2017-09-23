using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour {

	public static List<int> idList = new List<int> ();
	public static bool debug = true;
    public int id;
    bool taken = false;
    bool alreadyTaken;

	// Use this for initialization
	void Start () {
        if (debug)
            Debuger();

        alreadyTaken = DAO.estPrise(id);

        if (alreadyTaken)
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.6f);

		if (!(idList.Contains (id)))
			idList.Add (id);
		else
			Debug.Log ("id number " + System.Convert.ToString (id) + " already exist");
	}

    public void Prise()
    {
        DAO.setPrise(id,true);
    }

    void PriseTemp()
    {
        taken = true;
        GetComponent<Animator>().SetTrigger("taken");
    }

    public bool isTaken()
    {
        return taken;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && !alreadyTaken)
        {
            PriseTemp();
        }
    }

    void Debuger()
    {
        for (int i = 0; i < 10; i++)
        {
            DAO.setPrise(i, false);
        }
        DAO.setPieces(0);
    }
}
