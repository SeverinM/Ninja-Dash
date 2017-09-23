using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnnemie : MonoBehaviour {

	private Vector3 initialSize;
	private Vector3 previousPos;
	private SpriteRenderer box;
	private Rigidbody2D physic;
	public GameObject nuage;
	public int life = 3; //How many slash does it take to kill this ennemy ?
	private int maxHP; //Life cannot be greater than this value
    public int reward = 33; //Recharge rate when the ennemy is killed
    public playerScript scrpt;

	// Use this for initialization
	void Start () {
        if (scrpt == null)
            scrpt = GameObject.Find("Player").GetComponent<playerScript>();

		initialSize = transform.localScale;
		previousPos = transform.position;
		maxHP = life;
		physic = GetComponent<Rigidbody2D> ();
		box = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		box.color += new Color (0, 0.01f, 0.01f,0);
	}

	public virtual void ChangeLife(int nbLife,Collider2D coll){ //Called when this ennemy lose or win a life
		life += nbLife;
		if (life > maxHP)
			life = maxHP;
		if (life <= 0)
			Kill (coll);
	}

	public virtual void Kill(Collider2D coll){ //Called when this ennemy died
		Destroy (this.gameObject);
		Instantiate (nuage, transform.position, new Quaternion ());
		try{
			scrpt.SetCharge(reward);
		}
		catch{
			Debug.Log ("not found");
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Weapon" || coll.tag == "Deflected")
			Hit (coll);
	}

	public virtual void Hit(Collider2D coll){ //Called when an ennemi is hit
		ChangeLife (-1,coll);
        GetComponent<SpriteRenderer> ().color = Color.red;
	}
}
