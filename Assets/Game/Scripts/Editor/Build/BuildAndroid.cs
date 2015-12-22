#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Linq;

public class BuildAndroid : EditorWindow {

	public static string outputFilenameAndroidCardboard = BuildScript.outputFolder + "Android/" + BuildScript.productName + ".apk";
	public static string outputFilenameAndroidGearVR = BuildScript.outputFolder + "Android/" + BuildScript.productName + "GearVR.apk";

	private static string KeystorePath;
	private static string Username;
	private static string Password;

	public BuildAndroid() {
		EditorWindow window = GetWindow(typeof(BuildAndroid), true, "Android settings");

		KeystorePath = PlayerSettings.Android.keystoreName;
		Username = PlayerSettings.Android.keyaliasName;
		Password = PlayerSettings.Android.keyaliasPass;

		window.position = new Rect(Screen.width/2, Screen.height/2, 400, 100);
		window.Show();
	}

	void OnGUI() {
		GUILayout.Label ("Key store:", EditorStyles.boldLabel);

		KeystorePath = EditorGUILayout.TextField ("Keystore name", KeystorePath);
		Username = EditorGUILayout.TextField ("Username", Username);
		Password = EditorGUILayout.PasswordField("Password", Password);

		if (GUILayout.Button ("Validate")) {
			int versionCode;
			versionCode = int.Parse(BuildScript.buildNumber);
			PlayerSettings.Android.bundleVersionCode = versionCode;
			PlayerSettings.Android.keyaliasName = Username;
			PlayerSettings.Android.keyaliasPass = PlayerSettings.Android.keystorePass = Password;
			PlayerSettings.Android.keystoreName = Path.GetFullPath(KeystorePath).Replace('\\', '/');

			BuildAndroidCardboard();
			//TODO
			//BuildAndroidGearVR();
		}
	}

	private void BuildAndroidCardboard(){
		BuildScript.Build(BuildTarget.Android, outputFilenameAndroidCardboard);
	}

	private void BuildAndroidGearVR(){
		BuildScript.Build(BuildTarget.Android, outputFilenameAndroidGearVR);
	}
}

#endif