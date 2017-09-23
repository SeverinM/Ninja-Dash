using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showTrajectory : MonoBehaviour {

	public GameObject target;
	private Rigidbody2D rb;
	private Vector3 position;
	private Vector2 velocity;
	private float extraPower = 0;
	LineRenderer line;
	private int numStep;
	private float timeDelta;

	// Use this for initialization
	void Start () {
		rb = target.GetComponent<Rigidbody2D>();
		line = GetComponent<LineRenderer> ();
		numStep = 20;
	}

	// Update is called once per frame
	void Update () {
		UpdateTrajectory(rb.transform.position,(Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position).normalized * 230000 ,Physics2D.gravity);
	}

	void UpdateTrajectory (Vector3 initialPosition, Vector2 initialVelocity, Vector2 gravity)
	{
		line.startColor += new Color (0, -0.005f, -0.005f, 0.01f);
		line.endColor += new Color (0, -0.005f, -0.005f, 0.01f);
		velocity = initialVelocity;
		position = initialPosition;
		timeDelta = (3 / initialVelocity.magnitude) + extraPower;

		for (int i = 0; i < numStep; ++i) {
			line.SetPosition (i, position);
			position += new Vector3(velocity.x,velocity.y,0) * timeDelta + 0.5f * new Vector3(gravity.x,gravity.y,0) * timeDelta * timeDelta;
			velocity += gravity * timeDelta;
		}
		line.endColor = new Color (1, 0, 0, 0f);
	}

	void OnDisable(){
		line.startColor -= new Color (0, 1, 1);
		line.startColor -= new Color (0, 1, 1);
	}

	void OnEnable(){
		line = GetComponent<LineRenderer> ();
		extraPower = 0;
		line.startWidth = 1;
		line.endWidth = 1;
		line = GetComponent<LineRenderer> ();
		line.startColor += new Color (0, 0, 0, -1);
		line.endColor += new Color (0, 0, 0, -1);
	}

	public void lineGrowth(float width, float lenght){ //0.005f et 0.00000015f
		line.startWidth += width;
		line.endWidth += width;
		extraPower += lenght;
	}

	public void setColor(Color col){
		line.startColor = col;
		line.endColor = col;
	}
}
