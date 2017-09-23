using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chrono : MonoBehaviour {

    public static float time = 0; //Time
    public static string strTime = "error" ; //Another representation of time
    private int minutes;
    private int secondes;
    private float centiemes;
    private string output;
    private static bool stop;

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (!(stop))
        {
			time += Time.deltaTime * Time.timeScale;
            strTime = ConvertToStr(time);
            GetComponent<Text>().text = strTime.ToString();
        }
	}

    string ConvertToStr(float time)
    {
        output = "";
        minutes = (int)(time / 60);
        secondes = (int)(time % 60);
        centiemes = (int)((time - Mathf.Round(time)) * 100);
        if (centiemes < 0)
            centiemes += 100;

        output += minutes.ToString() + " : ";
        output += (secondes < 10 ? "0" : "") + secondes.ToString() + " : " ;
        output += (centiemes < 10 ? "0" : "") + centiemes.ToString();

        return output;
    }

    public static void arret(bool value)
    {
        stop = value;
    }
}
