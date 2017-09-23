using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour {

    private int currentNode; //Indice de la liste indiquant le chemin a suivre
    public List<Vector3> Nodeslist; //Liste des chemins que la plateforme doit suivre en relatif (ex : (1,1,0) signifie que l'on augmente x de 1 et y de 1), toujours mettre z à 0 
	public List<float> speedList; //Liste des vitesses à prendre en relation avec NodesList
    private Vector3 previousPosition; //Derniere position enregistré lorsqu'un noeud est atteint (en absolu)
    public bool loop; //Quand tous les nodes ont été fait , reprendre au debut ou arreter la pleteforme ?
    private bool canMove = true; //Bloque tout deplacement quand mis à 0
    private Vector3 roundPos; //Position actuelle arrondi
    public float defaultspeed; //Vitesse à prendre quand speedList ne peut donner de vitesse
	private float speed; //Vitesse actuelle de la plateforme
	public bool turnX = false; //retourner la plateforme si l'on va a gauche ?
    public bool turnY = false; //retourner la plateforme si l'on va en bas ? 
    public int restartLoopIndex = 0; //si l'on fait une boucle , a quel indice doit on reprendre ?
	public bool destroyEnd = false; //Detruit l'objet s'il atteint la fin du dernier noeud
    public List<float> waitingList; //Indique combien de temps attendre AVANT un noeud
    private float time;

	// Use this for initialization
	void Start () {
		speed = defaultspeed;
        previousPosition = this.transform.position;
        currentNode = 0;
		UpdateRotation ();
		previousPosition = transform.position;

		try{
			speed = speedList[currentNode];
		}
		catch{
			speed = defaultspeed;
		}

	}
	
	// Update is called once per frame
	void Update () {
        if (canMove && Time.timeSinceLevelLoad - time > (waitingList.Count > currentNode ? waitingList[currentNode] : 0))
        {
            roundPos = transform.position - (previousPosition + Nodeslist[currentNode]);
            roundPos = new Vector3(Mathf.Round(roundPos.x), Mathf.Round(roundPos.y), roundPos.z).normalized;
            transform.Translate(Nodeslist[currentNode].normalized * (Time.deltaTime * 10) * speed);
			if (roundPos == Vector3.zero || tooFar(roundPos))
                UpdateNode();
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.Translate(Nodeslist[currentNode].normalized * (Time.deltaTime * 10) * speed);
        }
    }

    void UpdateNode()
    {
        previousPosition = this.transform.position;
        if (currentNode < Nodeslist.Count - 1) //Il reste des nodes à parcourir
        {
            currentNode += 1;
            time = Time.timeSinceLevelLoad;
            UpdateRotation ();
			try{
				speed = speedList[currentNode];
			}
			catch{
				speed = defaultspeed;
			}
        }
        else
        {
            if (destroyEnd) //On detruit si c'est la fin...
            {
                Destroy(this.gameObject);
            }
            if (loop) //...sinon on recommence
            {
                currentNode = restartLoopIndex;
                time = Time.timeSinceLevelLoad;
                UpdateRotation ();
				try{
					speed = speedList[currentNode];
				}
				catch{
					speed = defaultspeed;
				}
            }
            else
            {
                canMove = false;
                speed = 0;
            }
        }
    }

	void UpdateRotation(){
		if (turnX) {
			GetComponent<SpriteRenderer> ().flipX = !(Nodeslist [currentNode].x < 0);
		}
        if (turnY)
        {
            GetComponent<SpriteRenderer>().flipY = !(Nodeslist[currentNode].y < 0);
        }
	}

	bool tooFar(Vector3 relativePos){//Fonction appellé à chaque frame pour verifier que l'on a pas dépassé le node, surtout sur des grosses vitesses
		bool ok = !(relativePos.x * Nodeslist [currentNode].x <= 0 && relativePos.y * Nodeslist [currentNode].y <= 0);
		return ok;
	}
}
