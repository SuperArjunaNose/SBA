using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetData {

	public bool isInhabited;
	public bool isFarmable;
	public bool isTerraformable;
	public bool ownedByPlayer;
	public List<PlantData> growies;
	public string systemAndPlace;

	//[SerializeField] SpriteRenderer srenderer;

//	void Awake() {
//		srenderer = GetComponent<SpriteRenderer> ();
//	}

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
