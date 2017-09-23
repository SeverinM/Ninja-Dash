using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour {

    public enum edgeSide
    {
        up,
        down,
        left,
        right
    }

    public edgeSide actualSide;
    public float scale; //what's the zoom when you leave the zone ? when you leave it ?
    public Camera cam;
    private float distance;
    private Vector3 center;
    private Vector3 size;
    private float lastDistance;
    public static GameObject currentZoom;
    private float hardCapTop;
    private float hardCapLow;

    void Start()
    {
        center = GetComponent<BoxCollider2D>().bounds.center;
        size = GetComponent<BoxCollider2D>().bounds.size;
        lastDistance = 0;
        hardCapTop = cam.orthographicSize + scale;
        hardCapLow = cam.orthographicSize - scale;
    }

	public void changeCameraSize(Vector3 playerPosition)
    {
        distance = getDistance(playerPosition);

        //+ = dezoom - = zoom
        if (distance > lastDistance && distance > hardCapLow)
        { //s'eloigne
            Debug.Log("eloigne");
            cam.orthographicSize += (Mathf.Abs(distance - lastDistance) / (actualSide == edgeSide.left || actualSide == edgeSide.right ? size.x : size.y)) * scale;
        }

        if (distance < lastDistance && distance < hardCapTop)
        { //se rapproche
            Debug.Log("approche");
            cam.orthographicSize -= (Mathf.Abs(distance - lastDistance) / (actualSide == edgeSide.left || actualSide == edgeSide.right ? size.x : size.y)) * scale;
        }

        lastDistance = distance;
    }

    public float getDistance(Vector3 playerPosition)
    {
        switch (actualSide)
        {
            case edgeSide.down:
                return Mathf.Abs(playerPosition.y - (center.y - (size.y / 2)));
            case edgeSide.up:
                return Mathf.Abs(playerPosition.y - (center.y + (size.y / 2)));
            case edgeSide.left:
                return Mathf.Abs(playerPosition.x - (center.x - (size.x / 2)));
            case edgeSide.right:
                return Mathf.Abs(playerPosition.x - (center.x + (size.x / 2)));
            default:
                return 0;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player" && currentZoom == null)
            currentZoom = this.gameObject;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player" && currentZoom == this.gameObject)
            currentZoom = null;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (currentZoom == this.gameObject && coll.tag == "Player")
        {
            changeCameraSize(coll.gameObject.transform.position);
        }
    }

}
