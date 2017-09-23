using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveZone : MonoBehaviour {

    public float duration;
    public float distance = 1946;
    float time = 0;
    bool reversed = false;
    int actualZone = 1;

	// Use this for initialization
	void Start () {
        setMove(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (time - Time.timeSinceLevelLoad > 0) {  
            GetComponent<RectTransform>().position += new Vector3(distance * (Time.timeSinceLevelLoad / duration) * (reversed ? 1 : -1), 0, 0);
        }
	}

    void setMove(bool reverse)
    {
        time = Time.timeSinceLevelLoad + duration;
        reversed = reverse;
        if (reverse)
            actualZone -= 1;
        else
            actualZone += 1;
    }
}
