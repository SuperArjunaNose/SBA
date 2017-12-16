using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HostileMeaty : MonoBehaviour {

	public int hp;
	public StarSystem currentStarSystem;

	public abstract void SelectNextStarSystem ();
	protected abstract void ApproachPlanet();

}
