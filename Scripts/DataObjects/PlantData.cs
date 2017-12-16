using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Noting numbers here for JSON deserialization purposes
//SEED: 			0
//PRODUCING FRUIT:	1
//POLLINATING:		2
public enum PlantState {
	SEED, 		
	PRODUCING_FRUIT,		
	POLLINATING		
};

public class PlantData {

	public string plantName;
	public string plantSeedIconFile;
	public int moisture_required;
	public int soil_fertility;
	public int photonsRequired;
	public bool canTravel;
	public PlantState plantState;
	public SpriteRenderer srenderer;
	public Animation animator;
	public string imageFile;
	public string plantAnimationFile;

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void DumpPlant() {
		Debug.Log(			"Plant Name: " + plantName +
							" Moisture Required: " + moisture_required +
							" Soil Fertility: " + soil_fertility +
							" Photons Required: " + photonsRequired +
							" Can Travel? " + canTravel +
							" Plant State: " + plantState.ToString() +
							" Image File String: " + imageFile +
							" Plant Animation File String: " + plantAnimationFile);

	}
		
	public static PlantData ObjectFromRaw(PlantDataRawObject pedro) {
		PlantData p = new PlantData();
		p.plantName = pedro.plantName;
		p.moisture_required = pedro.moisture_required;
		p.soil_fertility = pedro.soil_fertility;
		p.photonsRequired = pedro.photons_required;
		p.canTravel = pedro.canTravel;
		p.plantState = pedro.plantState;
		p.imageFile = pedro.imageFile;
		p.plantAnimationFile = pedro.plantAnimationFile;
		p.plantSeedIconFile = pedro.plantSeedIconFile;

		return p;
	}
}
