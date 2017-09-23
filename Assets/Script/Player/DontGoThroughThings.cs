using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontGoThroughThings : MonoBehaviour {
    Vector2 raycastFrame;
    float lenght;
    Vector2 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        raycastFrame = new Vector2(transform.position.x - previousPosition.x, transform.position.y - previousPosition.y).normalized;
        lenght = raycastFrame.magnitude;
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(previousPosition, raycastFrame, lenght)){
			if (hit.collider.isTrigger && (hit.collider.tag == "Wall" || hit.collider.tag == "Key" || hit.collider.tag == "Finish" || hit.collider.tag == "Ball"))
            {
                try
                {
                    hit.collider.gameObject.SendMessage("OnTriggerEnter2D", this.gameObject.GetComponent<Collider2D>());
                    hit.collider.gameObject.SendMessage("OnCollisionEnter2D", this.gameObject.GetComponent<Collision2D>());
                }
                catch {
                }
            }
        }

        previousPosition = transform.position;
    }

}
