using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : BaseEnnemie {

    public abstract class BossPattern
    {
        protected float stateDuration = 1; //Duration of state
        protected float time; //When this state started
        protected bool reversedX; //set to true when the player is behind the boss
        protected BossBehavior bossBehave;
        public abstract void nextPattern();
        public bool locked = true; //While this bool is set to true, nothing happen on update

        public virtual void updatePattern()
        {
            reversedX = !((bossBehave.transform.position.x - bossBehave.player.transform.position.x) < 0);
            bossBehave.GetComponent<SpriteRenderer>().flipX = reversedX;
        }

        public virtual void initPattern()
        {
            time = Time.timeSinceLevelLoad;
        }

        public virtual void setDuration(float value)
        {
            stateDuration = Math.Abs(value);
        }

        public float getDuration()
        {
            return stateDuration;
        }
    }

    class TeleportOut : BossPattern //Begin of teleport
    {
        float fadeOutTime = 0.5f;

        public TeleportOut(BossBehavior bossbehave)
        {
            bossBehave = bossbehave;
        }

        public override void nextPattern()
        {
            bossBehave.gameObject.tag = "Untagged"; //When the teleport has ended, boss cannot be touched

            if (UnityEngine.Random.Range(0,100) < 50)
                bossBehave.setPattern(new TeleportIn(bossBehave));
            else
                bossBehave.setPattern(new InvincibleAttack(bossBehave));
        }

        public override void initPattern()
        {
            base.initPattern();
            bossBehave.tag = "Untagged";
            setDuration(0.75f);
            locked = false;
            bossBehave.GetComponent<SpriteRenderer>().color = Color.white;
        }

        public override void updatePattern()
        {
            if (!(locked))
            {
                base.updatePattern();
                bossBehave.GetComponent<SpriteRenderer>().color -= new Color(0.05f, 0.05f, 0.05f, (1 / fadeOutTime) * Time.deltaTime);
                if (Time.timeSinceLevelLoad - time > stateDuration)
                    nextPattern();
            }
        }
    }

    class TeleportIn : BossPattern //end of teleport
    {
        float fadeInTime = 0.5f;

        public TeleportIn(BossBehavior bossbehave)
        {
            bossBehave = bossbehave;
            bossBehave.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        public override void initPattern()
        {
            base.initPattern();
            bossBehave.gameObject.tag = "Ennemy";
            setDuration(0.75f);
            bossBehave.transform.position = new Vector3(UnityEngine.Random.Range(0, 100), 2.4f, 0);
            locked = false;
        }

        public override void updatePattern()
        {
            if (!(locked))
            {
                base.updatePattern();
                bossBehave.GetComponent<SpriteRenderer>().color += new Color(0.05f, 0.05f, 0.05f, (1 / fadeInTime) * Time.deltaTime);
                if (Time.timeSinceLevelLoad - time > stateDuration)
                    nextPattern();
            }
        }

        public override void nextPattern()
        {
            bossBehave.tag = "Ennemy";
            bossBehave.setPattern (new Idle (bossBehave));
        }
    }

    class Attack1 : BossPattern //Throw a fireBall at horizontal
    {
        int probability;
        private GameObject gob;

        public Attack1(BossBehavior bossbehave, int prob = 0)
        {
            bossBehave = bossbehave;
            bossBehave.GetComponent<SpriteRenderer>().color = Color.white;
            probability = prob;
        }

        public override void initPattern()
        {
            //Call parent class
            base.initPattern();
            base.updatePattern();

            Vector3 playerPos = bossBehave.player.transform.position;
            Vector2 angle = new Vector2(playerPos.x - bossBehave.transform.position.x, playerPos.y - bossBehave.transform.position.y).normalized;

            setDuration(1);
            bossBehave.GetComponent<Animator>().SetInteger("flag", 2);
            gob = Instantiate(bossBehave.attack1Proj, bossBehave.transform.position, new Quaternion());
            gob.GetComponent<Rigidbody2D>().velocity = angle * 150;
            gob.transform.Rotate(0, 0, (Mathf.Acos(angle.x) * Mathf.Rad2Deg));
            locked = false;
        }

        public override void updatePattern()
        {
            if (!(locked))
            {
                base.updatePattern();
				bossBehave.GetComponent<SpriteRenderer>().color += new Color(0.05f, 0.05f, 0.05f);
                if (Time.timeSinceLevelLoad - time > stateDuration)
                    nextPattern();
            }
        }

        public override void nextPattern()
        {
			int rand = UnityEngine.Random.Range (0, 100);
			if (rand > probability)
				bossBehave.setPattern (new Attack1 (bossBehave, probability + 10));
			else {
				if (rand > 50)
					bossBehave.setPattern (new TeleportOut (bossBehave));
				else
					bossBehave.setPattern (new Cluster(bossBehave));
			}
				
        }
    }

    class InvincibleAttack : BossPattern
    {
        private GameObject gob;
        private float timeFireball = 0.15f; //set delay between each fireball
        private float timeFired = 0; //Time since the previous fireball;

        public InvincibleAttack(BossBehavior bossbehave)
        {
            bossBehave = bossbehave;
            bossBehave.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        }

        public override void nextPattern()
        {
            bossBehave.setPattern(new TeleportIn(bossBehave));
        }

        public override void initPattern()
        {
            setDuration(3);
            base.initPattern();
            locked = false;
        }

        public override void updatePattern()
        {
            if (!(locked))
            {
                if (Time.timeSinceLevelLoad - timeFired > timeFireball)
                {
                    float playerX = bossBehave.player.transform.position.x;
                    timeFired = Time.timeSinceLevelLoad;
                    gob = Instantiate(bossBehave.attack1Proj, new Vector3(playerX + UnityEngine.Random.Range(-50, 50),200, 0), new Quaternion());
                    gob.transform.Rotate(0, 0, -90);
                    gob.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -80);
                }

                if (Time.timeSinceLevelLoad - time > stateDuration)
                    nextPattern();
            }
        }
    }

    class Cluster : BossPattern
    {
        States thisState = States.Attack2;
        private GameObject gob;
        private int ballsNb = 10;

        public Cluster(BossBehavior bossbehave)
        {
            bossBehave = bossbehave;
        }

        public override void initPattern()
        {
            base.initPattern();
            base.updatePattern();
            bossBehave.GetComponent<Animator>().SetInteger("flag", 3);
            setDuration(2f);

            for (int i = 0; i < ballsNb; i++)
            {
                float angle = (UnityEngine.Random.Range(-40, 50) + (reversedX ? 180 : 0)) * Mathf.Deg2Rad;
                gob = Instantiate(bossBehave.attack1Proj, bossBehave.transform.position, new Quaternion());
                gob.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * UnityEngine.Random.Range(40, 120);
                gob.transform.Rotate(0, 0, angle * Mathf.Rad2Deg);
            }
        }

        public override void updatePattern()
        {
            base.updatePattern();
            bossBehave.GetComponent<SpriteRenderer>().color += new Color(0.05f, 0.05f, 0.05f);
            if (Time.timeSinceLevelLoad - time > stateDuration)
                nextPattern();
        }

        public override void nextPattern()
        {
			bossBehave.setPattern (new Idle (bossBehave));
        }
    }

	class Idle : BossPattern{
		public Idle(BossBehavior bossbehave){
			bossBehave = bossbehave;
		}

		public override void initPattern ()
		{
			base.initPattern ();
			setDuration (0.5f);
			locked = false;
		}

		public override void updatePattern(){
			if (!(locked)) {
				if (Time.timeSinceLevelLoad - time > stateDuration)
					nextPattern();
			}
		}

		public override void nextPattern(){
			int rand = UnityEngine.Random.Range (0, 100);

			if (rand >= 0 && rand < 33)
				bossBehave.setPattern (new Attack1 (bossBehave));
			if (rand >= 33 && rand < 66)
				bossBehave.setPattern (new Cluster (bossBehave));
			if (rand >= 66)
				bossBehave.setPattern (new TeleportOut (bossBehave));
		}
	}

    class Intro : BossPattern
    {
        float timeIn = 1.5f;
        float timeOut = 1.5f;
        float timeStand = 1;
        Vector3 travelCam;
        string state = "in";

        public Intro(BossBehavior bossbehave)
        {
            bossBehave = bossbehave;
        }

        public override void initPattern()
        {
            base.initPattern();
            Debug.Log(GameObject.Find("Player").transform.position - Camera.main.transform.position);
            bossBehave.player.GetComponent<playerScript>().setFocus(false);
            bossBehave.player.GetComponent<playerScript>().setOver(true);
            travelCam = bossBehave.transform.position - bossBehave.player.transform.position + new Vector3(32.8f,-9.4f, 0);
            locked = false;
        }

        public override void updatePattern()
        {
            base.updatePattern();
            if (!(locked))
            {
                switch (state)
                {
                    case "in":
                        Camera.main.gameObject.transform.position += travelCam * (1 / timeIn) * Time.deltaTime;
                        break;

                    case "out":
                        Camera.main.gameObject.transform.position -= travelCam * (1 / timeOut) * Time.deltaTime;
                        break;

					default:
						break;
                }

                if (Time.timeSinceLevelLoad- time > timeIn && state == "in")
                {
                    time = Time.timeSinceLevelLoad;
                    state = "stand";
                }

                if (Time.timeSinceLevelLoad - time > timeStand && state == "stand")
                {
					travelCam = bossBehave.transform.position - bossBehave.player.transform.position;
                    time = Time.timeSinceLevelLoad;
                    state = "out";
                }

				if (Time.timeSinceLevelLoad - time > timeOut && state == "out")
                {
                    nextPattern();
                }
					
            }
        }

        public override void nextPattern()
        {
            bossBehave.player.GetComponent<playerScript>().setFocus(true);
            bossBehave.player.GetComponent<playerScript>().setOver(false);
            bossBehave.setPattern (new TeleportOut(bossBehave));
        }
    }

    class Outro : BossPattern
    {
        float timeIn = 1.5f;
        float timeOut = 1.5f;
        float timeStand = 1;
        Vector3 travelCam;
        string state;

        public Outro(BossBehavior bossbehave)
        {
            bossBehave = bossbehave;
            state = "in";
        }

        public override void initPattern()
        {
            base.initPattern();
            bossBehave.player.GetComponent<playerScript>().setFocus(false);
            travelCam = bossBehave.transform.position - bossBehave.player.transform.position + new Vector3(32.8f, -9.4f, 0); ;
            bossBehave.GetComponent<Animator>().SetInteger("flag", 1);
            locked = false;
        }

        public override void updatePattern()
        {
            base.updatePattern();
            if (!locked)
            {
                switch (state)
                {
                    case "in":
                        Camera.main.gameObject.transform.position += travelCam * (1 / timeIn) * Time.deltaTime;
                        break;

                    case "out":
                        Camera.main.gameObject.transform.position -= travelCam * (1 / timeOut) * Time.deltaTime;
                        break;

                    default:
                        break;
                }

                if (Time.timeSinceLevelLoad - time > timeIn && state == "in")
                {
                    time = Time.timeSinceLevelLoad;
                    state = "stand";
                }

                if (Time.timeSinceLevelLoad - time > timeStand && state == "stand")
                {
                    travelCam = bossBehave.transform.position - bossBehave.player.transform.position;
                    time = Time.timeSinceLevelLoad;
                    state = "out";
                }

                if (Time.timeSinceLevelLoad - time > timeOut && state == "out")
                {
                    Debug.Log(Time.timeSinceLevelLoad - time);
                    nextPattern();
                }
            }
        }

        public override void nextPattern()
        {
            bossBehave.player.GetComponent<playerScript>().setFocus(true);
            GameObject.Find("GameManager").GetComponent<GestionJeu>().Win();
            bossBehave.GetComponent<SpriteRenderer>().sprite = bossBehave.theSprite;
            Destroy(bossBehave);
        }
    }

    
    BossPattern currentPattern;
    public enum States { Intro, Outro, TeleportOut, TeleportIn , Attack1 , Attack2, Attack3};
    public GameObject attack1Proj;
    public GameObject player;
    public Sprite theSprite;

    //Animator flag : 0 = idle, 1 = die , 2 = attack , 3 = throwing , 4 = jump
	// Use this for initialization
	void Start () {
        setPattern(new Intro(this));
	}
	
	// Update is called once per frame
	void Update () {
        currentPattern.updatePattern();
        GetComponent<SpriteRenderer>().color += new Color(0.05f, 0.05f, 0.05f, 0) * Time.deltaTime;
    }

    public void setPattern(BossPattern patt)
    {
        currentPattern = patt;
        currentPattern.initPattern();
    }

    public override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Weapon")
        {
            life -= 1;
            if (life <= 0)
                Kill(this.GetComponent<Collider2D>());
            else
                GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public override void Kill(Collider2D coll)
    {
        setPattern(new Outro(this));
    }
}
