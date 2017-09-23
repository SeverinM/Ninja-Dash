using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angryEnemy : MonoBehaviour {

    abstract class basePattern
    {
        public string status = "nothing";
        protected angryEnemy enemy;
        protected bool locked = true;

        public abstract void initPattern();
        public abstract void updatePattern();
        public abstract void nextPattern();
    }

    class Waiting : basePattern
    {
        public Waiting(angryEnemy enn)
        {
            enemy = enn;
            status = "waiting";
        }

        public override void initPattern(){}
        public override void updatePattern(){}
        public override void nextPattern() {
            enemy.setPattern(new GoIn(enemy, enemy.transform.position, enemy.player.transform.position));
        }
    }

    class GoIn : basePattern
    {
        float time;
        float duration = 1;
        Vector3 originPosition;
        Vector3 targetedPosition;
        Vector2 dir;
        
        public GoIn(angryEnemy enn,Vector3 origin,Vector3 target)
        {
            enemy = enn;
            originPosition = origin;
            targetedPosition = target;
            status = "goIn";
        }

        public override void initPattern()
        {
            dir = new Vector2(targetedPosition.x - originPosition.x, targetedPosition.y - originPosition.y);
            enemy.transform.eulerAngles = Vector3.zero;
            enemy.transform.Rotate(0, 0, Mathf.Acos(dir.normalized.x) * Mathf.Rad2Deg * (dir.y < 0 ? -1 : 1));
            enemy.GetComponent<SpriteRenderer>().flipY = (dir.x < 0);
            locked = false;
            time = Time.timeSinceLevelLoad;
        }

        public override void updatePattern()
        {
            if (!locked)
            {
                enemy.transform.position += ((Vector3)dir * Time.deltaTime) / duration;
                if (Time.timeSinceLevelLoad - time > duration)
                    nextPattern();
            }
        }

        public override void nextPattern()
        {
           enemy.setPattern(new GoOut(enemy, targetedPosition, originPosition));
        }
    }

    class GoOut : basePattern
    {
        float time;
        float duration = 3;
        Vector3 originPosition;
        Vector3 targetedPosition;
        Vector2 dir;

        public GoOut(angryEnemy enn, Vector3 origin, Vector3 target)
        {
            enemy = enn;
            originPosition = origin;
            targetedPosition = target;
            status = "goOut";
        }

        public override void initPattern()
        {
            dir = new Vector2(targetedPosition.x - originPosition.x, targetedPosition.y - originPosition.y);
            enemy.transform.eulerAngles = Vector3.zero;
            enemy.transform.Rotate(0, 0, Mathf.Acos(dir.normalized.x) * Mathf.Rad2Deg * (dir.y < 0 ? -1 : 1));
            enemy.GetComponent<SpriteRenderer>().flipY = (dir.x < 0);
            locked = false;
            time = Time.timeSinceLevelLoad;
        }

        public override void updatePattern()
        {
            if (!locked)
            {
                enemy.transform.position += ((Vector3)dir * Time.deltaTime) / duration;
                if (Time.timeSinceLevelLoad - time > duration)
                    nextPattern();
            }
        }

        public override void nextPattern()
        {
            enemy.setPattern(new Waiting(enemy));
        }
    }


    basePattern currentPattern;
    public GameObject player;
    public float lenght;

	// Use this for initialization
	void Start () {
        setPattern(new Waiting(this));
	}
	
	// Update is called once per frame
	void Update () {
        currentPattern.updatePattern();
        if ((player.transform.position - transform.position).magnitude < lenght && currentPattern.status == "waiting")
            currentPattern.nextPattern();
	}

    void setPattern(basePattern patt)
    {
        currentPattern = patt;
        currentPattern.initPattern();
    }
}
