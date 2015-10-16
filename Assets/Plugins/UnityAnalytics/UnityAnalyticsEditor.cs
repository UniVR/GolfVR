#if UNITY_EDITOR && !(UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5_0 || UNITY_5_1)
using UnityEngine;
using UnityEditor;
using System.Collections;
namespace UnityEngine.Cloud.Analytics
{
	[InitializeOnLoad]
	public class UnityAnalyticsVersionChecker {
		#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0  || UNITY_4_1)
		static UnityAnalyticsVersionChecker(){	
				Debug.LogError("Unity Analytics SDK doesn't support versions less than 4.2");
		}
		#else
		static UnityAnalyticsVersionChecker(){	
			// Defer calling Runonce until the first editor update is called, we do this so that Application.isPlaying gets the correct value
			EditorApplication.update += RunOnce; 
		}
		static void RunOnce(){
			// Only show upgrade popup when project is opened, not when the app is playing. This will also show popup everytime the project recompiles.
			if(!Application.isPlaying)
				EditorWindow.GetWindowWithRect<UnityAnalyticsSDKUpgradeWindow> (new Rect (300, 300, 380, 130), true, "Unity Analytics SDK");
			EditorApplication.update -= RunOnce;
		}
		#endif
	}

	public class UnityAnalyticsSDKUpgradeWindow : EditorWindow {
		private GUIContent upgrade = new GUIContent("How To Upgrade", "See docs on how to upgrade.");
		private GUIContent close = new GUIContent("Close", "Close this window.");
		private const string upgradeDocLink = "https://analytics.unity3d.com/upgrade51";
		void OnGUI ()
		{
			GUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			GUILayout.BeginVertical();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.Label ("Unity Analytics is now integrated directly inside the Unity Engine.");
			GUILayout.Label ("This requires a simple set of changes.");

			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(upgrade, GUILayout.MaxWidth(120)))
			{
				Application.OpenURL(upgradeDocLink);
				Close();
			}
			if (GUILayout.Button(close, GUILayout.MaxWidth(120)))
			{
				Close();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}
	}
}
#endif