using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPointer : MonoBehaviour {

	public float rayCastLength = 2.5f;

	[SerializeField] private GameObject panelTextObject;
	private GameObject g;

	public GameObject GetObjectPointedAt() {
		return g;
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down);

		if (hit.collider != null) {
			if ((hit.collider.gameObject.GetComponent (typeof(PlanetAttachable)) as PlanetAttachable) != null)
				panelTextObject.GetComponent<Text> ().text = (hit.collider.gameObject.GetComponent (typeof(PlanetAttachable)) as PlanetAttachable).DumpInfo ();
			else if (hit.collider.tag == "Planet") {
				panelTextObject.GetComponent<Text> ().text = hit.collider.gameObject.GetComponent<Planet> ().planetName;
			}
		}
		else {
			//Debug.Log ("Didn't hit nuthin'");
		}
		g = hit.collider.gameObject;
	}
}
