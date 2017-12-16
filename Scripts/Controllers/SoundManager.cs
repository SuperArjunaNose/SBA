using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

	public static SoundManager sndmgr = null;
	public float SFXVolume;
	public float MusicVolume;

	public AudioClip mus1;
	public AudioClip mus2;
	public AudioClip mus3;
	private AudioSource asource;
	private AudioSource SFXasource;
	// Use this for initialization

	void Awake() {
		if (sndmgr == null)
			sndmgr = this;
		else if (sndmgr != this)
			Destroy(gameObject); 

		DontDestroyOnLoad(gameObject);
	}

	public void OnMusicValue(Slider sldr) {
		asource.volume = sldr.value;
	}

	void Start () {
		int rand = Random.Range (0, 3);
		asource = GetComponent (typeof(AudioSource)) as AudioSource;
		switch (rand) {
		case 0:
			asource.clip = mus1;
			asource.Play ();
			break;
		case 1:
			asource.clip = mus2;
			asource.Play ();
			break;
		case 2:
			asource.clip = mus3;
			asource.Play ();
			break;
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
