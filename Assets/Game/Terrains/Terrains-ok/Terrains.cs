using UnityEngine;
using System.Collections;

public class Terrains : MonoBehaviour {

	public Terrain[] terrains;

	// Use this for initialization
	void Start () {
		terrains [0].SetNeighbors (null, terrains [3], terrains [1], null);
		terrains [1].SetNeighbors (terrains [0], terrains [4], terrains [2], null);
		terrains [2].SetNeighbors (terrains [1], terrains [5], null, null);
		terrains [3].SetNeighbors (null, terrains [6], terrains [4], terrains [0]);
		terrains [4].SetNeighbors (terrains [3], terrains [7], terrains [5], terrains [1]);
		terrains [5].SetNeighbors (terrains [4], terrains [8], null, terrains [2]);
		terrains [6].SetNeighbors (null, null, terrains [7], terrains [3]);
		terrains [7].SetNeighbors (terrains [6], null, terrains [8], terrains [4]);
		terrains [8].SetNeighbors (terrains [7], null, null, terrains [5]);
		
		for (int cnt=0; cnt < terrains.Length; cnt++) {
			terrains[cnt].Flush();
		}
	}
}



// http://getyour411.com/wordpress/2014/02/using-terrains-and-setneighbors/ //