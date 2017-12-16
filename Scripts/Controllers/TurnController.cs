using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

/*
 * Handles changes in the simulation before and after the user clicks Next Turn in the menu.
 * Data classes are the Model, and this is the ViewController.
 * 
 **/
public class TurnController : MonoBehaviour {
	
	public Vector3 centerPosition;
	public Vector3 leftPosition;
	public Vector3 rightPosition;
	public Vector3 sunPosition;

	private Transform centerPlanet;
	private Transform leftPlanet;
	private Transform rightPlanet;
	private Vector3 newCameraPosition;

	public List<StarData> starSystems;
	public StarData presentSystem;

	public Transform starPrefab;
	public Transform planetPrefab;
	public Transform shadowPrefab;

	public float scalePercentage;
	public float starSpritePixelsPerUnit;
	public float planetPixelsPerUnit;
	public float movementCoefficient=5.0f;
	public float planetOffset=6.0f;

//	public Sprite inhabitedPlanetSprite;
//	public Sprite terraformablePlanetSprite;
//	public Sprite dudPlanetSprite;
//	public Sprite farmablePlanetSprite;

	#if UNITY_ANDROID || UNITY_IOS
	[SerializeField] private Sprite inhabitedPlanet;
	[SerializeField] private Sprite farmablePlanet;
	[SerializeField] private Sprite terraformablePlanet;
	[SerializeField] private Sprite dudPlanet;
	[SerializeField] private Sprite shdw;
	#endif


	private StarSystem sSystem;

//	void Awake() {
//		LoadPlanetSprites ();  //TODO: Do I need to worry about using sprites before they're fully loaded from disk?  I'll probably find out.
//	}

	// Use this for initialization
	void Start () {
		//starSystems = GetComponent<SimulationManager> ().starSystems;
		presentSystem = GetComponent<SimulationManager> ().startingSystem;
		DrawStarSystem (presentSystem);
		newCameraPosition = Camera.main.transform.position;

	}

	/**
	 * Given the star system that the space barn is currently in, draw the stars and planets.  Instantiate them as Unity objects if they haven't
	 * been already.
	 */
	public void DrawStarSystem(StarData currentSystem) {
		if (true) { //TODO: Replace this with a check to see if currentSystem has been instantiated
			int placeCounter=1;
			//Instantiate and position the star
			GameObject starSystemObject = Instantiate(starPrefab, sunPosition, Quaternion.identity).gameObject;
			starSystemObject.GetComponent<StarSystem> ().LoadStarData (currentSystem, starSpritePixelsPerUnit);
			List<GameObject> planetShadows = new List<GameObject>();
			//Get the StarData component and copy over relevant attributes
			sSystem = starSystemObject.GetComponent<StarSystem> ();




			//Iterate through the StarData's planets list and start instantiating planets.

			//First instantiate the planets
			foreach(PlanetData pData in currentSystem.planets) {
				Transform planetTransform = Instantiate(planetPrefab, new Vector3(2f, 10f, 7f), Quaternion.identity);
				Transform planetShadowTransform = Instantiate(shadowPrefab, new Vector3(2f, 10f, 7f), Quaternion.identity);
				//planetTransform.Translate(new Vector3 (100f, 100f, 100f));

				GameObject planetObject = planetTransform.gameObject;
				GameObject planetShadowObject = planetShadowTransform.gameObject;

				Planet p = planetObject.GetComponentInChildren<Planet> ();
				//SpriteRenderer psr = p.GetComponent<SpriteRenderer> ();
				p.isFarmable = pData.isFarmable;
				p.isInhabited = pData.isInhabited;
				p.isTerraformable = pData.isTerraformable;
				p.ownedByPlayer = pData.ownedByPlayer;
				p.planetName = pData.systemAndPlace;
				//p.placeInSystem = placeCounter;
				//placeCounter++;
				//Stopping here.  Need some way to move around planets list
				//probs arising with parallel arrays and added data stuff
				//TODO: THERE IS A LIST OF GROWIES ON THE PLANETDATA OBJECT, BUT WE'RE NOT DEALING WITH THAT YET
				#if UNITY_STANDALONE
				if (p.isFarmable) {
					StartCoroutine (LoadSprite (Path.Combine(Application.streamingAssetsPath, "farmable2.png"), planetObject.GetComponentInChildren<SpriteRenderer>(), planetPixelsPerUnit));
					if (p.ownedByPlayer) {
						for (int i = 0; i < planetTransform.transform.childCount; i++)
							planetTransform.GetChild (i).gameObject.SetActive (true);
					}
				} else if (p.isInhabited) {
					StartCoroutine (LoadSprite (Path.Combine(Application.streamingAssetsPath, "inhabited2.png"), planetObject.GetComponentInChildren<SpriteRenderer>(), planetPixelsPerUnit));
				} else if (p.isTerraformable) {
					StartCoroutine (LoadSprite (Path.Combine(Application.streamingAssetsPath, "terraformable2.png"), planetObject.GetComponentInChildren<SpriteRenderer>(), planetPixelsPerUnit));
				} else {
					StartCoroutine (LoadSprite (Path.Combine(Application.streamingAssetsPath, "dud2.png"), planetObject.GetComponentInChildren<SpriteRenderer>(), planetPixelsPerUnit));
				}

				sSystem.planets.Add (planetObject);
				StartCoroutine(LoadSprite (Path.Combine(Application.streamingAssetsPath, "shadow.png"), planetShadowObject.GetComponentInChildren<SpriteRenderer>(), planetPixelsPerUnit));

				#elif UNITY_ANDROID || UNITY_IOS
					if (p.isFarmable) {
						planetObject.GetComponentInChildren<SpriteRenderer>().sprite = farmablePlanet;
						if (p.ownedByPlayer) {
							for (int i = 0; i < planetTransform.transform.childCount; i++)
								planetTransform.GetChild (i).gameObject.SetActive (true);
						}
					} else if (p.isInhabited) {
						planetObject.GetComponentInChildren<SpriteRenderer>().sprite = inhabitedPlanet;
					} else if (p.isTerraformable) {
						planetObject.GetComponentInChildren<SpriteRenderer>().sprite = terraformablePlanet;
					} else {
						planetObject.GetComponentInChildren<SpriteRenderer>().sprite = dudPlanet;
					}

					sSystem.planets.Add (planetObject);
					planetShadowObject.GetComponentInChildren<SpriteRenderer>().sprite = shdw;

				#endif
				planetShadows.Add (planetShadowObject);
			}

			//sSystem.planets[0].transform.position = centerPosition;
			//planetShadows [0].transform.position = new Vector3 (centerPosition.x, centerPosition.y, centerPosition.z - .01f);
			centerPlanet = sSystem.planets [0].transform;
			for (int i = 0; i < sSystem.planets.Count; i++) {
				sSystem.planets [i].transform.position = new Vector3 (centerPosition.x+(float)(planetOffset*i), centerPosition.y, centerPosition.z);
				planetShadows[i].transform.position = new Vector3 (centerPosition.x+(float)(planetOffset*i), centerPosition.y, centerPosition.z-.01f);
				planetShadows [i].transform.localScale *= 1.075f;
				//sSystem.planets [i].transform.localScale *= scalePercentage;
			}
			rightPlanet = sSystem.planets [1].transform;
			//sSystem.planets[1].transform.position = rightPosition;
			//Debug.Log((float)Screen.width / (float)Screen.height);
		} else {
		}
	}




