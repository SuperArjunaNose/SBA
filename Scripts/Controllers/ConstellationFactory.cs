using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class PlantDataRawObject {
	public string plantName;
	public int moisture_required;
	public int soil_fertility;
	public int photons_required;
	public bool canTravel;
	public PlantState plantState;
	public string imageFile;
	public string plantAnimationFile;
	public string plantSeedIconFile;
}

[System.Serializable]
public class PlantDataRawObjectList {
	public List<PlantDataRawObject> pdata = new List<PlantDataRawObject>();
}

[System.Serializable]
public class GoodieDataRawObject {
	public string goodieName;
	public int fertilizerPoints;
	public int moisturePoints;
	public string goodieIconFileName;
}

[System.Serializable]
public class GoodieDataRawObjectList {
	public List<GoodieDataRawObject> goodieData = new List<GoodieDataRawObject>();
}

[System.Serializable]
public class ShopDataRawObject {
	public string shopGraphicFileName;
	public string shopName;
	public ShopkeeperMood shopkeeperMood;
}

[System.Serializable]
public class ShopDaraRawObjectList {
	public List<ShopDataRawObject> shopData = new List<ShopDataRawObject>();
}

public class ConstellationFactory : MonoBehaviour {

	public SimulationManager smgr;
	private string PlantDataFileName = "plantData.json"; 
	private string GoodieDataFileName = "goodieData.json";
	private String PlantDataPatchFileName;  //TODO: Add functionality so that on PC players can load a "patch file" to modify the PlantDataData data

	void Start() {

	}

	//Creates a Star object with the specified name, and randomly generated:
	//		Star class type
	//		photon emission value (with multiplier based on its class
	public StarData CreateStar(string name) {
		StarData s = new StarData ();

		//Set the name of the star
		s.name = name;
		//Debug.Log ("Set star name: " + s.name);

		//Randomly select the star class
		Array sClasses = Enum.GetValues (typeof(StarType));
		s.starClass = (StarType)sClasses.GetValue ((int)Random.Range (0, sClasses.Length));
		//Debug.Log ("Set star class: " + s.starClass);

		//Depending on the class of the star, set the photonsEmitted value with a multiplier so the brighter the star, the more photonsEmitted
		//TODO: Add mycorrhizatrons, technobabble subatomic particles that do weird things to the space PlantDataDatas
		switch (s.starClass) {
			case StarType.M:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted));
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " on star " + s.name);
				break;
			case StarType.K:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted) * smgr.classKMultiplier);
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " with StarType class multiplier " + smgr.classKMultiplier + " on star " + s.name);
				break;
			case StarType.G:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted) * smgr.classGMultiplier);
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " with StarType class multiplier " + smgr.classGMultiplier + " on star " + s.name);
				break;
			case StarType.B:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted) * smgr.classBMultiplier);
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " with StarType class multiplier " + smgr.classBMultiplier + " on star " + s.name);
				break;
			default:
				Debug.LogError ("Error in setting star type.  Please check ConstellationFactory.CreateStar (string name)");
				break;
		}

		return s;
	}

	//Creates a Star object with the specified name, and randomly generated:
	//		Star class type
	//		photon emission value (with multiplier based on its class
	public StarData CreateStar(string name, StarType stype) {
			StarData s = new StarData ();

			//Set the name of the star
			s.name = name;
			//Debug.Log ("Set star name: " + s.name);

			//Randomly select the star class
			s.starClass = stype;
			//Debug.Log ("Set star class: " + s.starClass);

			//Depending on the class of the star, set the photonsEmitted value with a multiplier so the brighter the star, the more photonsEmitted
			//TODO: Add mycorrhizatrons, technobabble subatomic particles that do weird things to the space PlantDataDatas
			switch (stype) {
				case StarType.M:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted));
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " on star " + s.name);
				break;
			case StarType.K:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted) * smgr.classKMultiplier);
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " with StarType class multiplier " + smgr.classKMultiplier + " on star " + s.name);
				break;
			case StarType.G:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted) * smgr.classGMultiplier);
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " with StarType class multiplier " + smgr.classGMultiplier+ " on star " + s.name);
				break;
			case StarType.B:
				s.photonsEmitted = (int)Math.Ceiling(Random.Range (smgr.minPhotonsEmitted, smgr.maxPhotonsEmitted) * smgr.classBMultiplier);
				//Debug.Log ("Set star photon emission: " + s.photonsEmitted + " with StarType class multiplier " + smgr.classBMultiplier+ " on star " + s.name);
				break;
			default:
				//Debug.LogError ("Error in setting star type.  Please check ConstellationFactory.CreateStar(string name, StarType stype)");
				break;
			}

			return s;
	}

	public PlanetData CreatePlanet() {
//		int temp;
		PlanetData pt = new PlanetData ();
		pt.isInhabited = (Random.Range (0f, 1f) > (1 - smgr.inhabitedProbability));  //
		if (pt.isInhabited) {
			pt.isFarmable = false;
			pt.isTerraformable = false;
		}
		else {
			pt.isFarmable = (Random.Range (0f, 1f) > (1 - smgr.farmableProbability));
			if (!pt.isFarmable) {
				pt.isTerraformable = (Random.Range (0f, 1f) > (1 - smgr.terraformableProbability));
			} else
				pt.isTerraformable = false;
		}

		pt.ownedByPlayer = false;
		return pt;

		//Calling it a night here
		//Want a generic shuffle method
		//Like stars, 1 inhabited planet, 1 farmable planet, the rest can be whatever, Random.Range (2,10) planets total
		//Do I need a generic shuffle method?  I could just pick a random number from Random.Range(2,10) and insert the inhabited and farmable planets when I'm at that iteration of the for loop
	}

	public PlanetData CreatePlanet(bool isInhabited, bool isFarmable, bool isTerraformable, bool ownedByPlayer) {
		PlanetData pt = new PlanetData ();
		pt.isInhabited = isInhabited;
		pt.isFarmable = isFarmable;
		pt.isTerraformable = isTerraformable;
		pt.ownedByPlayer = ownedByPlayer;
		return pt;
	}

	public HostileMeaty CreateHostileMeaty(string name) {
		//int temp;
		//if(isHostile)
		//	Meaty m = new Meaty ();


		return null;
	}

