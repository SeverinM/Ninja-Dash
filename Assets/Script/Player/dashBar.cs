using UnityEngine;
using System.Collections;

public class dashBar : MonoBehaviour {

	private float lenght = 0.5f; //The lenght of a complete bar
	private float progress = 1; //The dash reload progress, value must be between 0 and 1
	private LineRenderer line;
	private Vector2 position; //The position of the first point

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		line.SetPosition (0, new Vector3 (position.x, position.y, 0));
		line.SetPosition (1, new Vector3 (position.x + (lenght * progress), position.y, 0));
	}

	public void setPosition(Vector2 pos){ //The position of the top left point of the bar
		position = pos;
	}

	public void setProgress(float progr){ //The portion of the bar displayed
		progress = progr;
	}

	public void setlenght(float len){ //The length of the complete bar
		lenght = len;
	}

	public float getProgress(){
		return progress;
	}
}
