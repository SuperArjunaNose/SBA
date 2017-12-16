using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimulationManager : MonoBehaviour {

	public static SimulationManager mgr = null;
	public List<StarData> starSystems;
	public List<PlantData> plants;
	public List<GameObject> plantObjects;
	public List<GoodieData> goodieDataList;
	public List<GameObject> goodieObjects;
	public List<HostileMeaty> meaties;
	public List<Fruit> fruitObjects;
	public SpaceBarn barn;
	public int turn;
	public StarData startingSystem; //TODO: Rethink this data type WHEN WE GET AROUND TO INSTANTIATING ALL THIS LOVELY DATA WE LOADED AND GENERATED
	public Transform plantPrefab;
	public Transform goodiePrefab;
	public Transform fruitPrefab;


	private string[] starSystemList = new string[] {"Arctium", "Calendula", "Daucus", "Hippophae", "Lycium", "Symphytum", "Urtica"};
	private Stack<String> starNameList;
	private ConstellationFactory cf;

	//SIMULATION PARAMETERS
	public int numStarSystems = 7;
	public float minPhotonsEmitted;
	public float maxPhotonsEmitted;
	public float classKMultiplier;
	public float classGMultiplier;
	public float classBMultiplier;

	[Range(0,1)]
	public float inhabitedProbability;
	[Range(0,1)]
	public float farmableProbability;
	[Range(0,1)]
	public float terraformableProbability;



	//Before everybody calls Start()...
	void Awake () {
		if (mgr == null)
			mgr = this;

		else if (mgr != this)
			Destroy(gameObject); 

		DontDestroyOnLoad(gameObject); //We might switch to a different scene to handle combat sequences, so I don't want this going anywhere.
		starSystems = new List<StarData>();
		ConstructConstellation();
	}

//	void Start() {
//	}

//	// Update is called once per frame
//	void Update () {
//		
//	}


	/**
	*	---STAR CREATION---
	*	The Galactic Department of Agriculture (GDA) divides the four star classes into a set of Space PlantDataData Hardiness Zones
	*	Class M and K: Zones 1-3
	*	Class G: Zones 4-6
	*	Class B: Zones 7-13
	*
	*	The zones are based on the minimum amount of photons a space PlantDataData receive (shhh, don't think about the physics too much)
	*	The following PlantDataDatas are hardy to these hardiness zones:
	*	
	*	Apogee Apple, Solar Strawberry, Waxing Watermelon: Zone 1
	*	Ozone Orange: Zone 4
	*	Lunation Lemon: Zone 7
	*	TODO: Place new PlantDataDatas into these zones.
	*
	*	So there's a sense of progression and challenge:
	*		-Create ONE class M or K star (PLAYER STARTS HERE)
	*		-Create ONE class G star 
	*		-Create ONE class B star
	*		-The other starSystems.Length - 3 stars can be whatever class
	*
	*	---PLANET CREATION---
	*
	*   We'll set our startingSystem Star reference to the created 
	*   Class M star system so we can control what planets get added to it later.  We MUST add:
	*   1 inhabited planet
	*	1 farmable planet that the player will own
	*
	*	Additionally, the Ozone Orange and Lunation Lemons will be impossible to grow UNLESS we do this:
	*	1. The class G star must have AT LEAST ONE farmable planet
	*	2. The class B star must have AT LEAST ONE terraformable planet
	*
	* 	---PlantDataData CREATION---
	* 
	* 	---MEATY CREATION---
	* 
	* 
	* REQUIRE: starNameList.Length() == numStarSystems
	**/
	void ConstructConstellation() {
		//Shuffle up the list of star names, create the ConstellationFactory, set up some temp variables
		starNameList = new Stack<String> (ShuffledStarNameList());
		cf = GetComponent<ConstellationFactory> ();
		int i = 0;
		StarData gSystem, bSystem;

		//START STAR CREATION
		//Create a class M star
		startingSystem = cf.CreateStar (starNameList.Pop (), StarType.M);
		starSystems.Add (startingSystem);
		i++;

		//Put Space Barn placement here?  Sorta.  We'll set our startingSystem Star reference to the created 
		//Class M star system so we can control what planets get added to it later.  We MUST add:
		//1 inhabited planet
		//1 farmable planet that the player will own
		//to this system.

		//Create a class G star
		gSystem = cf.CreateStar (starNameList.Pop (), StarType.G);
		starSystems.Add (gSystem);
		i++;

		//Create a class B star
		bSystem = cf.CreateStar (starNameList.Pop (), StarType.B);
		starSystems.Add (bSystem);
		i++;
		for (i = i; i < numStarSystems; i++) {
			starSystems.Add (cf.CreateStar (starNameList.Pop ()));
		}

		Debug.Log ("Final sanity check for star creation: ");
		 foreach(StarData star in starSystems)
			Debug.Log("Name: " + star.name + " Class: " + star.starClass);

		//END STAR CREATION

		//---------------------------------------------------

		//START PLANET CREATION
		//
		//-----------START STARTING SYSTEM GENERATION-----------
		//After deciding how many planets will be in the system, we want to randomize where in the solar system these planets are
		//Create the place in the system for:
		//A. the inhabited planet
		//B. the farmable planet

		//How many planets in the system?  Roll the dice, man, roll the dice!
		int startingSystemNumberPlanets = (int)Random.Range (2, 10);

		//Where will the inhabited planet be in this system?  Roll the dice, man, roll the dice!
		int startingInhabitedPlanetPlace = (int)Random.Range (0, startingSystemNumberPlanets);

		//Where will the farmable planet be in this system?  Roll the dice, man, roll the dice!
		int startingFarmablePlanetPlace = 0;

		//We can't have the inhabited planet in the same place as the farmable planet, so is that has happened we roll the dice until that ain't a thing anymore
		//I'm worried this could be an infinite loop in the making, so I put a Debug.Log in to check
		while (startingInhabitedPlanetPlace == startingFarmablePlanetPlace) {
			Debug.Log ("In planet placement loop.  You should not see this message more than 2 or 3 times tops.");  //Just 'cause I'm paranoid doesn't mean C# isn't out to get me
			startingFarmablePlanetPlace = (int)Random.Range (0, startingSystemNumberPlanets);
		}

		PlanetData createdPData;

		for (int p = 0; p < startingSystemNumberPlanets; p++) {
			//If this is the place we're adding the inhabited planet, add it to the planets list here
			if (p == startingInhabitedPlanetPlace) {
				createdPData = cf.CreatePlanet (true, false, false, false);
				createdPData.systemAndPlace = startingSystem.name + " " + (p+1);
				startingSystem.planets.Add (createdPData);
			}
			//Ditto with the farmable planet
			else if (p == startingFarmablePlanetPlace) {
				createdPData = cf.CreatePlanet (false, true, false, true);
				createdPData.systemAndPlace = startingSystem.name + " " + (p+1);
				startingSystem.planets.Add (createdPData);
			}
			//Otherwise do whatever, but make sure the planet isn't owned by the player
			else {
				createdPData = cf.CreatePlanet();
				createdPData.systemAndPlace = startingSystem.name + " " + (p+1);
				startingSystem.planets.Add (createdPData);
			}

		}
		//Debug.Log (startingSystem.dumpSystem ());


		//-----------END STARTING SYSTEM GENERATION-----------
		//-----------START G CLASS SYSTEM GENERATION-----------
		//I should probably move this off to a separate function call, and I would do that if I had time, but this is for a game jam

		//How many planets in the system?  Roll the dice, man, roll the dice!
		int gSystemNumberPlanets = (int)Random.Range (2, 10);

		//Where will the inhabited planet be in this system?  Roll the dice, man, roll the dice!
		int gInhabitedPlanetPlace = (int)Random.Range (0, startingSystemNumberPlanets);

		//Where will the farmable planet be in this system?  Roll the dice, man, roll the dice!
		int gFarmablePlanetPlace = 0;

		//We can't have the inhabited planet in the same place as the farmable planet, so is that has happened we roll the dice until that ain't a thing anymore
		//I'm worried this could be an infinite loop in the making, so I put a Debug.Log in to check
		while (gInhabitedPlanetPlace == gFarmablePlanetPlace) {
			Debug.Log ("In planet placement loop.  You should not see this message more than 2 or 3 times tops.");  //Just 'cause I'm paranoid doesn't mean C# isn't out to get me
			gFarmablePlanetPlace = (int)Random.Range (0, gSystemNumberPlanets);
		}

		for (int p = 0; p < gSystemNumberPlanets; p++) {
			//If this is the place we're adding the inhabited planet, add it to the planets list here
			if (p == startingInhabitedPlanetPlace) {
				createdPData = cf.CreatePlanet (true, false, false, false);
				createdPData.systemAndPlace = gSystem.name + " " + (p+1);
				gSystem.planets.Add (createdPData);
			}
			//Ditto with the farmable planet
			else if (p == startingFarmablePlanetPlace) {
				createdPData = cf.CreatePlanet (false, true, false, false);
				createdPData.systemAndPlace = gSystem.name + " " + (p+1);
				gSystem.planets.Add (createdPData);
			}
			//Otherwise do whatever, but make sure the planet isn't owned by the player
			else {
				createdPData = cf.CreatePlanet ();
				createdPData.systemAndPlace = gSystem.name + " " + (p+1);
				gSystem.planets.Add (createdPData);
			}

		}
		Debug.Log (gSystem.dumpSystem ());

		//-----------END G CLASS SYSTEM GENERATION-----------
		//-----------START B CLASS SYSTEM GENERATION-----------
		//Note: I'm going to make it 1 inhabited, 1 TERRAFORMABLE instead of farmable
		//How many planets in the system?  Roll the dice, man, roll the dice!
		int bSystemNumberPlanets = (int)Random.Range (2, 10);

		//Where will the inhabited planet be in this system?  Roll the dice, man, roll the dice!
		int bInhabitedPlanetPlace = (int)Random.Range (0, startingSystemNumberPlanets);

		//Where will the farmable planet be in this system?  Roll the dice, man, roll the dice!
		int bTerraformablePlanetPlace = 0;

		//We can't have the inhabited planet in the same place as the farmable planet, so is that has happened we roll the dice until that ain't a thing anymore
		//I'm worried this could be an infinite loop in the making, so I put a Debug.Log in to check
		while (bInhabitedPlanetPlace == bTerraformablePlanetPlace) {
			Debug.Log ("In planet placement loop.  You should not see this message more than 2 or 3 times tops.");  //Just 'cause I'm paranoid doesn't mean C# isn't out to get me
			bTerraformablePlanetPlace = (int)Random.Range (0, bSystemNumberPlanets);
		}

		for (int p = 0; p < bSystemNumberPlanets; p++) {
			//If this is the place we're adding the inhabited planet, add it to the planets list here
			if (p == bInhabitedPlanetPlace) {
				createdPData = cf.CreatePlanet (true, false, false, false);
				createdPData.systemAndPlace = bSystem.name + " " + (p+1);
				bSystem.planets.Add (createdPData);
			}
			//Ditto with the farmable planet
			else if (p == bTerraformablePlanetPlace) {
				createdPData = cf.CreatePlanet (false, false, true, false);
				createdPData.systemAndPlace = bSystem.name + " " + (p+1);
				bSystem.planets.Add (createdPData);
			}
			//Otherwise do whatever, but make sure the planet isn't owned by the player
			else {
				createdPData = cf.CreatePlanet ();
				createdPData.systemAndPlace = bSystem.name + " " + (p+1);
				bSystem.planets.Add (createdPData);
			}

		}
		Debug.Log (bSystem.dumpSystem ());

		//-----------END B CLASS SYSTEM GENERATION-----------
		//Generate the whatever else systems
		int nextSystemNumberPlanets = (int)Random.Range (2, 10);

		for(int x = 3; x < starSystems.Count; x++) {
			for (int d = 0; d < nextSystemNumberPlanets; d++) {
				createdPData = cf.CreatePlanet ();
				createdPData.systemAndPlace = starSystems [x].name + " " + (d + 1);
				starSystems [x].planets.Add (createdPData);
			}
				Debug.Log (starSystems[x].dumpSystem ());
				nextSystemNumberPlanets = (int)Random.Range (2, 10);
		}

		//END PLANET CREATION

		//START PlantData CREATION
		//I'm trying to make this part of the game moddable, which means doing the following:
		//
		//*** All the specifics of each PlantData are written down in a separate PlantDataa file, PlantData.json
		//
		//*** In ConstellationFactory, the CreatePlantDataDatas method reads in that JSON file and deserializes it 
		//into a PlantDataRawObjectList object
		//
		//*** As a debug measure, we call p.DumpPlantData() to make sure everything loaded in ok.
		//
		//*** CreatePlantDatas then iterates through the list in the PlantDataRawObjectList object and calls
		//a static method in the PlantData class to instantiate PlantData objects based on those properties,
		//and finally returns the list of created PlantDatas.
		Debug.Log("About to call CreatePlantDatas");
		plants = cf.CreatePlantDatas();
		goodieDataList = cf.CreateGoodieDatas ();
		//END PlantData CREATION
		//START PLANT CREATION
		foreach (PlantData pData in plants) {
			GameObject plantObject = Instantiate (plantPrefab.transform).gameObject;
			plantObject.GetComponent<Plant> ().LoadFromPlantData (pData);
			plantObjects.Add (plantObject);
		}
		//END PLANT CREATION

		//Start Fruit Creation
		foreach (PlantData pData in plants) {
			GameObject fruitObject = Instantiate (fruitPrefab.transform).gameObject;
			fruitObject.GetComponent<Fruit> ().LoadFruitFromPlantData (pData.plantName, pData.imageFile, 32.0f);
			fruitObjects.Add (fruitObject.GetComponent<Fruit> ());
		}
		//End Fruit Creation


		//Start Goodie CREATION
		foreach(GoodieData gData in goodieDataList) {
			GameObject goodieObject = Instantiate (goodiePrefab.transform).gameObject;
			goodieObject.GetComponent<Goodie> ().LoadFromGoodieData (gData);
			goodieObjects.Add (goodieObject);
		}

		//End Goodie CREATION
		//Begin Set SpaceBarn starting inventory
		barn.AddGoodie(goodieObjects[0]);
		barn.AddGoodie (goodieObjects[1]);
		barn.AddPlant (plantObjects [0].GetComponent<Plant> ());
		foreach(Fruit fr in fruitObjects)
			barn.AddFruit (fr);

		//End SpaceBarn starting inventory


		//START MEATY CREATION

		//END MEATY CREATION

	}

	void WriteGameStateToJSON() {
		
	}

	void ReadGameStateFromJSON() {
		
	}

	public List<String> ShuffledStarNameList() {
		List<String> scpy = new List<String>(starSystemList);
		int x = scpy.Count();
		int r;
		List<String> shuffled = new List<String> ();
		for (int i = 0; i < x; i++) {
			r = Random.Range (0, scpy.Count ());
			shuffled.Add(scpy.ElementAt(r));
			scpy.RemoveAt (r);
		}
		return shuffled;
	}
}