	#if UNITY_STANDALONE
	//http://answers.unity3d.com/questions/839515/how-to-load-sprite-from-streamingassets-instead-of.html
	public static IEnumerator LoadSprite(string absoluteImagePath, SpriteRenderer sr, float pixelsPerUnit)
	{
		string finalPath;
		WWW localFile;
		Texture texture;
		Sprite sprite;

		finalPath = "file://" + absoluteImagePath;
		localFile = new WWW(finalPath);
		//Debug.Log (finalPath);
		if (!string.IsNullOrEmpty (localFile.error)) {
			Debug.LogError (localFile.error);
			yield break;
		} 
		else {
			yield return localFile;

			texture = localFile.texture;
			texture.filterMode = FilterMode.Point;
			sprite = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f), pixelsPerUnit);
			//sprite = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), Vector2.zero, pixelsPerUnit);
			//sprite.pivot = new Vector2(32,32);
			//sprite.pivot = new Vector2(0,0);
			sr.sprite = sprite;
		}
	}

	#elif UNITY_ANDROID || UNITY_IOS
	public static IEnumerator LoadSprite(string absoluteImagePath, SpriteRenderer sr, float pixelsPerUnit)
	{
		string finalPath;
		WWW localFile;
		Texture texture;
		Sprite sprite;

		finalPath = "file://" + absoluteImagePath;
		localFile = new WWW(finalPath);

		if (!string.IsNullOrEmpty (localFile.error)) {
			Debug.LogError (localFile.error);
			yield break;
		} 
		else {
			yield return localFile;

			texture = localFile.texture;
			texture.filterMode = FilterMode.Point;
			sprite = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f), pixelsPerUnit);
			//sprite = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), Vector2.zero, pixelsPerUnit);
			//sprite.pivot = new Vector2(32,32);
			//sprite.pivot = new Vector2(0,0);
			sr.sprite = sprite;
		}
	}

	#endif
	//http://answers.unity3d.com/questions/839515/how-to-load-sprite-from-streamingassets-instead-of.html
	public static IEnumerator LoadSprite(string absoluteImagePath, Sprite s, float pixelsPerUnit)
	{
				string finalPath;
				WWW localFile;
				Texture texture;

				finalPath = "file://" + absoluteImagePath;
				localFile = new WWW(finalPath);
				//Debug.Log (finalPath);
				if (!string.IsNullOrEmpty (localFile.error)) {
					Debug.LogError (localFile.error);
					yield break;
				} 
				else {
					yield return localFile;

					texture = localFile.texture;
					texture.filterMode = FilterMode.Point;
					s = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f), pixelsPerUnit);
				}
	}

