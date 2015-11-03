using UnityEngine;
using UnityEditor;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

[System.Serializable]
class Row  {
	public string Key;
	public string[] LocaleName;
	public string[] Values;
}

class TableJson {
	public string displayName;
	public string description;
	public Column[] columns;
	public string[][] rows;
}

class Column{
	public string displayName;
}

public class RowShareLocalization : EditorWindow
{
	private static Row[] row;
	private static SerializedObject serializedRow;
	
	private string rowShareApiUrl = "http://www.rowshare.com/api/list/export/";
	private string rowShareUrl = "http://www.rowshare.com/";

	[MenuItem("Localization/Import")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(RowShareLocalization));
	}


	void OnGUI()
	{
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		rowShareUrl = EditorGUILayout.TextField ("Rowshare url: ", rowShareUrl);

		if (GUILayout.Button ("Import")) {
			Match match = Regex.Match(rowShareUrl, "[0-9a-z]{32}");

			if (match == null || !match.Success){
				Debug.Log("Table not found !");
				return;
			}

			var tableId = match.Value;
			Debug.Log("Id: " + tableId);

			var url = rowShareApiUrl + tableId;
			Debug.Log("Download from : " + url);

			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
			WebClient client = new WebClient();
			string JsonTable = client.DownloadString(url);
			Debug.Log("Download success");

			Debug.Log("Deserialization");
			TableJson table = JsonConvert.DeserializeObject<TableJson>(JsonTable);
			Debug.Log("Deserialization success of table : " + table.displayName);

			for(int i=0; i<table.columns.Length; i++){
				Debug.Log(table.columns[i].displayName);
			}
		};
	}



	private bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for (int i=0; i<chain.ChainStatus.Length; i++) {
				if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}
}






