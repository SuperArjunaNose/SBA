using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Plant : MonoBehaviour, UsableItem {

	public string plantName;
	public string fruitImageFile;
	public string plantAnimationFile;
	public int moistureFromSoil;
	public int fertilityFromSoil;
	public int photonsRequired;
	public string plantSeedIconfile;
	public bool canTravel;
	public PlantState plantState;
	//public Animator a;
	public GameObject spaceBarnObject;
	public float fruitPixelsPerUnit=32.0f;
	private RuntimeAnimatorController plantAnimation;

	public SimulationManager smgr;
	private Sprite plantSeedIcon;
	private Planter planter;
	[SerializeField] Fruit fruitIProduce;

	public void SetPlanter(Planter pntr) {
		planter = pntr;
	}

	public string GetItemType() {
		return "Plant";
	}


	//TODO: Refactor this so users can substitute their own animation files
	//public string plantAnimationFile;
	//public Sprite sprte;

	public string GetItemName() {
		if (plantState == PlantState.SEED)
			return plantName + " seed";
		else
			return plantName + " plant";
	}

	public Sprite GetIcon() {
		return plantSeedIcon;
	}

	public string GetIconFileName() {
		if (plantState == PlantState.SEED)
			return plantSeedIconfile;
		else
			return plantAnimationFile;
	}

	public RuntimeAnimatorController ShowPlantAnim() {
		return plantAnimation;
	}

	public void LoadFromPlantData(PlantData pd) {
		plantName=pd.plantName;
		moistureFromSoil = pd.moisture_required;
		fertilityFromSoil = pd.soil_fertility;
		photonsRequired = pd.photonsRequired;
		canTravel = pd.canTravel;
		plantAnimationFile = pd.plantAnimationFile;
		fruitImageFile = pd.imageFile;
		plantSeedIconfile = pd.plantSeedIconFile;
		#if UNITY_STANDALONE
		StartCoroutine(TurnController.LoadSprite(Path.Combine(Application.streamingAssetsPath, plantSeedIconfile), plantSeedIcon, fruitPixelsPerUnit));
		#elif UNITY_ANDROID || UNITY_IOS
		switch(produce.fruitName) { case "Solar Strawberry": break; case "Apogee Apple": break; case "Ozone Orange": break; case "Lunation Lemon": break; case "Waxing Watermelon": break;}
		#endif


		//TODO: REWRITE this so we somehow get it out of Streaming Assets
		plantAnimation = Resources.Load (plantAnimationFile) as RuntimeAnimatorController;

		fruitIProduce = Instantiate (smgr.fruitPrefab.gameObject.GetComponent<Fruit>());
		fruitIProduce.LoadFruitFromPlantData (plantName, pd.imageFile, 32.0f);
	}

	void Awake() {
		Messenger.AddListener (GameEvent.TURN_ADVANCED, OnTurnAdvance);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.TURN_ADVANCED, OnTurnAdvance);
	}

	Fruit ProduceFruit() {
		Fruit produce = Instantiate (fruitIProduce);
		produce.fruitName = plantName;
		//#if UNITY_STANDALONE
		//StartCoroutine(TurnController.LoadSprite(Path.Combine(Application.streamingAssetsPath, produce.fruitName), produce.fruitIcon, fruitPixelsPerUnit));
		//#elif UNITY_ANDROID || UNITY_IOS
//		switch(produce.GetItemName()) { 
//			case "Solar Strawberry": 
//			break; 
//			case "Apogee Apple": 
//				break; 
//			case "Ozone Orange": 
//				break; 
//			case "Lunation Lemon": 
//				break; 
//			case "Waxing Watermelon": 
//			break;
//		}
		//#endif
		return produce;
	}

	void OnTurnAdvance() {
		Debug.Log ("In OnTurnAdvance");
		if (planter != null && planter.TakeResources (fertilityFromSoil, moistureFromSoil)) {
			Debug.Log("Took Resources");
			spaceBarnObject.GetComponent<SpaceBarn>().AddFruit (ProduceFruit ());
			if (plantState == PlantState.SEED) {
				plantState = PlantState.PRODUCING_FRUIT;
				planter.ShowPlant (ShowPlantAnim ());
			}
		}


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
