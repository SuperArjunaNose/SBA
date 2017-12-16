using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ANGRY = 0
//NEUTRAL = 1
//HAPPY = 2
public enum ShopkeeperMood {
	ANGRY,
	NEUTRAL,
	HAPPY
}

public class Shop : MonoBehaviour, PlanetAttachable {

	private SpriteRenderer srenderer;
	#if UNITY_ANDROID || UNITY_IOS
	public Sprite shopGraphic;
	#else
	private Sprite shopGraphic;
	public string shopGraphicFileName;
	#endif

	public float ShopPixelsPerUnit;
	private string shopName;
	private ShopkeeperMood mood;

	// Use this for initialization
	void Start () {
		srenderer = GetComponent (typeof(SpriteRenderer)) as SpriteRenderer;
	}

	public void LoadFromDataFile(ShopDataRawObject sdro) {
		shopGraphicFileName = sdro.shopGraphicFileName;
		shopName = sdro.shopName;
		mood = sdro.shopkeeperMood;
		TurnController.LoadSprite (shopGraphicFileName, srenderer, ShopPixelsPerUnit);
	}

	public string DumpInfo() {
		return "Galactic Farmer's Union\nClick shop to enter";
	}

//	// Update is called once per frame
//	void Update () {
//		
//	}
}
