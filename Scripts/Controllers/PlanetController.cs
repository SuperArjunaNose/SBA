using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {

	[SerializeField] private Vector3 sittingPlantPosition;

	public Animator[] animArr;
	//public Sprite defaultSprite;
	//public string plant;
	//public RuntimeAnimatorController sStraw;
	//public RuntimeAnimatorController oOrange;

	//REQUIRE: Collider c must have an Animator compoenent attached to its gameObject
	public void TransitionPlantAnimation(Collider c, string controllerName) {
		c.gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load (controllerName) as RuntimeAnimatorController;
	}

//	public void SitDownPlant(Collider c) {
//	}

	// Use this for initialization
//	void Start () {
		//animArr = gameObject.GetComponentsInChildren<Animator> ();
		//sStraw 
		
//	}
	
	// Update is called once per frame
//	void Update () {
//		if(Input.GetKeyDown(KeyCode.F)) {
//			Debug.Log ("Keypress of F detected");
//			foreach(Animator a in animArr) {
//				a.runtimeAnimatorController = Resources.Load("SolarStrawberryC") as RuntimeAnimatorController;
//				a.Play (plant);
//			}
//		}
//		if(Input.GetKeyDown(KeyCode.O)) {
//			Debug.Log ("Keypress of O detected");
//			foreach(Animator a in animArr) {
//				a.runtimeAnimatorController = oOrange;
//				a.Play (plant);
//			}
//		}
//	}
}