//	
//	// Update is called once per frame
	void Update () {
		int nxt;

		//lmbReleased = Input.GetMouseButtonUp (0);
		#if UNITY_STANDALONE
			if (Input.GetMouseButton(0)) {	

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hit = Physics2D.GetRayIntersection (ray, Mathf.Infinity);

				if (hit.collider != null) {/*  hit.collider.transform == thisTransform)*/
					if (hit.collider.tag == "Planet" && !EventSystem.current.IsPointerOverGameObject()) { //TEST FOR THIS: Will I accidentally click the planet when clicking UI buttons?
						//Debug.Log("Collider Layer: " + hit.collider.gameObject.layer);
						if (Mathf.Abs(Camera.main.transform.position.x - hit.collider.transform.position.x) < 2.0f) {
							//Debug.Log ("In rotate if");
							//http://answers.unity3d.com/questions/413110/how-to-rotate-a-game-object-towards-the-mouse-clic.html
							Vector3 mousePos = Input.mousePosition;
							mousePos.z = 9.0f; //The distance from the camera to the player object
							Vector3 lookPos = Camera.main.ScreenToWorldPoint (mousePos);
							lookPos = lookPos - hit.collider.transform.position;
							float angle = Mathf.Atan2 (lookPos.y, lookPos.x) * Mathf.Rad2Deg;
							hit.collider.transform.localRotation = Quaternion.AngleAxis (angle, Vector3.forward);
						} else {
							//Debug.Log ("In Lerp else");
							newCameraPosition = new Vector3(hit.collider.transform.position.x + (Mathf.Abs(centerPosition.x)), Camera.main.transform.position.y, Camera.main.transform.position.z);
						}
					}
					//UI code here?
				}


				//lmbReleased = false;

			} else {
				Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, newCameraPosition, movementCoefficient * Time.deltaTime);
			}
		#elif UNITY_IOS || UNITY_ANDROID 
			if (Input.touchCount > 0) {	

				Touch newTouch = Input.touches[0];

				Ray ray = Camera.main.ScreenPointToRay (newTouch.position);
				RaycastHit2D hit = Physics2D.GetRayIntersection (ray, Mathf.Infinity);

				if (hit.collider != null) {/*  hit.collider.transform == thisTransform)*/
					if (hit.collider.tag == "Planet" && !EventSystem.current.IsPointerOverGameObject()) { //TEST FOR THIS: Will I accidentally click the planet when clicking UI buttons?
						Debug.Log("Collider Layer: " + hit.collider.gameObject.layer);
						if (Mathf.Abs(Camera.main.transform.position.x - hit.collider.transform.position.x) < 2.0f) {
							Debug.Log ("In rotate if");
							//http://answers.unity3d.com/questions/413110/how-to-rotate-a-game-object-towards-the-mouse-clic.html
							Vector3 mousePos = Input.mousePosition;
							mousePos.z = 9.0f; //The distance from the camera to the player object
							Vector3 lookPos = Camera.main.ScreenToWorldPoint (mousePos);
							lookPos = lookPos - hit.collider.transform.position;
							float angle = Mathf.Atan2 (lookPos.y, lookPos.x) * Mathf.Rad2Deg;
							hit.collider.transform.localRotation = Quaternion.AngleAxis (angle, Vector3.forward);
						} else {
							Debug.Log ("In Lerp else");
							newCameraPosition = new Vector3(hit.collider.transform.position.x + (Mathf.Abs(centerPosition.x)), Camera.main.transform.position.y, Camera.main.transform.position.z);

						}
					}
					//UI code here?
				}


				//lmbReleased = false;

			} else {
				Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, newCameraPosition, movementCoefficient * Time.deltaTime);
			}
		#endif
	}

	public void MoveSpaceBarn(string dest_system) {
		Debug.Log ("In MoveSpaceBarn with dest_system: " + dest_system);
	}

	public void AdvanceTurn() {
		//Broadcast the ADVANCE_TURN event
		Messenger.Broadcast(GameEvent.TURN_ADVANCED);
//		if (seedPlanted)
//			seedPlanted = false;
//		if (courseSet)
//			courseSet = false;
	}



}
