using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    public static GameObject instance;

    Animator anim;
    SpriteRenderer sprt;
    Rigidbody2D physic;
    BoxCollider2D box;

	private List<string> gravityFieldList;
	private List<GameObject> trailList;

    public GestionJeu gamemanager;
	public GameObject traj;
	public GameObject nuage; //Animation des nuages 
    public GameObject bardash; //UI dash Bar
    public GameObject trailObject;
    public GameObject fadein;
    private GameObject instantiatedTrail;

	private float timeSlowing; //Since when the player slowed ? 
    private GameObject katana;
    private int leftCount = 0;
    private int rightCount = 0;
    public bool canWallRide = true;

	private Vector3 camPos;
	public bool followX = true; //May the camera should follow the player on the X axis ? 
	public bool followY = true; //May the camera should follow the player on the Y axis ?

	private float beginRotating;
    private float rotateSpeed = 0.5f; //How many time does it take to rotate to zero ?
    private Vector2 vectorRotate; //The vector followed to rotate to 0

	private Vector3 defaultSize;
	private bool lastSlowing;

	public float dashPower = 130000;
    private float extraPower = 0;
    private float timeExtra = 1.5f; //Time required to get the max power

    public float dur = 0.23f;
	private bool slowing = false; //Is the player slowing time ?
    public bool rotating = false;
    private float time = 0; //Time when the dash is over
    public bool dashing = false; //You cannot dash again if you are already dashing
    public float dashMax = 5; //Number of maximum dash player can do on air
    public float reloadTime; //How many time does it take to reload a single dash
    public float endDashFactor = 10000f; //The higher the number is, the slower you will get a the end of a dash
	public float cameraOffsetMax;
	private float cameraOffset;
	private Vector3 camDir;
	private bool limitReach = false;
	private float limitWallrideVelocity = 100;
    private int blockTouch = 0;
    private bool rideLeft = false;
    private bool rideRight = false;

    public float dashUsed = 0; //Number of dash currently used, is used to update the dash bar. 0 means you got all your dash available
    public bool focus = true;
    public bool canForce = true;

    private bool over = false; //block every command
	[HideInInspector]
    public bool ontheGround = false; //set to false when the player is touching nothing
	private bool touch = false; //is the player wallriding ?
	public bool invincible = false;
    bool bottom = false;
    bool top = false;

    private float gravityValue;
    enum Gravity { up , down , left , right};
    Gravity actualGravity;
    private bool reversedX
    {
        get { return transform.localScale.x < 0; }
    }

    private bool reversedY
    {
        get { return transform.localScale.y < 0; }
    }

	public bool FollowX{
		get {return followX;}
	}

	public bool FollowY{
		get {return followY;}
	}

    // Use this for initialization
    void Start()
    {
        if (fadein == null)
            fadein = GameObject.Find("FadeIn");

        if (bardash == null)
            bardash = GameObject.Find("BarreDashUI");

        if (gamemanager == null)
        {
            try
            {
                gamemanager = GameObject.Find("GameManager").GetComponent<GestionJeu>();
            }
            catch
            {
                gamemanager = null;
            }
        }

        trailList = new List<GameObject> ();
		traj.SetActive (false);
		gravityFieldList = new List<string> ();
        gravityValue = Physics2D.gravity.y;
        actualGravity = Gravity.down;
        chrono.arret(false);
		defaultSize = transform.localScale;
        
        if (instance == null)
            instance = this.gameObject;
        else
            Kill();

        foreach (Transform child in transform)
        {
            if (child.tag == "Weapon")
                katana = child.gameObject;
        }

        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprt = GetComponent<SpriteRenderer>();
        physic = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
		if (!(over)) {
            rideLeft = false;
            rideRight = false;
            touch = (blockTouch != 0);
			lastSlowing = slowing;
			Reload (); //Real reload
			eventManager (); //Get all event and act
			updateCamera ();
			updateSlow (); //
			updateTrail ();
			updateUI (dashMax - dashUsed);

			katana.SetActive (dashing); //The katana is enabled only during dashing state
			anim.SetBool ("Air", !(ontheGround));
			anim.SetBool ("Dashing", dashing);

			if (rotating)
				RotateToZero ();
		}
    }

    void Dash(Vector2 direction, float duration, float strength, bool free)
    {
		physic.velocity = Vector2.zero;
		anim.Play ("Dash",-1,0);
        direction = direction.normalized;
		direction = new Vector3 (direction.x, direction.y, 0);
        physic.AddForce(direction * strength);

        dashing = true;
        time = Time.timeSinceLevelLoad + duration; //Set the time when dash will end 

        if (!free)
            dashUsed += 1;

		transform.localScale = defaultSize;
        if (actualGravity == Gravity.down)
        {
			if (direction.x < 0)
				transform.localScale = new Vector3 (defaultSize.x, -defaultSize.y, defaultSize.z);
        }

		if (actualGravity == Gravity.up)
		{
			if (direction.x > 0)
				transform.localScale = new Vector3 (defaultSize.x, -defaultSize.y, defaultSize.z);
		}

        if (actualGravity == Gravity.right)
        {
            if (direction.y < 0)
				transform.localScale = new Vector3(defaultSize.x, -defaultSize.y, defaultSize.z);
        }

		if (actualGravity == Gravity.left) {
			if (direction.y > 0)
				transform.localScale = new Vector3(defaultSize.x, -defaultSize.y, defaultSize.z);
		}

		transform.right = direction;
    }

    void updateUI(float dashNb)
    {
        if (Time.timeSinceLevelLoad - timeSlowing > 0.25f) { 
			traj.SetActive (slowing);
		}

        if (bardash != null)
        {
            Image image = bardash.GetComponent<Image>();
            image.fillAmount = dashNb / dashMax;
            if (dashMax - dashUsed < 1.4f)
            {
                if (dashMax - dashUsed < 1)
                    image.color = Color.red;
                else
                    image.color = Color.yellow;
            }
            else
                image.color = Color.white;

            if (touch && canWallRide)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            else
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
    }


	public float getCharge(){
		return (dashUsed / dashMax) * 100;
	}

    public void SetCharge(float ammount)
    { //Allow to instantly change the charge , 0 = empty 100 = full
		if (ammount < 0)
			ammount = 0;

		if (ammount > 100)
			ammount = 100;

        if (ammount >= 0 && ammount <= 100)
        {
            dashUsed -= dashMax * (ammount / 100);
        }

        if (dashUsed > dashMax) dashUsed = dashMax;
        if (dashUsed < 0) dashUsed = 0;

    }

    public void RotateToZero()
    {
        if (actualGravity == Gravity.down)
        {   
			transform.right = new Vector3 (transform.right.x + (reversedX != reversedY ? -0.01f : 0.01f), transform.right.y * 0.95f, 0);
			if (Mathf.Abs(transform.right.y) < 0.01f)
				rotating = false;
			if ((!(reversedX || reversedY) && transform.right.x < 0) || (reversedX || reversedY) && transform.right.x > 0) {
				rotating = true;
				transform.Rotate (Vector3.forward * -5);
			}
        }

		if (actualGravity == Gravity.up)
		{   
			transform.right = new Vector3 (transform.right.x + (reversedX || reversedY ? 0.01f : -0.01f), transform.right.y * 0.95f, 0);
			if (Mathf.Abs (transform.right.y) < 0.01f)
				rotating = false;
		}

        if (actualGravity == Gravity.right) {
			transform.right = new Vector3(transform.right.x * 0.95f, transform.right.y + (reversedX || reversedY ? -0.01f : 0.01f) , 0);
			if (Mathf.Abs (transform.right.x) < 0.01f)
				rotating = false;
        }

		if (actualGravity == Gravity.left) {
			transform.right = new Vector3(transform.right.x * 0.95f, transform.right.y + (reversedX || reversedY ? 0.01f : -0.01f) , 0);
			if (Mathf.Abs (transform.right.x) < 0.01f)
				rotating = false;
		}
    }

    public void InstantRotate()
    {
        switch (actualGravity)
        {
            case Gravity.down:
                transform.right = new Vector3(1 * (reversedX != reversedY ? -1 : 1), 0, 0);
                break;

            case Gravity.up:
                transform.right = new Vector3(-1 * (reversedX != reversedY ? -1 : 1), 0, 0);
                break;

            case Gravity.right:
                transform.right = new Vector3(0, -1 * (reversedX != reversedY ? 1 : -1), 0);
                break;

            case Gravity.left:
                transform.right = new Vector3(0, 1 * (reversedX != reversedY ? 1 : -1), 0);
                break;
        }
    }

	public void Reload(){
		if (dashUsed > 0)
		{
			if (reloadTime != 0) {
				if (!(slowing))
				{
					dashUsed -= (Time.deltaTime / reloadTime);
					if (ontheGround)
						dashUsed -= (Time.deltaTime / reloadTime);
				}
				else
					dashUsed += (Time.deltaTime / reloadTime) / 3;
			}
		}
		else
			dashUsed = 0;
	}

	void eventManager(){
		if (Input.GetMouseButtonDown (0) && (dashUsed < dashMax - 1 || touch) && canForce) { //Slow time as long as you hold the click , but consume dash bar
			slowing = true;
			timeSlowing = Time.timeSinceLevelLoad;
		}

		if (Input.GetMouseButtonUp(0) && (slowing || !canForce))
		{ //dash started
			slowing = false;
			if (dashMax > (dashUsed + 1) || touch) {
				rotating = false;
				Dash (Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position, dur, (dashPower + extraPower) / 1.5f, touch && canWallRide);
				limitReach = false;
			}
            extraPower = 0;
			trailList.Add (Instantiate (trailObject) as GameObject);
        }

		if (dashUsed >= dashMax) {
			dashUsed = dashMax;
			slowing = false;
		}
			
		if (dashing && Time.timeSinceLevelLoad > time)
		{ //when dash ended
			physic.velocity = (physic.velocity / endDashFactor);
			physic.AddForce (new Vector2 (0, -100));
			dashing = false;
			rotating = true;
			beginRotating = Time.timeSinceLevelLoad;
		}

	}

	void updateCamera (){
        if (focus)
        {
            camDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position)).normalized * cameraOffset;
            camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(followX ? transform.position.x : camPos.x, followY ? transform.position.y : camPos.y, -10) + (followX && followY ? camDir : Vector3.zero); //The camera always follow the player
        }
    }

    public void Kill() //Called when the player get hit
    {
        if (!over)
        {
            Restart.flag = "restart";
            fadein.GetComponent<Animator>().SetTrigger("End");
            Destroy(this.gameObject);
            Instantiate(nuage, transform.position, new Quaternion());
        }
    }

    public void setOver(bool value)
    {
        over = value;
        if (value)
            slowing = false;
    }

    public void cancelDash()
    {
        dashing = true;
        time = Time.timeSinceLevelLoad;
    }

    public void updateGravity (string grav, float modifier)
    {
        rotating = true;
        switch (grav)
        {
            case "up":
                actualGravity = Gravity.up;
                Physics2D.gravity = new Vector2(0,-gravityValue) * modifier;
                break;

            case "down":
                actualGravity = Gravity.down;
                Physics2D.gravity = new Vector2(0, gravityValue) * modifier;
                break;

            case "left":
                actualGravity = Gravity.left;
                Physics2D.gravity = new Vector2(gravityValue, 0) * modifier;
                break;

            case "right":
                actualGravity = Gravity.right;
                Physics2D.gravity = new Vector2(-gravityValue, 0) * modifier;
                break;

            default:
                Debug.Log("gravité inconnu : " + grav);
                break;
        }
    }

    public void groundTouch()
    {
        switch (actualGravity)
        {
			case Gravity.up:
                transform.localScale = new Vector3(defaultSize.x, -defaultSize.y, 0);	
				transform.right = new Vector3 (-1 * (reversedX != reversedY ? -1 : 1), 0, 0);
                break;

			case Gravity.left:
				transform.right = new Vector3(0,-1 * (reversedX != reversedY ? -1 : 1) ,0);
				break;

			case Gravity.down:
				transform.localScale = defaultSize;
				transform.right = new Vector3 (1 * (reversedX != reversedY ? -1 : 1), 0, 0);
                break;

            case Gravity.right:
				transform.right = new Vector3(0,1 * (reversedX != reversedY ? -1 : 1), 0);
                break;
        }
    }

	public void enterGravityField(string gravitystr,float scale){
		gravityFieldList.Add (gravitystr);
		updateGravity (gravitystr,scale);
	}

	public void exitGravityField(string gravitystr){
		gravityFieldList.Remove (gravitystr);
		if (gravityFieldList.Count == 0)
			updateGravity ("down",1);
		else
			updateGravity (gravityFieldList [gravityFieldList.Count - 1],1);
	}

	void updateSlow(){
		if (!(slowing)) Time.timeScale = 1f; //Set time to normal when not slowing anymore

		if (!(slowing)) { //center camera little by little once slow is done
			cameraOffset /= 2;
		}

		if (slowing && !(limitReach)) { //slow increase dash power until it reach half the power
			extraPower += ((dashPower * Time.deltaTime)/ timeExtra);
			if (Time.timeSinceLevelLoad - timeSlowing > 0.25f)
				cameraOffset += (cameraOffsetMax * Time.deltaTime) / timeExtra;
			if (traj.activeSelf)
				traj.GetComponent<showTrajectory> ().lineGrowth (0.007f, 0.00000012f);
		}

		if (extraPower >= dashPower / 2 && traj.activeSelf && !(limitReach)) {
			limitReach = true;
			traj.GetComponent<showTrajectory> ().setColor (Color.white);
		}

		if (slowing && Time.timeScale > 0.3f) {
			Time.timeScale -= 0.03f;
		}
	}

	void updateTrail(){
        List<GameObject> deleteList = new List<GameObject>();
		foreach (GameObject gob in trailList) {
			gob.GetComponent<TrailRenderer> ().startColor += new Color (0, 0, 0,(float)(-1f * Time.deltaTime));
			gob.GetComponent<TrailRenderer> ().endColor += new Color (0, 0, 0,(float)(-1f * Time.deltaTime) );
			if (gob.GetComponent<TrailRenderer> ().startColor.a == 0) {
                deleteList.Add(gob);
            }
		}

        foreach(GameObject gob in deleteList)
        {
            trailList.Remove(gob);
            Destroy(gob);
        }

		if (dashing && trailList.Count > 0)
		{
			trailList [trailList.Count - 1].transform.position = transform.position;
		}

	}

	void OnDestroy(){
		instance = null;
		Time.timeScale = 1;
	}

	void OnBecameInvisible(){
        if (!(followX && FollowY))
		    Kill ();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
        if (coll.name.Contains("Wall"))
            blockTouch += 1;

        if ((coll.tag == "Ennemy" || coll.tag == "Ball") && !(dashing))
			Kill();
		if (coll.tag == "Finish")
		{
			slowing = false;
			dashing = false;
			over = true;
			physic.velocity *= 0.01f;
            if (gamemanager != null)
			    gamemanager.Win();
		}
	}

    void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Undashable" || coll.gameObject.tag == "Ball")
		{
			Kill();
		}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Contains("Wall"))
            blockTouch -= 1;
    }

	void OnTriggerStay2D(Collider2D other){

        if (actualGravity == Gravity.down) {
			if (other.name == "WallLeft") {
                rideLeft = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(-600, 0));
                    if (physic.velocity.y < -limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x, physic.velocity.y / 1.1f);
                }
			}
			if (other.name == "WallRight") {
                rideRight = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(600, 0));
                    if (physic.velocity.y < -limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x, physic.velocity.y / 1.1f);
                }
			}
		}

		if (actualGravity == Gravity.up) {
            if (other.name == "WallLeft") {
                rideLeft = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(600, 0));
                    if (physic.velocity.y > -limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x, physic.velocity.y / 1.1f);
                }
			}
			if (other.name == "WallRight") {
                rideRight = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(-600, 0));
                    if (physic.velocity.y > -limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x, physic.velocity.y / 1.1f);
                }
			}
		}

		if (actualGravity == Gravity.right) {
            if (other.name == "WallLeft") {
                rideLeft = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(0, -600));
                    if (physic.velocity.x > limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x / 1.1f, physic.velocity.y);
                }
			}
			if (other.name == "WallRight") {
                rideRight = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(0, 600));
                    if (physic.velocity.x > limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x / 1.1f, physic.velocity.y);
                }
			}
		}

		if (actualGravity == Gravity.left) {
            if (other.name == "WallLeft") {
                rideLeft = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(0, 600));
                    if (physic.velocity.x < -limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x / 1.1f, physic.velocity.y);
                }
			}
			if (other.name == "WallRight" && !(touch )) {
                rideRight = true;
                if (!touch)
                {
                    physic.AddForce(new Vector2(0, -600));
                    if (physic.velocity.x < -limitWallrideVelocity)
                        physic.velocity = new Vector2(physic.velocity.x / 1.1f, physic.velocity.y);
                }
			}
		}

        if (rideLeft && rideRight)
            Kill();
    }

    public void setFocus(bool value)
    {
        focus = value;
    }

    public bool isOver()
    {
        return over;
    }

    public void touchdown(bool value)
    {
        bottom = value;
        if (bottom && top)
            Kill();
    }

    public void touchUp(bool value)
    {
        top = value;
        if (bottom && top)
            Kill();
    }
}
