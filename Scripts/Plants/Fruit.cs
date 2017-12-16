using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Fruit : MonoBehaviour, UsableItem {

	public Sprite fruitIcon;
	public string fruitName;
	private string fileName;

	public void LoadFruitFromPlantData(string fruitNameStr, string fruitFileName, float pixelsPerUnit) {
		fruitName = fruitNameStr;
		fileName = fruitFileName;
		StartCoroutine (LoadSprite (Path.Combine(Application.streamingAssetsPath, fruitFileName), fruitIcon, pixelsPerUnit));
	}

	public string GetIconFileName() {
		return fileName;
	}

	public string GetItemName() {
		return fruitName;
	}

	public Sprite GetIcon() {
		return fruitIcon;
	}

	public string GetItemType() {
		return "Fruit";
	}

	//http://answers.unity3d.com/questions/839515/how-to-load-sprite-from-streamingassets-instead-of.html
	public IEnumerator LoadSprite(string absoluteImagePath, Sprite s, float pixelsPerUnit)
	{
		string finalPath;
		WWW localFile;
		Texture texture;

		finalPath = "file://" + absoluteImagePath;
		localFile = new WWW(finalPath);
		Debug.Log (finalPath);
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
