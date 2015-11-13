#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Linq;

public class Android : EditorWindow {

	private static string KeystorePath;
	private static string Username;
	private static string Password;

	public static void Init() {
		EditorWindow window = GetWindow(typeof(Android), true, "Android settings");

		KeystorePath = PlayerSettings.Android.keystoreName;
		Username = PlayerSettings.Android.keyaliasName;
		Password = PlayerSettings.Android.keyaliasPass;

		window.position = new Rect(Screen.width/2, Screen.height/2, 400, 200);
		window.Show();
	}

	void OnGUI() {
		GUILayout.Label ("Key store:", EditorStyles.boldLabel);

		KeystorePath = EditorGUILayout.TextField ("Keystore name", KeystorePath);
		Username = EditorGUILayout.TextField ("Username", Username);
		Password = EditorGUILayout.PasswordField("Password", Password);

		if (GUILayout.Button ("Validate")) {
			int versionCode;
			versionCode = int.Parse(Build.buildNumber);
			PlayerSettings.Android.bundleVersionCode = versionCode;
			PlayerSettings.Android.keyaliasName = Username;
			PlayerSettings.Android.keyaliasPass = PlayerSettings.Android.keystorePass = Password;
			PlayerSettings.Android.keystoreName = Path.GetFullPath(KeystorePath).Replace('\\', '/');
			BuildPipeline.BuildPlayer(Build.GetScenes(), Build.outputFilenameAndroid, BuildTarget.Android, BuildOptions.None);
		}
	}
}

#endif