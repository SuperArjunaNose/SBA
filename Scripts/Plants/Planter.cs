using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour, PlanetAttachable {

	private Plant plant;
	private Animator anim;
	private RuntimeAnimatorController animNextTurn;
	public int fertilizer_points;
	public int moisture_points;
	public Vector3 plantPosition;
	public bool seedPlanted;

	void Awake() {
		Messenger.AddListener (GameEvent.TURN_ADVANCED, OnTurnAdvanced);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.TURN_ADVANCED, OnTurnAdvanced);
	}

	public bool TakeResources(int fertilizerTaken, int moistureTaken) {
		if (fertilizerTaken < fertilizer_points) {
			fertilizer_points -= fertilizerTaken;
		}
		else return false;

		if (moistureTaken < moisture_points) {
			moisture_points -= moistureTaken;
		}
		else return false;

		return true;
	}

	public string DumpInfo() {
		string str = "";
		if (plant && !seedPlanted)
			str += plant.GetItemName ();
		else
			str += "Space Planter";
//		if (plant && seedPlanted) {
//			str += plant.GetComponent<SpriteRenderer> ().sprite.name;
//		} else if (plant && !seedPlanted) {
//			str += plant.GetItemName ();
//		} else {
//			str += "Space Planter";
//		}
		str += "\nFertilizer: " + fertilizer_points + "\nMoisture: " + moisture_points;
		if (seedPlanted)
			str += "\nSeed Planted";
		return str;
	}

	public void ShowPlant(RuntimeAnimatorController r) {
		anim.runtimeAnimatorController = r;
		transform.position = new Vector3(transform.position.x, transform.position.y-0.48f, transform.position.z);
		seedPlanted = false;
	}

	public void PlantSeed(Plant p) {
		seedPlanted=true;
		plant = p;
		plant.SetPlanter (this);
//		animNextTurn = r;
	}

	public void ApplyGoodie(Goodie g) {
		fertilizer_points += g.fertilizerPoints;
		moisture_points += g.moisturePoints;
	}

	void OnTurnAdvanced() {

	}

	// Use this for initialization
	void Start () {
		anim = GetComponent (typeof(Animator)) as Animator;
		fertilizer_points = 3;
		moisture_points = 3;
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
