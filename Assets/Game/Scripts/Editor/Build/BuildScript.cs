#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using System.Linq;

// http://jonathanpeppers.com/Blog/automating-unity3d-builds-with-fake
// And
// http://www.thegameengineer.com/blog/2013/07/23/unity-3-building-the-project/
public class BuildScript : MonoBehaviour {

	public static string productName = "GolfVR";
	public static string outputFolder = "bin/";
	//public static string outputFilenameIos = outputFolder + "Android/" + productName + ".TODO?";

	public static string versionNumber;
	public static string buildNumber;

	[MenuItem("Build/Build all")]
	public static void BuildAll()
	{
		BuildAndroid ();
		//BuildIos ();
		//BuildWindows ();
	}

	[MenuItem("Build/Build android")]
	public static void BuildAndroid()
	{
		Init();
		new BuildAndroid();
	}




	private static void Init()
	{
		versionNumber = Environment.GetEnvironmentVariable("VERSION_NUMBER");
		if (string.IsNullOrEmpty(versionNumber))
			versionNumber = "1.0.0.0";
		
		buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");
		if (string.IsNullOrEmpty(buildNumber))
			buildNumber = "1";
		
		PlayerSettings.productName = productName;
		PlayerSettings.bundleVersion = versionNumber;
	}

	public static void Build(BuildTarget target, string output){		
		var outputDirectory = output.Remove (output.LastIndexOf ("/"));
		if (!Directory.Exists(outputDirectory))
			Directory.CreateDirectory(outputDirectory);

		string[] level_list = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
		string scenesNames = "(";
		foreach( string s in level_list)
			scenesNames += s.Remove( s.IndexOf(".unity") ) + ", ";
		scenesNames = scenesNames.Remove (scenesNames.Length - 2) + ")";

		Debug.Log("Building Platform: " + target.ToString() );
		Debug.Log("Building Target: " + output);
		Debug.Log("Scenes Processed: " + level_list.Length );		
		Debug.Log("Scenes Names: " + scenesNames);

		string results = BuildPipeline.BuildPlayer( level_list, output, target, BuildOptions.None );		
		if ( results.Length == 0 )
			Debug.Log("No Build Errors" );
		else
			Debug.LogError("Build Error:" + results);
	}


/*
	[MenuItem("Build/Build windows (not tested)")]
	public static void BuildWindows()
	{
	}
*/
}

#endif
