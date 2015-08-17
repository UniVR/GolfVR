using System;
using System.Collections.Generic;
// using UnityEditor;
using UnityEngine;

/// <summary>
/// *** Texture physics ***
/// Detect the terrain under the ball and adapt the physic of the ball.
/// 
/// Based on this answer: How can I perform some action based on the terrain texture currently under my player?
/// http://answers.unity3d.com/questions/14998/how-can-i-perform-some-action-based-on-the-terrain.html
/// </summary>
[Serializable]
public class TexturePhysics{
	[SerializeField] public float drag;
	[SerializeField] public float angularDrag;
}

public class DetectTerrainType : MonoBehaviour {

	[SerializeField] public TexturePhysics[] TerrainTexturePhysics;
	[SerializeField] public float[] currentTextureLevel;

	private MainScript mainScript;

	//The ball
	private Transform ballTransf;
	private Rigidbody ballRigidBody;

	//Terrain properties
	private Terrain currentTerrain;
	private int textureSize;
	private TerrainData terrainData;
	private Vector3 terrainPos;

	private float coefX;
	private float coefZ;

	void Start () {
		mainScript = GetComponent<MainScript> ();

		ballTransf = mainScript.Ball.transform;
		ballRigidBody = mainScript.Ball.GetComponent<Rigidbody>();

		InitTerrainData ();
	}

	public void SetBallDrag(){
		InitTerrainData ();

		var ballPos = ballTransf.position;
		var mapX = (ballPos.x - terrainPos.x) * coefX;
		var mapZ = (ballPos.z - terrainPos.z) * coefZ;
		var splatmapData = terrainData.GetAlphamaps((int)mapX, (int)mapZ, 1, 1); 
		
		float drag = 0;
		float angularDrag = 0;
		
		for (int i=0; i<textureSize; i++) {
			currentTextureLevel[i] = splatmapData[0,0,i];
			drag += TerrainTexturePhysics[i].drag * currentTextureLevel[i];
			angularDrag += TerrainTexturePhysics[i].angularDrag * currentTextureLevel[i];
		}
		
		ballRigidBody.drag = drag;
		ballRigidBody.angularDrag = angularDrag;
	}

	public bool IsOutOfBound(){
		if (currentTextureLevel [textureSize - 1] > 0.1) 
			return true;
		return false;
	}

	private void InitTerrainData(){
		Terrain terrain = mainScript.GetCurrentHole().Terrain;
		if (terrain != currentTerrain) {
			terrainData = terrain.terrainData;
			terrainPos = terrain.transform.position;
		
			textureSize = TerrainTexturePhysics.Length;
			currentTextureLevel = new float[textureSize];
		
			coefX = terrainData.alphamapWidth / terrainData.size.x;
			coefZ = terrainData.alphamapHeight / terrainData.size.z;
		}
	}

}
