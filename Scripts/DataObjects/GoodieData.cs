using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodieData {

	public string name;
	public int fertilizerPoints;
	public int moisturePoints;
	public string goodieIconFileName;

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void DumpGoodie() {
		Debug.Log(			"Goodie Name: " + name +
			" Moisture Added: " + moisturePoints +
			" Fertility Added: " + fertilizerPoints +
			" Icon File Name: " + goodieIconFileName);

	}
	public static GoodieData ObjectFromRaw(GoodieDataRawObject gdro) {
		GoodieData g = new GoodieData();
		g.name = gdro.goodieName;
		g.moisturePoints = gdro.moisturePoints;
		g.fertilizerPoints = gdro.fertilizerPoints;
		g.goodieIconFileName = gdro.goodieIconFileName;

		return g;
	}

}
