#if (UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL || UNITY_METRO) && (UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5_0 || UNITY_5_1)
#define UNITY_ANALYTICS_SUPPORTED_PLATFORM
#endif

using System;
using System.Collections.Generic;

namespace UnityEngine.Cloud.Analytics
{
	public enum Gender
	{
		Male,
		Female,
		Unknown
	}

	public enum AnalyticsResult
	{
		Ok,
		NotInitialized,
		AnalyticsDisabled,
		TooManyItems,
		SizeLimitReached,
		TooManyRequests,
		InvalidData,
		UnsupportedPlatform
	}

	public enum LogLevel
	{
		None,
		Error,
		Warning,
		Info
	}

	public static class UnityAnalytics
	{
		public static AnalyticsResult StartSDK(string appId)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.StartWithAppId(appId);
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}

		public static void SetLogLevel(LogLevel logLevel, bool enableLogging=true)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			Logger.EnableLogging = enableLogging;
			Logger.SetLogLevel((int)logLevel);
			#endif
		}
		
		public static AnalyticsResult SetUserId(string userId)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.SetUserId(userId);
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}

		public static AnalyticsResult SetUserGender(Gender gender)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.SetUserGender( gender==Gender.Male ? "M" : gender==Gender.Female ? "F" : "U" );
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}

		public static AnalyticsResult SetUserBirthYear(int birthYear)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.SetUserBirthYear(birthYear);
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.Transaction(productId, amount, currency, null, null);
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.Transaction(productId, amount, currency, receiptPurchaseData, signature);
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}

		public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
			IUnityAnalyticsSession session = UnityAnalytics.GetSingleton();
			return (AnalyticsResult)session.CustomEvent(customEventName, eventData);
			#else
			return AnalyticsResult.UnsupportedPlatform;
			#endif
		}
		#if UNITY_ANALYTICS_SUPPORTED_PLATFORM
		private static SessionImpl s_Implementation;
		private static IUnityAnalyticsSession GetSingleton()
		{
			if (s_Implementation == null) {
				Logger.loggerInstance = new UnityLogger();
				IPlatformWrapper platformWrapper = PlatformWrapper.platform;
				#if NETFX_CORE
				IFileSystem fileSystem = new WindowsFileSystem();
				#elif UNITY_WEBPLAYER || UNITY_WEBGL
				IFileSystem fileSystem = new VirtualFileSystem();
				#else
				IFileSystem fileSystem = new FileSystem();
				#endif
				ICoroutineManager coroutineManager = new UnityCoroutineManager();
				s_Implementation = new SessionImpl(platformWrapper, coroutineManager, fileSystem);
				GameObserver.CreateComponent(platformWrapper, s_Implementation);
			}
			return s_Implementation;
		}
		#endif
	}
}
