using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// A little home-made script to align plateform and to some stuff
/// </summary>
public class MappingEditor : EditorWindow {

	private GameObject selection; //The first selected GameObject
	private GameObject instantiateObj; 
	private GameObject instantiated;
	private float startingX = 0;
	private float startingY = 0;
	private float stepX = 1; //When you move to right / left , for how many unit do you move ?
	private float stepY = 1; //When you move to down / up , for how many unit do you move ?
    private float positionX = 0; //X position of recorded gameObject
	private float positionY = 0; //Y position of recorded gameObject
	private string error = ""; //Unused
	private float sizeX = 1;
	private float sizeY = 1;
    private Vector3 distance = Vector3.zero; //Distance between 2 GO
    public bool halfDistance = false;
    private bool moveObjects = false; //Selected gameObject may move ?
    private bool autoCreate = false; //Instantiate a new gameObject at every move ?
    private float originalSpeed = 0;
    private float stepSpeed = 0;
    private bool reversed = false;

	[MenuItem ("Window/Mapping Editor")]
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(MappingEditor));
	}

	void OnGUI () {

		if (GUILayout.Button ("Copier taille gameobject (selection 1 GO)")) {
			if (Selection.gameObjects.Length == 1) {
				selection = Selection.gameObjects [0];
				sizeX = selection.transform.lossyScale.x;
				sizeY = selection.transform.lossyScale.y;
			}
		}
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Taille X: " + sizeX);
		EditorGUILayout.LabelField ("Taille Y : " + sizeY);
		EditorGUILayout.EndHorizontal ();


		if (GUILayout.Button("copier 'pas' + centrer curseur (selection 1 GO)")){
			if (Selection.gameObjects.Length == 1) {
				selection = Selection.gameObjects [0];
				stepX = selection.GetComponent<SpriteRenderer> ().bounds.size.x;
				stepY = selection.GetComponent<SpriteRenderer> ().bounds.size.y;
				positionX = selection.transform.position.x;
				positionY = selection.transform.position.y;
			}
		}
		stepX = EditorGUILayout.FloatField ("Step X", stepX);
		stepY = EditorGUILayout.FloatField ("Step Y", stepY);

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("position X : " + positionX);
		EditorGUILayout.LabelField ("position Y : " + positionY);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginVertical ();
        if (GUILayout.Button("Haut", GUILayout.Width(100), GUILayout.Height(30))) {
            positionY += stepY / (halfDistance ? 2 : 1);
            if (moveObjects)
                MoveGameObject(new Vector3(0, stepY / (halfDistance ? 2 : 1), 0));
            if (autoCreate) Create();
        }

		EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button("Gauche", GUILayout.Width(100), GUILayout.Height(30))) {
            positionX -= stepX / (halfDistance ? 2 : 1);
            if (moveObjects)
                MoveGameObject(new Vector3(-stepX / (halfDistance ? 2 : 1) ,0, 0));
            if (autoCreate) Create();
        }

        if (GUILayout.Button("Droite", GUILayout.Width(100), GUILayout.Height(30))) {
            positionX += stepX / (halfDistance ? 2 : 1);
            if (moveObjects)
                MoveGameObject(new Vector3(stepX / (halfDistance ? 2 : 1),0, 0));
            if (autoCreate) Create();
        }

        halfDistance = GUILayout.Toggle(halfDistance, "Demi distance");
        moveObjects = GUILayout.Toggle(moveObjects, "Deplacer selection");
        autoCreate = GUILayout.Toggle(autoCreate, "Auto create");
        EditorGUILayout.EndHorizontal ();

        if (GUILayout.Button("Bas", GUILayout.Width(100), GUILayout.Height(30))) {
            positionY -= stepY / (halfDistance ? 2 : 1);
            if (moveObjects)
                MoveGameObject(new Vector3(0, -stepY / (halfDistance ? 2 : 1), 0));
            if (autoCreate) Create();
        }
        EditorGUILayout.EndVertical ();

		instantiateObj = (GameObject)EditorGUILayout.ObjectField ("test",instantiateObj,typeof(GameObject),true);

		if (GUILayout.Button("Create")){
            Create();
		}

        if (GUILayout.Button("Afficher distance (selection 2 GO)"))
        {
            if (Selection.gameObjects.Length == 2)
            {
                distance = Selection.gameObjects[0].transform.position - Selection.gameObjects[1].transform.position;
            }
        }

        EditorGUILayout.LabelField("La distance entre les deux objets est de " + distance + " soit une longueur de " + distance.magnitude);

        stepSpeed = EditorGUILayout.FloatField("temps original", stepSpeed);
        originalSpeed = EditorGUILayout.FloatField("ecart de temps", originalSpeed);
        reversed = EditorGUILayout.Toggle("ordre inverse" , reversed);

        if (GUILayout.Button("Creer destrcution de block en serie"))
            setSpeed(stepSpeed, originalSpeed);
	}

    void MoveGameObject(Vector3 vec)
    {
        foreach(GameObject gob in Selection.gameObjects)
        {
            gob.transform.position += vec;
        }
    }

    void Create()
    {
        instantiated = PrefabUtility.InstantiatePrefab(instantiateObj) as GameObject;
        instantiated.transform.position = new Vector3(positionX, positionY, 0);
        Vector3 originScale = instantiated.transform.lossyScale;
        float scalingX = originScale.x / sizeX;
        float scalingY = originScale.y / sizeY;
        instantiated.transform.localScale = new Vector3(instantiated.transform.localScale.x / scalingX, instantiated.transform.localScale.y / scalingY, 1);
    }

    void setSpeed(float originalSpeed, float step)
    {
        float speed = originalSpeed;
        IEnumerable<GameObject> gobList = Selection.gameObjects.OrderBy(gameobj => gameobj.transform.GetSiblingIndex());
        if (reversed)
            gobList = gobList.Reverse();
        foreach (GameObject gob in gobList)
        {
            try
            {
                Debug.Log("delai placé a " + speed + " position " + gob.transform.GetSiblingIndex());
                gob.GetComponent<blockKey>().duree = speed;
            }
            catch
            {
                Debug.Log("Erreur, application impossible sur " + gob.name + " a la position " + gob.transform.GetSiblingIndex());
            }
            speed += step;
        }
    }
}
