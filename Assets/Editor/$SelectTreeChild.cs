using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Collections.Generic;

public class SelectTreeChild {

	[MenuItem ("Terrain/Select TreeChild")]
	public static void DoSelectTreeChild () {
		GameObject[] objs = Selection.gameObjects;
		List<GameObject> childrens = new List<GameObject>();
		foreach (GameObject obj in objs) {
			if (obj.gameObject.name == "Tree2") {
				childrens.Add(obj.transform.gameObject);
			}
		}
		Selection.objects = childrens.ToArray();
	}
}