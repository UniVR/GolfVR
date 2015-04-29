using System;
using System.Collections.Generic;
using UnityEditor;
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

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (Transform))]
public class DetectTerrainType : MonoBehaviour {
	
	private int textureSize;

	[Tooltip("Physics of each layer on the terrain")]
	[SerializeField] public TexturePhysics[] TerrainTexturePhysics;
	
	[Tooltip("Visualize the current texture under the object (auto generated values)")]
	[SerializeField] public float[] currentTextureLevel;


	private Transform ballTransf;
	private Rigidbody ballRigidBody;

	private TerrainData terrainData;
	private Vector3 terrainPos;

	private Vector3 OldPosition;

	void Start () {
		ballTransf = GetComponent<Transform>();
		ballRigidBody = GetComponent<Rigidbody> ();

		Terrain terrain = Terrain.activeTerrain;
		terrainData = terrain.terrainData;
		terrainPos = terrain.transform.position;
		
		textureSize = TerrainTexturePhysics.Length;
		currentTextureLevel = new float[textureSize];
	}

	void Update () {
		var ballPos = ballTransf.position;
		var mapX = ((ballPos.x - terrainPos.x) / terrainData.size.x) *  terrainData.alphamapWidth;
		var mapZ = ((ballPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight;
		var splatmapData = terrainData.GetAlphamaps((int)mapX, (int)mapZ, 1, 1); 

		float drag = 0;
		float angularDrag = 0;

		for (int i=0; i<textureSize; i++) {
			currentTextureLevel[i] = splatmapData[0,0,i];
			drag += TerrainTexturePhysics[i].drag * currentTextureLevel[i];
			angularDrag += TerrainTexturePhysics[i].angularDrag * currentTextureLevel[i];
		}

		if (currentTextureLevel [textureSize - 1] > 0.1) { //Out of bound
			ballTransf.position = OldPosition;
			ballRigidBody.velocity = Vector3.zero;
		} else if (ballRigidBody.velocity.magnitude < 0.1) { //Save old position
			OldPosition = ballPos;
		}

		ballRigidBody.drag = drag;
		ballRigidBody.angularDrag = angularDrag;
	}
}
