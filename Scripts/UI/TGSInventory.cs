using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGSInventory : InventoryInterface {

	private Dictionary<string, int> inventoryDict;

	public void AddItem(string itemToAdd) {
		if (inventoryDict.ContainsKey (itemToAdd)) {
			inventoryDict [itemToAdd]++;
		} 
		else inventoryDict [itemToAdd] = 1;

	}

	public void RemoveItem(string itemToRemove) {
		if (inventoryDict.ContainsKey (itemToRemove)) {
			inventoryDict [itemToRemove]--;
		}

	}

	public Dictionary<string, int> DisplayInventory() {
		return inventoryDict;
	}

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}




}
