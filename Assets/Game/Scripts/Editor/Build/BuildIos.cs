#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Linq;

public class BuildIos : EditorWindow {
	
	public static string outputFilenameAndroidCardboard = BuildScript.outputFolder + "Ios/" + BuildScript.productName + ".apk";
	public static string outputFilenameAndroidGearVR = BuildScript.outputFolder + "Ios/" + BuildScript.productName + "GearVR.apk";
	
	public BuildIos() {
		EditorWindow window = GetWindow(typeof(BuildAndroid), true, "Ios settings");

		//TODO

		window.position = new Rect(Screen.width/2, Screen.height/2, 400, 100);
		window.Show();
	}
	
	void OnGUI() {
		//TODO
	}

	/*
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
	
	// IL2CPP
	public static void BuildIos64()
	{
		CheckDir("Scratch/Xcode");
		PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
		PlayerSettings.SetPropertyInt("Architecture", (int)iPhoneArchitecture.Universal, BuildTargetGroup.iOS);
		BuildPipeline.BuildPlayer(GetScenes(), "Scratch/Xcode", BuildTarget.iOS, BuildOptions.None);
	}
	*/
}

#endif