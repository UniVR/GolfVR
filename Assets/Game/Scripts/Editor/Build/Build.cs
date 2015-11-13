#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection;

using System.Linq;

// http://jonathanpeppers.com/Blog/automating-unity3d-builds-with-fake
public class Build : MonoBehaviour {

	//private static string[] Levels = new string[] {"Assets/MainMenu.unity", "Assets/GolfVR.unity", "Assets/Exit.unity"};

	public static string productName = "GolfVR";
	public static string outputFolder = "bin/";
	public static string outputFilenameAndroid = outputFolder + productName + ".apk";
	public static string outputFilenameIos = "";

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
		InitVariables ();
		Android.Init ();
	}



	public static string[] GetScenes()
	{
		return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
	}



	private static void InitVariables()
	{
		CheckDir (outputFolder);

		versionNumber = Environment.GetEnvironmentVariable("VERSION_NUMBER");
		if (string.IsNullOrEmpty(versionNumber))
			versionNumber = "1.0.0.0";
		
		buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");
		if (string.IsNullOrEmpty(buildNumber))
			buildNumber = "1";
		
		PlayerSettings.productName = productName;
		PlayerSettings.bundleVersion = /*PlayerSettings.shortBundleVersion =*/ versionNumber;
	}
	
	private static void CheckDir(string dir)
	{
		if (!Directory.Exists(dir))
			Directory.CreateDirectory(dir);
	}


/*
	[MenuItem("Build/Build iOs (not tested)")]
	public static void BuildIos()
	{
		CheckDir ("Scratch/Xcode");
		PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
		PlayerSettings.SetPropertyInt("Architecture", (int)iPhoneArchitecture.Universal, BuildTargetGroup.iOS);
		BuildPipeline.BuildPlayer(GetScenes(), "Scratch/Xcode", BuildTarget.iOS, BuildOptions.None);

		//TODO: Build Xcode project
		//let Exec command args =
    	//	let result = Shell.Exec(command, args)
    	//	if result <> 0 then failwithf "%s exited with error %d" command result
		//let Xcode args =
    	//	Exec "xcodebuild" args
	}

	[MenuItem("Build/Build iOs64 (IL2CPP) (not tested)")]
	public static void BuildIos64()
	{
		CheckDir("Scratch/Xcode");
		PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
		PlayerSettings.SetPropertyInt("Architecture", (int)iPhoneArchitecture.Universal, BuildTargetGroup.iOS);
		BuildPipeline.BuildPlayer(GetScenes(), "Scratch/Xcode", BuildTarget.iOS, BuildOptions.None);
	}
	
	[MenuItem("Build/Build windows (not tested)")]
	public static void BuildWindows()
	{
	}
*/
}

#endif
