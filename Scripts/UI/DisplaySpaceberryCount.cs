using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpaceberryCount : MonoBehaviour {

	private SpaceBarn sBarn;
	private Text txt;

	// Use this for initialization
	void Start () {
		sBarn = GameObject.FindGameObjectWithTag ("SpaceBarn").GetComponent<SpaceBarn>();
		txt = GetComponent<Text> ();
		txt.text =  "SpaceBerries:\n" + sBarn.spaceBerries;
	}

	void OnEnable() {
		txt.text = "SpaceBerries:\n" + sBarn.spaceBerries;
	}

}
