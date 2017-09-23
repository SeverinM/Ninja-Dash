using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager, undestroyable gameObject which allow to manage the whole game
/// </summary>
public class GestionJeu : MonoBehaviour {

	int softCaplvl; //Last level reached by the player
	public int hardCaplvl; //Last level existing

	public float volume{
		get{return GameObject.Find ("Audio Source").GetComponent<AudioSource> ().volume;}
		set{GameObject.Find ("Audio Source").GetComponent<AudioSource> ().volume = value;}
	}

	public GameObject son{
		get{return GameObject.Find ("Audio Source");}
	}
		
    private GameObject instanceOptions; //Instanciated prefab
    private GameObject instancePause; //Instanciated prefab
    private GameObject quitConfirm;
    public GameObject confirmQuit;
    public GameObject options;
    public GameObject pause;
    public GameObject dashBar;
    public GameObject fadeIn; //GameObject which make a fade in
    public GameObject fadeOut; //GameObject which make a fade out
    public GameObject kunaiUI;

    private static GameObject instance;
    public static string actualZone = ""; //Actually unused
    public static int actualLevel = 0; //Allow to spot the actual level and load another level by charging the Scene called "Niveau X"
    public GameObject endWindow;

	//Every Music
	public AudioClip lvl1to10; //Music played from level 1 to 10 (included)
	public AudioClip lvl10to20; //Music played from level 10 to 20

	// Use this for initialization
	void Start () {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            DAO.setZone(1);
            instance = this.gameObject;
        }

		softCaplvl = DAO.getSoftCap ();
		if (softCaplvl > hardCaplvl)
			softCaplvl = hardCaplvl;
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	}


    /// <summary>
    /// This function is called when the player win , display a end window 
    /// </summary>
	public void Win(){
        foreach (Kunai kun in GameObject.FindObjectsOfType<Kunai>())
        {
            if (kun.isTaken())
            {
                kun.Prise();
                DAO.setPieces(DAO.getPieces() + 1);
            }
        }

        softCaplvl += 1;
		DAO.setSoftCap (DAO.getSoftCap () + 1);
        chrono.arret(true);
        Time.timeScale = 1;
		((GameObject)Instantiate (endWindow, GameObject.Find ("Canvas").transform)).transform.SetSiblingIndex (0);
		if (actualLevel >= hardCaplvl)
			GameObject.Find ("Next").GetComponent<UnityEngine.UI.Button> ().interactable = false;
    }

    /// <summary>
    /// This function is called when player die , restart the level by launching a fade out animation
    /// </summary>
	public void Lose(){
        GameObject gob = Instantiate(fadeIn, GameObject.Find("Canvas").transform) as GameObject;
	}

    /// <summary>
    /// Go back to level select
    /// </summary>
    public void Menu()
    {
        int nb = DAO.getZone();

        switch (nb)
        {
            case 1:
                SceneManager.LoadScene("LevelSelect");
                break;

            case 2:
                SceneManager.LoadScene("select2");
                break;

            case 3:
                SceneManager.LoadScene("select3");
                break;

            case 4:
                SceneManager.LoadScene("select4");
                break;

            case 5:
                SceneManager.LoadScene("select2");
                break;

            default:
                break;
        }

		fadeSound (lvl1to10);
    }

    /// <summary>
    /// Load the scene : "Niveau X" and update the music (if needed)
    /// </summary>
    /// <param name="lvl"></param>
    public void LoadLvl(int lvl)
    {		
		if (lvl >= 1 && lvl <= 10)
			fadeSound (lvl1to10);
		if (lvl > 10)
			fadeSound (lvl10to20);
			
       SceneManager.LoadScene("Niveau " + lvl + actualZone);
	   Physics2D.gravity = new Vector2 (0, -29); //Reset the gravity
       actualLevel = lvl;
    }

    /// <summary>
    /// Go to the next level (can currently crash the game)
    /// </summary>
    public void nextLevel()
    {
        actualLevel += 1;
        LoadLvl(actualLevel);
    }

    /// <summary>
    /// Reload the current level (scene)
    /// </summary>
    public void Retry()
    {
        LoadLvl(actualLevel);
    }

    /// <summary>
    /// Currently unused
    /// </summary>
    /// <param name="zone"></param>
    public void setZone(string zone)
    {
        actualZone = zone;
    } 

    /// <summary>
    /// Called when the player hit the pause button
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0;
        GameObject.Find("Player").GetComponent<playerScript>().setOver(true);
        instancePause = Instantiate(pause) as GameObject;
        instancePause.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    /// <summary>
    /// Called when the player hit the resume button
    /// </summary>
    public void UnPause()
    {
        GameObject.Find("Player").GetComponent<playerScript>().setOver(false);
        GameObject.Find("Player").GetComponent<playerScript>().cancelDash();
        Destroy(instancePause);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Display a message to confirm quit...
    /// </summary>
    public void pauseToConfirm()
    {
        Destroy(instancePause);
        quitConfirm = Instantiate(confirmQuit) as GameObject;
        quitConfirm.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    /// <summary>
    /// ... and display again the pause menu the player selected "no"
    /// </summary>
    public void confirmToPause()
    {
        Destroy(quitConfirm);
        Pause();
    }

    /// <summary>
    /// Display the option window
    /// </summary>
	public void Options(){
		instanceOptions = Instantiate (options, GameObject.Find ("Canvas").transform, false) as GameObject;
        GameObject.Find("VolumePrincipale").GetComponent<UnityEngine.UI.Slider>().value = volume;
	}

    /// <summary>
    /// The player left the options window
    /// </summary>
	public void removeOptions(){
		Destroy (instanceOptions);
	}

    /// <summary>
    /// Allow to make transition between the current music and a new one, nothing happen is both are the same
    /// </summary>
    /// <param name="music">The music to change</param>
	public void fadeSound(AudioClip music){
		son.GetComponent<AudioManager> ().fadeMusic (music);
	}

    /// <summary>
    /// This flag is used to set the scene to display after a fade in
    /// </summary>
    /// <param name="value">The new flag</param>
	public void setFlag(string value){
		Restart.flag = value;
	}

    /// <summary>
    /// Launch fade
    /// </summary>
	public void launchFade(){
		try{
			GameObject.Find("FadeIn").GetComponent<Animator>().SetTrigger("End");
		}
		catch{
		}
	}

	public int getSoft(){
		return softCaplvl;
	}

    public void nextZone()
    {
        DAO.setZone(DAO.getZone() + 1);
        setFlag("menu");
        launchFade();
    }

    public void previousZone()
    {
        DAO.setZone(DAO.getZone() - 1);
        launchFade();
        setFlag("menu");
    }
}
