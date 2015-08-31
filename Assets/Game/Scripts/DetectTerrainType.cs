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

	public float Debug_CurrentDrag;
	public float Debug_CurrentAngularDrag;

	[SerializeField] public TexturePhysics[] TerrainTexturePhysics;
	[SerializeField] public float[] currentTextureLevel;

	//Terrain properties
	private int textureSize;
	private TerrainData terrainData;
	private Vector3 terrainPos;

	private float coefX;
	private float coefZ;

	public void SetBallDrag(Terrain terrain, Vector3 ballPos, Rigidbody ball, float addedDrag = 0f){
		try{
		InitTerrainData (terrain);
		var splatmapData = getSplatMapData (ballPos);		
		float drag = addedDrag;
		float angularDrag = addedDrag;
		
		for (int i=0; i<textureSize; i++) {
			currentTextureLevel[i] = splatmapData[0,0,i];
			drag += TerrainTexturePhysics[i].drag * currentTextureLevel[i];
			angularDrag += TerrainTexturePhysics[i].angularDrag * currentTextureLevel[i];
		}
		
		Debug_CurrentDrag = ball.drag = drag;
		Debug_CurrentAngularDrag = ball.angularDrag = angularDrag;
		}catch(Exception e){
			Debug.Log("Ball out of the current terrain");
		}
	}

	public bool IsOutOfBound(Terrain terrain, Vector3 ballPos){
		try{
			InitTerrainData (terrain);
			var splatmapData = getSplatMapData (ballPos);	
			if (splatmapData[0, 0, textureSize - 1] > 0.1) 
				return true;
			return false;
		}
		catch(Exception e){
			return true;
		}
	}

	private float[,,] getSplatMapData(Vector3 pos){
		var mapX = (pos.x - terrainPos.x) * coefX;
		var mapZ = (pos.z - terrainPos.z) * coefZ;
		return terrainData.GetAlphamaps((int)mapX, (int)mapZ, 1, 1); 
	}

	private void InitTerrainData(Terrain terrain){
		terrainData = terrain.terrainData;
		terrainPos = terrain.transform.position;
	
		textureSize = TerrainTexturePhysics.Length;
		currentTextureLevel = new float[textureSize];
	
		coefX = terrainData.alphamapWidth / terrainData.size.x;
		coefZ = terrainData.alphamapHeight / terrainData.size.z;
	}

}