//	public FriendlyMeaty CreateFriendlyMeaty(string name) {
//		int temp;
//		//if(isHostile)
//		//	Meaty m = new Meaty ();
//
//
//		return null;
//	}

	//Randomly generates a PlantDataData
	//REQUIRE: PlantDataData must be in the SEED state
	public List<PlantData> CreatePlantDatas() {
		string PlantDataFilePath = Path.Combine (Application.streamingAssetsPath, PlantDataFileName);

		//Set up the temp stuff we know ahead of time
		//int temp;
		PlantData p;
		PlantDataRawObjectList pdrol;
		List<PlantData> Plants = new List<PlantData>();
		if (File.Exists (PlantDataFilePath)) {
			Debug.Log("in CreatePlantDatas, file exists");
			string PlantDataJSONData = File.ReadAllText (PlantDataFilePath);
			pdrol = JsonUtility.FromJson<PlantDataRawObjectList>(PlantDataJSONData);
			for (int i = 0; i < pdrol.pdata.Count; i++) {
				p = PlantData.ObjectFromRaw (pdrol.pdata [i]);
				p.DumpPlant();
				Plants.Add (p);
			}

		} else {
			Debug.LogError("Error: PlantData data file not found.  Are you missing plantData.json?");
		}
		//TODO: plant data patch file handling here

		return Plants;
	}

	//Load Goodie Data
	public List<GoodieData> CreateGoodieDatas() {
		string GoodieDataFilePath = Path.Combine (Application.streamingAssetsPath, GoodieDataFileName);

		//Set up the temp stuff we know ahead of time
		//int temp;
		GoodieData g;
		GoodieDataRawObjectList gdrol;
		List<GoodieData> goodies = new List<GoodieData>();
		if (File.Exists (GoodieDataFilePath)) {
			Debug.Log("in CreateGoodieDatas, file exists");
			string GoodieDataJSONData = File.ReadAllText (GoodieDataFilePath);
			gdrol = JsonUtility.FromJson<GoodieDataRawObjectList>(GoodieDataJSONData);
			for (int i = 0; i < gdrol.goodieData.Count; i++) {
				g = GoodieData.ObjectFromRaw (gdrol.goodieData [i]);
				g.DumpGoodie();
				goodies.Add (g);
			}

		} else {
			Debug.LogError("Error: Goodie data file not found.  Are you missing goodieData.json?");
		}
		//TODO: Goodie data patch file handling here

		return goodies;
	}
}
