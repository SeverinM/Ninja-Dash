using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ball : MonoBehaviour {

	private List<GameObject> playerHitList;
	private Rigidbody2D physicPlayer; //Physic component of player who hit the projtectiles
	private Rigidbody2D physic; //Physic component of the gameobject;
    public bool deflectable = true;

	// Use this for initialization
	void Start () {
		physic = GetComponent<Rigidbody2D> ();
		playerHitList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Hit (Collider2D coll)
	{
		physicPlayer = coll.gameObject.GetComponent<Rigidbody2D> ();
		if (coll.gameObject.tag == "Weapon" && deflectable) {
			physicPlayer = coll.transform.parent.gameObject.GetComponent<Rigidbody2D> ();
			physic.velocity = physicPlayer.velocity * 1.2f;
			playerHitList.Add (physicPlayer.gameObject);
			this.tag = "Deflected";
		}
	}

	public void OnTriggerEnter2D(Collider2D coll){
		Hit (coll);

        if (coll.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
