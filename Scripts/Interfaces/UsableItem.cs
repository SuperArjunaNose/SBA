using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UsableItem {

	string GetItemName();
	string GetIconFileName();
	Sprite GetIcon();
	string GetItemType();
}
