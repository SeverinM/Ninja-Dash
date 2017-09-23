using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	private bool fading = false;
	public float fadeFactor;
	private AudioSource src;
	private AudioClip nextMusic;

	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource> ();
		if (fadeFactor == 0)
			fadeFactor = 0.001f;
		if (fadeFactor < 0)
			fadeFactor = -fadeFactor;
		 
	}
	
	// Update is called once per frame
	void Update () {
		if (fading) {
			src.volume -= fadeFactor;
			if (src.volume == 0) {
				fadeFactor = -fadeFactor;
				if (nextMusic != null)
					src.clip = nextMusic;
				src.Play ();
			}
			if (src.volume == 1)
				fading = false;
		}
	}

	public void fadeMusic(AudioClip music){
		if (music != src.clip) {
			fadeFactor = Mathf.Abs (fadeFactor);
			fading = true;
			nextMusic = music;
		}
	}
}
