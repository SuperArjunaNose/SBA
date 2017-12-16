using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private List<GameObject> currentButtons;
	public GameObject tcObject;
	public GameObject MainMenuRootObject;
	public GameObject SpaceBarnRootObject;
	public GameObject StarSystemRootObject;
	public GameObject OptionsQuitRootObject;
	public GameObject FruitInventoryMenuRootObject;
	public GameObject TGSInventoryMenuRootObject;

	private SimulationManager sm;
	private TurnController tc;
	private GameObject ActiveMenuRootObject;
	private SpaceBarn spaceBarn;
	private bool firstActivation=true;
	private PlanetPointer planetPointer;
	// Use this for initialization
	void Start () {
		currentButtons = new List<GameObject> ();
		tc = tcObject.GetComponent<TurnController> ();
		sm = tcObject.GetComponent<SimulationManager> ();
		planetPointer = GameObject.FindGameObjectWithTag ("PlanetPointer").GetComponent<PlanetPointer> ();
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("MainMenuButton")) {
			currentButtons.Add (g);
		}
		Debug.Log (currentButtons.Count);
		ActiveMenuRootObject = MainMenuRootObject;
		spaceBarn = GameObject.FindGameObjectWithTag("SpaceBarn").GetComponent<SpaceBarn> ();
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	public void LoadMenu(string tag) {
		switch(tag) {
			case "SpaceBarnMenuItem":
				SpaceBarnRootObject.SetActive (true);
				for (int i = 0; i < SpaceBarnRootObject.transform.childCount; i++) {
					SpaceBarnRootObject.transform.GetChild (i).gameObject.SetActive (true);  
				}
				ActiveMenuRootObject.SetActive (false);
				ActiveMenuRootObject = SpaceBarnRootObject;
				break;
			case "StarSystemMenuItem":
				StarSystemRootObject.SetActive (true);
				for (int i = 0; i < StarSystemRootObject.transform.childCount; i++) {
					StarSystemRootObject.transform.GetChild (i).gameObject.SetActive (true);  
				}
				ActiveMenuRootObject.SetActive (false);
				ActiveMenuRootObject = StarSystemRootObject;
				if (firstActivation) {
					SetStarSystemText ();
					firstActivation = false;
				}
				break;
			case "OptionsQuitMenu":
				OptionsQuitRootObject.SetActive (true);
				for (int i = 0; i < OptionsQuitRootObject.transform.childCount; i++) {
					OptionsQuitRootObject.transform.GetChild (i).gameObject.SetActive (true);  
				}
				ActiveMenuRootObject.SetActive (false);
				ActiveMenuRootObject = OptionsQuitRootObject;
				break;
			case "MainMenuButton":
				MainMenuRootObject.SetActive (true);
				for (int i = 0; i < MainMenuRootObject.transform.childCount; i++) {
					MainMenuRootObject.transform.GetChild (i).gameObject.SetActive (true);  
				}
				ActiveMenuRootObject.SetActive (false);
				ActiveMenuRootObject = MainMenuRootObject;
				break;
			case "SpaceFruitMenu":
				FruitInventoryMenuRootObject.SetActive (true);
				DisplaySpaceFruitMenu ();
				ActiveMenuRootObject.SetActive (false);
				ActiveMenuRootObject = FruitInventoryMenuRootObject;
				break;
		case "ToolsGoodiesSeedsMenu":
				TGSInventoryMenuRootObject.SetActive (true);
				DisplayTGSMenu ();
				ActiveMenuRootObject.SetActive (false);
				ActiveMenuRootObject = TGSInventoryMenuRootObject;
				break;
		}

	}

	private void DisplaySpaceFruitMenu() {
		Dictionary<string, int> fruitInv = spaceBarn.fruitInventory;
		int i = 0;
		foreach (string fruitStr in fruitInv.Keys) {
			GameObject g = FruitInventoryMenuRootObject.transform.GetChild (i).gameObject;  
			g.SetActive (true);

			//Set the icon
			if(spaceBarn.FruiticonFilenameFromString(fruitStr) != null) {
				StartCoroutine (LoadSpriteUI (Path.Combine (Application.streamingAssetsPath, spaceBarn.FruiticonFilenameFromString (fruitStr)), g.GetComponent<Image> (), 32.0f));
				g.GetComponentInChildren<Text> ().text = "x" + fruitInv [fruitStr];
				g.GetComponent<ButtonDisplayText> ().description = fruitStr;
			}
			i++;
		}

	}

	private void DisplayTGSMenu() {
		Dictionary<string, int> TGSInv = spaceBarn.ToolsGoodiesSeeds;
		int i = 0;
		foreach(string tgsStr in TGSInv.Keys) {
			GameObject g = TGSInventoryMenuRootObject.transform.GetChild (i).gameObject;  
			if (TGSInv [tgsStr] > 0)
				g.SetActive (true);
			else {
				g.SetActive (false);
				i++;
				continue;
			}
			//Set the icon
			StartCoroutine(LoadSpriteUI(Path.Combine(Application.streamingAssetsPath, spaceBarn.TGSiconFileNameFromString(tgsStr)), g.GetComponent<Image> (), 32.0f));
			g.GetComponentInChildren<Text>().text = "x" + TGSInv[tgsStr];
			g.GetComponent<ButtonDisplayText>().description = tgsStr;
			i++;
		}


	}

	//http://answers.unity3d.com/questions/839515/how-to-load-sprite-from-streamingassets-instead-of.html
	public IEnumerator LoadSpriteUI(string absoluteImagePath, Image i, float pixelsPerUnit)
	{
		string finalPath;
		WWW localFile;
		Texture texture;

		finalPath = "file://" + absoluteImagePath;
		Debug.Log (finalPath);
		localFile = new WWW(finalPath);

		if (!string.IsNullOrEmpty (localFile.error)) {
			//Debug.LogError (localFile.error);
			yield break;
		} 
		else {
			yield return localFile;

			texture = localFile.texture;
			texture.filterMode = FilterMode.Point;
			i.overrideSprite = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f), pixelsPerUnit);
		}
	}

	private void SetStarSystemText() {
		Debug.Log ("In SetStarSystemText");
		string[] SystemNames = sm.ShuffledStarNameList ().ToArray();
		Debug.Log ("ShuffledStarNames array length: " + SystemNames.Length);
		int x = 0;
		for (int i = 0; i < ActiveMenuRootObject.transform.childCount; i++) {
			GameObject g = ActiveMenuRootObject.transform.GetChild (i).gameObject;
			if (g.GetComponent(typeof(Button))) {
				Debug.Log ("Found object with button");
				if (x < SystemNames.Length && g.tag == "StarSystemMenuItem") {
					g.GetComponentInChildren<Text>().text = SystemNames [x];
					(g.GetComponent(typeof(Button)) as Button).onClick.AddListener(delegate { SystemWarp(g.GetComponentInChildren<Text>().text); } );
					x++;
				}

			}

		}
	}

	public void ApplyFruit(string item) {

	}

	public void ApplyTGS(string item) {
		GameObject tgt = planetPointer.GetObjectPointedAt();

		if (tgt.GetComponent(typeof(Planter))) {
			UsableItem useItem = spaceBarn.TGSItemFromString (item);

			if (useItem.GetItemType () == "Plant") {
				//Debug.Log ("In plant branch");
				(tgt.GetComponent (typeof(Planter)) as Planter).PlantSeed (useItem as Plant);
				int seedCount = spaceBarn.RemovePlant (useItem as Plant);
				if (seedCount == 0) {
					DisplayTGSMenu ();
				}
			} else if (useItem.GetItemType () == "Goodie") {
				//Debug.Log ("In goodie branch");
				(tgt.GetComponent (typeof(Planter)) as Planter).ApplyGoodie (useItem as Goodie);
				int goodieCount = spaceBarn.RemoveGoodie (useItem as Goodie);
				if (goodieCount == 0) {
					DisplayTGSMenu ();
				}
			} else if (useItem.GetItemType () == "Tool") {

			}
		} else if (tgt.GetComponent(typeof(Planet))) {
			//TODO: Terraforming stuff here
		} else {
			//TODO: Put some default stuff here
		}
	}

	public void SystemWarp(string sys) {
		tc.MoveSpaceBarn (sys);
	}

	public void StartGame() {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("TitleCard"))
			g.SetActive (false);
	}

	public void Quit() {
		Application.Quit ();
	}



}
