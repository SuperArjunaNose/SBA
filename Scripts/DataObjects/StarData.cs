using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StarType {
	M, 		//Red
	K,		//Orange
	G,		//Yellow
	B		//Blue
};

public class StarData {

	public StarType starClass;
	public int photonsEmitted;
	public String name;
	public List<PlanetData> planets = new List<PlanetData> ();

//	// Use this for initialization
//	void Awake () {
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public string dumpSystem() {
		string s = "Star name: " + name + " Star Class: " + starClass.ToString() + " Planets: " + planets.Count + "\n";

		for (int i = 0; i < planets.Count ; i++) {
			if (planets [i].isFarmable)
				s = string.Concat (s, "Planet " + (i+1) + " is Farmable\n");
			else if(planets [i].isInhabited)
				s = string.Concat (s, "Planet " + (i+1) + " is Inhabited\n");
			else if(planets [i].isTerraformable)
				s = string.Concat (s, "Planet " + (i+1) + " is Terraformable\n");
			else 
				s = string.Concat (s, "Planet " + (i+1) + " is a dud\n");

			if(planets[i].ownedByPlayer)
				s = string.Concat (s, "This planet is owned by the player\n");
		}

		return s;
	}
}
