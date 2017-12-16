using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goodie : MonoBehaviour, UsableItem {

	public string name;
	public int fertilizerPoints;
	public int moisturePoints;

	public Sprite goodieIcon;
	public string goodieIconFileName;

	public string GetItemName() {
		return name;
	}

	public Sprite GetIcon() {
		return goodieIcon;
	}

	public string GetItemType() {
		return "Goodie";
	}

	public void LoadFromGoodieData(GoodieData gdro) {
		name = gdro.name;
		fertilizerPoints = gdro.fertilizerPoints;
		moisturePoints = gdro.moisturePoints;
		goodieIconFileName = gdro.goodieIconFileName;
		Debug.Log ("Goodie Icon File Name: " + goodieIconFileName);
		#if UNITY_STANDALONE
		StartCoroutine(LoadSprite(Path.Combine(Application.streamingAssetsPath, goodieIconFileName), goodieIcon, 32.0f));
		#elif UNITY_ANDROID || UNITY_IOS
		switch(name) {
			case "Standard Space Fertilizer":
			break;
			case "Centauri Spring Water Ration":
			break;
		}
		#endif
	}

	public string GetIconFileName() {
		return goodieIconFileName;
	}

	//http://answers.unity3d.com/questions/839515/how-to-load-sprite-from-streamingassets-instead-of.html
	public IEnumerator LoadSprite(string absoluteImagePath, Sprite s, float pixelsPerUnit)
	{
		Debug.Log (absoluteImagePath);
		string finalPath;
		WWW localFile;
		Texture2D texture;

		finalPath = "file://" + absoluteImagePath;
		Debug.Log (finalPath);
		localFile = new WWW(finalPath);

		if (!string.IsNullOrEmpty (localFile.error)) {
			Debug.LogError (localFile.error);
			yield break;
		} 
		else {
			yield return localFile;

			texture = localFile.texture;
			s = Sprite.Create (texture as Texture2D, new Rect (0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f), pixelsPerUnit);
		}
	}
/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
*/
}
