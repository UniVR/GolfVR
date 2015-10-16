#if UNITY_IPHONE && !UNITY_EDITOR && (UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5_0 || UNITY_5_1)
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;

namespace UnityEngine.Cloud.Analytics
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		[DllImport ("__Internal")]
		public static extern string UnityEngine_Cloud_GetAppVersion();
		[DllImport ("__Internal")]
		public static extern string UnityEngine_Cloud_GetBundleIdentifier();
		[DllImport ("__Internal")]
		public static extern string UnityEngine_Cloud_GetAppInstallMode();
		[DllImport ("__Internal")]
		public static extern bool UnityEngine_Cloud_IsJailbroken();

		public override string appVersion
		{
			get { 
				try{
					return UnityEngine_Cloud_GetAppVersion(); 
				}catch(System.Exception){
					return "UNKNOWN";
				}
			}
		}

		public override string appBundleIdentifier
		{
			get { 
				try{
					return UnityEngine_Cloud_GetBundleIdentifier(); 
				}catch(System.Exception){
					return "UNKNOWN";
				}
			}
		}

		public override string appInstallMode
		{
			get { 
				try{
					return UnityEngine_Cloud_GetAppInstallMode(); 
				}catch(System.Exception){
					return "UNKNOWN";
				}
			}
		}
		
		public override bool isRootedOrJailbroken
		{
			get { 
				try{
					return UnityEngine_Cloud_IsJailbroken(); 
				}catch(System.Exception){
					return false;
				}
			}
		}		
	}
}
#endif