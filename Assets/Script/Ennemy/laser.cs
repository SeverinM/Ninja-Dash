using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour {

	private float nextTime = 0; //Time we pass to the next state 
	public float nothingToBegin; //Time between nothing and begin
	public float beginToLaser; //Time between begin and laser
	public float laserToNothing; //Time between laser and nothing
	public string state = "nothing";
	private Vector3 vectorsecondPoint; //Vector to get the second point of the line
	LineRenderer line;
	private float lenght; //lenght of raycast
	public float lenghtLaser; //lenght of laser
    private float hitDistance;
    public bool ignoreWall = false; 

	// Use this for initialization
	void Start () {
		if (state != "nothing" && state != "laser" && state != "begin")
			state = "nothing";
		line = GetComponent<LineRenderer> ();
		line.enabled = true;
        hitDistance = 0;
	}
	
	// Update is called once per frame
	void Update () {
        vectorsecondPoint = new Vector3(transform.right.x, transform.right.y, 0).normalized * (hitDistance >= 0 && !(ignoreWall) ? hitDistance : lenghtLaser);
        hitDistance = -1;
		lenght = (line.GetPosition (0) - line.GetPosition (1)).magnitude;
		if (state == "laser" || state == "begin") {
			foreach (RaycastHit2D hit in Physics2D.RaycastAll(line.GetPosition(0),line.GetPosition (1) - line.GetPosition (0),lenght + 1)) {
                if (hit.collider.gameObject.tag == "Player" && state == "laser")
                {
                    hit.collider.gameObject.GetComponent<playerScript>().Kill();
                }
                if (hit.collider.gameObject.tag == "Wall" && !(ignoreWall))
                {
                    hitDistance = hit.distance;
                    break;
                }
			}
		}
		if (Time.timeSinceLevelLoad > nextTime && (nothingToBegin > 0 || beginToLaser > 0))
			updateState ();

        try
        {
            line.SetPosition(0, transform.parent.position);
            line.SetPosition(1, transform.parent.position + vectorsecondPoint);
        }
        catch
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + vectorsecondPoint);
        }
    }

	void updateState(){
		switch (state) {
		case "nothing":
			nextTime = Time.timeSinceLevelLoad + beginToLaser;
			state = "begin";
			line.startColor += new Color (0, 0, 0, 0.5f);
			line.endColor += new Color (0, 0, 0, 0.5f);
			break;
		case "begin":
			nextTime = Time.timeSinceLevelLoad + laserToNothing;
			state = "laser";
			line.startColor += new Color (0, 0, 0, 0.5f);
			line.endColor += new Color (0, 0, 0, 0.5f);
			break;
		case "laser":
			nextTime = Time.timeSinceLevelLoad + nothingToBegin;
			state = "nothing";
			line.startColor -= new Color (0, 0, 0, 1);
			line.endColor -= new Color (0, 0, 0, 1);
			break;
		}
	}
}
