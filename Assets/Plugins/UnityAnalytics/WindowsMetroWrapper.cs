#if UNITY_METRO
using System.Text;
	#if UNITY_EDITOR
	using UnityEditor;

	[InitializeOnLoad]
	public class WindowsCapability 
	{
		static WindowsCapability()
		{
			#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
				PlayerSettings.Metro.SetCapability (PlayerSettings.MetroCapability.InternetClientServer, true);
				PlayerSettings.Metro.SetCapability (PlayerSettings.MetroCapability.InternetClient, true);
			#else
				PlayerSettings.WSA.SetCapability (PlayerSettings.WSACapability.InternetClientServer, true);
				PlayerSettings.WSA.SetCapability (PlayerSettings.WSACapability.InternetClient, true);
			#endif
		}

	}
	#endif


namespace UnityEngine.Cloud.Analytics
{
	internal class WindowsMetroWrapper : BasePlatformWrapper 
	{
		public override string Md5Hex(string input)
		{
			UTF8Encoding ue = new UTF8Encoding();
			byte[] bytes = ue.GetBytes(input);

			// encrypt bytes
			byte[] hashBytes = UnityEngine.Windows.Crypto.ComputeMD5Hash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++)
			{
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');
		}
	}
}
#endif