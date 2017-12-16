using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDisplayText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

	public string description;
	[SerializeField] private GameObject textDisplayObject;
	private Text textDisplay;
	private UIController uic;

	public void OnPointerEnter (PointerEventData eventData) {
		textDisplay.text = description;

	}

	public void ApplyItemClickedTGS() {
		uic.ApplyTGS(description);
		textDisplay.text = "";
	}

	public void ApplyItemClickedFruit() {
		uic.ApplyFruit (description);
		textDisplay.text = "";
	}


	public void OnPointerExit (PointerEventData eventData)
	{
		textDisplay.text = "";
	}

	void Start() {
		textDisplay = textDisplayObject.GetComponent<Text> ();
		uic = (GameObject.FindGameObjectWithTag ("UIController").GetComponent (typeof(UIController)) as UIController);
	}
}
