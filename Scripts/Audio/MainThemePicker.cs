using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainThemePicker : MonoBehaviour {

	public AudioClip mus1;
	public AudioClip mus2;
	public AudioClip mus3;
	private AudioSource asource;
	// Use this for initialization
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

}
