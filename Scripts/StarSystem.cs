using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarSystem : MonoBehaviour {

	public SpriteRenderer starRenderer;
	public String name;
	public List<GameObject> planets;
	public int photons_emitted;

	public GameObject tcObject;
	private TurnController tc;

	#if UNITY_ANDROID || UNITY_IOS
	[SerializeField] private Sprite ClassMStar;
	[SerializeField] private Sprite ClassKStar;
	[SerializeField] private Sprite ClassGStar;
	[SerializeField] private Sprite ClassBStar;
	#endif

	public void LoadStarData(StarData sd, float ppu) {
		name = sd.name;
		photons_emitted = sd.photonsEmitted;
		#if UNITY_STANDALONE
		tc = tcObject.GetComponent<TurnController> ();
		#endif
		#if UNITY_STANDALONE
			//Based on the class of star, load in its sprite from StreamingAssets
			//TODO: Change hardcoding of sprite names?
			switch (sd.starClass) {
				case StarType.M:
					StartCoroutine (TurnController.LoadSprite (Path.Combine(Application.streamingAssetsPath, "ClassMStar.png"), starRenderer, ppu));
					break;
				case StarType.K:
					StartCoroutine (TurnController.LoadSprite (Path.Combine(Application.streamingAssetsPath, "ClassKStar.png"), starRenderer, ppu));
					break;
				case StarType.G:
					StartCoroutine (TurnController.LoadSprite (Path.Combine(Application.streamingAssetsPath, "ClassGStar.png"), starRenderer, ppu));
					break;
				case StarType.B:
					StartCoroutine (TurnController.LoadSprite (Path.Combine(Application.streamingAssetsPath, "ClassBStar.png"), starRenderer, ppu));
					break;
			}
		#elif UNITY_ANDROID || UNITY_IOS
			switch (sd.starClass) {
				case StarType.M:
					starRenderer.sprite = ClassMStar;
					break;
				case StarType.K:
					starRenderer.sprite = ClassKStar;
					break;
				case StarType.G:
					starRenderer.sprite = ClassGStar;
					break;
				case StarType.B:
					starRenderer.sprite = ClassBStar;
					break;
			}
		#endif
	}

//	// Use this for initialization
	void Awake () {
		starRenderer = GetComponent<SpriteRenderer> ();
		tcObject = GameObject.FindGameObjectWithTag ("SimulationManager");
	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
