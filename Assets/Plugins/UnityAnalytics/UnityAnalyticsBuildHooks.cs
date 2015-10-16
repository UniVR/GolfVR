#if UNITY_EDITOR && (UNITY_5_0 || UNITY_5_1) && UNITY_STANDALONE
using System;
using UnityEditor;
using System.IO;

namespace UnityEngine.Cloud.Analytics
{
	public static class UnityAnalyticsBuildHooks{
		public static void CreatePackageHook(){
			SetPluginImportSettings();
		}

		private static void SetPluginImportSettings(){
			var pluginImporters = PluginImporter.GetAllImporters ();

			foreach (var pluginImporter in pluginImporters) {
				string folderName = Path.GetFileName(Path.GetDirectoryName(pluginImporter.assetPath));
				string fileName = Path.GetFileName(pluginImporter.assetPath);

				if(folderName.Equals("Plugins"))
				{
					if(fileName.Equals("UnityEngine.Cloud.Analytics.dll")){
						pluginImporter.SetCompatibleWithAnyPlatform(false);

						foreach (BuildTarget buildTargetEnum in System.Enum.GetValues(typeof(BuildTarget)))
						{
							pluginImporter.SetCompatibleWithPlatform(buildTargetEnum, true);
						}
						pluginImporter.SetCompatibleWithEditor(true);

						pluginImporter.SetCompatibleWithPlatform(BuildTarget.WP8Player, false);
						pluginImporter.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);

						pluginImporter.SaveAndReimport();
					}else if (fileName.Equals("UnityEngine.Cloud.Analytics.Util.dll")){
						pluginImporter.SetCompatibleWithAnyPlatform(false);

						foreach (BuildTarget buildTargetEnum in System.Enum.GetValues(typeof(BuildTarget)))
						{
							pluginImporter.SetCompatibleWithPlatform(buildTargetEnum, true);
						}
						pluginImporter.SetCompatibleWithEditor(true);

						pluginImporter.SetCompatibleWithPlatform(BuildTarget.WP8Player, true);
						pluginImporter.SetCompatibleWithPlatform(BuildTarget.WSAPlayer, false);

						pluginImporter.SaveAndReimport();
					}
				}
			}

		}
	}
}
#endif