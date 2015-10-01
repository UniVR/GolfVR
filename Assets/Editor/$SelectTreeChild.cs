using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Collections.Generic;

public class SelectTreeChild {

	[MenuItem ("Terrain/Select TreeChild")]
	public static void SelectTreeChilds () {
		GameObject[] objs = Selection.gameObjects;
		List<GameObject> childrens = new List<GameObject>();
		foreach (GameObject obj in objs) {
			if (obj.gameObject.tag == "TreeChild") {
				childrens.Add(obj.transform.gameObject);
			}
		}
		Selection.objects = childrens.ToArray();
	}
}