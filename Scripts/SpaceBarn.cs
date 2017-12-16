using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBarn : MonoBehaviour {

	public List<UsableItem> tgs_list; //TODO: Rethink this data type WHEN WE GET AROUND TO INSTANTIATING ALL THIS LOVELY DATA WE LOADED AND GENERATED
	public List<Fruit> harvest;

	public Dictionary<string, int> ToolsGoodiesSeeds;
	public Dictionary<string, int> fruitInventory;

	public int spaceBerries;

	// Use this for initialization
	void Awake () {
		tgs_list = new List<UsableItem> ();
		harvest = new List<Fruit>();
		ToolsGoodiesSeeds = new Dictionary<string, int>();
		fruitInventory = new Dictionary<string, int>();
		spaceBerries = 500;
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public Sprite FruiticonFromString(string name) {
		foreach (Fruit f in harvest) {
			if (name == f.fruitName)
				return f.fruitIcon;
		}
		return null;
	}	

	public string FruiticonFilenameFromString(string name) {
		foreach (Fruit f in harvest) {
			if (name == f.GetItemName())
				return f.GetIconFileName();
		}
		return null;
	}	

	public string TGSiconFileNameFromString(string name) {
		Debug.Log (name);
		foreach (UsableItem u in tgs_list) {
			if (name == u.GetItemName ())
				return u.GetIconFileName();

		}
		Debug.Log ("null");
		return null;
	}

	public Sprite TGSiconFromString(string name) {
		Debug.Log (name);
		foreach (UsableItem u in tgs_list) {
			if (name == u.GetItemName())
				return u.GetIcon();
			
		}
		Debug.Log ("null");
		return null;
	}

	public UsableItem TGSItemFromString(string name) {
		Debug.Log (name);
		foreach (UsableItem u in tgs_list) {
			if (name == u.GetItemName())
				return u;

		}
		Debug.Log ("null");
		return null;
	}

	public int AddGoodie(GameObject goodieToAddObject) {
		Goodie goodieToAdd = goodieToAddObject.GetComponent<Goodie>();
		if (ToolsGoodiesSeeds == null || goodieToAddObject == null || goodieToAdd == null)
			Debug.Log ("of mans first disobediance and the fruit");
		if(ToolsGoodiesSeeds.ContainsKey(goodieToAdd.GetItemName()))
			ToolsGoodiesSeeds[goodieToAdd.GetItemName ()]++;
		else 
			ToolsGoodiesSeeds [goodieToAdd.GetItemName ()] = 1;
		//ToolsGoodiesSeeds.AddItem (goodieToAdd.name);
		tgs_list.Add (goodieToAdd);
		return ToolsGoodiesSeeds [goodieToAdd.GetItemName ()];
	}

	public int AddPlant(Plant plantToAdd) {
		if(ToolsGoodiesSeeds.ContainsKey(plantToAdd.GetItemName()))
			ToolsGoodiesSeeds[plantToAdd.GetItemName ()]++;
		else 
			ToolsGoodiesSeeds[plantToAdd.GetItemName ()] = 1;
		//ToolsGoodiesSeeds.AddItem (goodieToAdd.name);
		tgs_list.Add (plantToAdd);
		return ToolsGoodiesSeeds [plantToAdd.GetItemName ()];
	}

	public int AddFruit(Fruit fruitToAdd) {
		if(fruitInventory.ContainsKey(fruitToAdd.GetItemName()))
			fruitInventory[fruitToAdd.GetItemName ()]++;
		else 
			fruitInventory[fruitToAdd.GetItemName ()] = 1;
		harvest.Add (fruitToAdd);
		return fruitInventory [fruitToAdd.GetItemName ()];
	}

	public int RemoveGoodie(Goodie goodieToRemove) {
		if (ToolsGoodiesSeeds.ContainsKey (goodieToRemove.GetItemName ())) {
			ToolsGoodiesSeeds [goodieToRemove.GetItemName ()]--;
			return ToolsGoodiesSeeds [goodieToRemove.GetItemName ()];
		}
		else return 0;
	}

	public int RemovePlant(Plant plantToRemove) {
		if (ToolsGoodiesSeeds.ContainsKey (plantToRemove.GetItemName ())) {
			ToolsGoodiesSeeds [plantToRemove.GetItemName ()]--;
			return ToolsGoodiesSeeds [plantToRemove.GetItemName ()];
		}
		else return 0;
	}

	public int RemoveFruit(Fruit fruitToRemove) {
		if (fruitInventory.ContainsKey (fruitToRemove.GetItemName ())) {
			fruitInventory[fruitToRemove.GetItemName ()]--;
			return fruitInventory [fruitToRemove.GetItemName ()];
		}
		else return 0;
	}

}
