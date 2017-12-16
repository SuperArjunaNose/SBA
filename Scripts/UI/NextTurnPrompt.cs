using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTurnPrompt : MonoBehaviour {

	private Text txt;
	private TurnController tc;

	void Awake() {
		Messenger.AddListener (GameEvent.TURN_ADVANCED, ResetText);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.TURN_ADVANCED, ResetText);
	}

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text> ();
		tc = GameObject.FindGameObjectWithTag ("SimulationManager").GetComponent<TurnController> ();
	}

	void ResetText() {
		txt.text = "Next Turn";
	}


}
