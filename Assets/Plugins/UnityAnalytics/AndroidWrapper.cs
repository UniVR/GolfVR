#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;

namespace UnityEngine.Cloud.Analytics
{
	internal class AndroidWrapper : BasePlatformWrapper
	{
		public override string appVersion
		{
			get {
				string appVer = null;
				using(var appUtilClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
				{
					appVer = appUtilClass.CallStatic<string>("getAppVersion");
				}
				return appVer;
			}
		}

		public override string appBundleIdentifier
		{
			get {
				string appBundleId = null;
				using(var appUtilClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
				{
					appBundleId = appUtilClass.CallStatic<string>("getAppPackageName");
				}
				return appBundleId;
			}
		}

		public override string appInstallMode
		{
			get {
				string appInstallMode = null;
				using(var appUtilClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
				{
					appInstallMode = appUtilClass.CallStatic<string>("getAppInstallMode");
				}
				return appInstallMode;
			}
		}
		
		public override bool isRootedOrJailbroken
		{
			get {
				bool isBroken = false;
				using(var appUtilClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
				{
					isBroken = appUtilClass.CallStatic<bool>("isDeviceRooted");
				}
				return isBroken;
			}
		}

        public override string deviceUniqueIdentifier
        {
            get 
            { 
                try 
                {
                    AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
                    AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
                    string ANDROID_ID = clsSecure.GetStatic<string>("ANDROID_ID");
                    string androidId = clsSecure.CallStatic<string>("getString", objResolver, ANDROID_ID);

                    return Md5Hex(androidId);
                } 
            #if UNITY_4_0 || UNITY_4_1 || UNITY_4_2
                catch (System.Exception)
                {
                    return "";
                }
            #else
                catch (UnityEngine.AndroidJavaException)
                {
                    return "";
                }
                catch (System.Exception)
                {
                    return "";
                }
            #endif
            }
        }
	}
}
#endif

