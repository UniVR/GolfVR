#if NETFX_CORE
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics.Util;
namespace UnityEngine.Cloud.Analytics
{

	public class WindowsFileSystem : IFileSystem
	{
		public virtual void DirectoryDelete(string path, Boolean recursive)
		{
			try
			{
				WindowsDirectory.Delete (path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
			}
		}

		public virtual bool DirectoryExists(string path)
		{
			try
			{
				return WindowsDirectory.Exists (path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
				return false;
			}
		}

		public virtual void CreateDirectory(string path)
		{
			try
			{
				path = path.Remove(0,Application.persistentDataPath.Length);
				WindowsDirectory.Create(path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
			}
		}

		public virtual string[] GetDirectories(string path)
		{
			try
			{
				return WindowsDirectory.GetDirectories(path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
				return null;
			}
		}

		public virtual string[] GetFiles(string path)
		{
			try
			{
				return WindowsDirectory.GetFiles(path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
				return null;
			}
		}

		public virtual bool FileExists(string path)
		{
			try
			{
				return WindowsFile.Exists(path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
				return false;
			}
		}

		public virtual void FileDelete(string path)
		{
			try
			{
				WindowsFile.Delete (path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
			}
		}

		public virtual byte[] ReadAllBytes(string path)
		{
			try
			{
				return WindowsFile.ReadAllBytes(path.Replace ('/', '\\'));
			}
			catch(Exception)
			{
				return null;
			}
		}

		public virtual string[] ReadAllLines(string path)
		{
			try
			{
				IList<string> lines = WindowsFile.ReadAllLines (path.Replace ('/', '\\'));
				int length = lines.Count;

				string[] linesArray = new string[length];
				int i = 0;
				foreach (var line in lines) {
					linesArray[i] = line;	
					i++;
				}

				return linesArray;
			}
			catch(Exception)
			{
				return null;
			}
		}

		public virtual string ReadAllText(string path)
		{
			try{
				return WindowsFile.ReadAllText(path.Replace ('/', '\\'));
			}catch(Exception){
				return null;
			}
		}

		public virtual void WriteAllBytes(string path, byte[] bytes)
		{
			try
			{
				WindowsFile.WriteAllBytes(path.Replace ('/', '\\'), bytes);
			}
			catch(Exception)
			{
			}
		}

		public virtual void WriteAllLines(string path, string[] lines)
		{
			try
			{
				WindowsFile.WriteAllLines(path.Replace ('/', '\\'), lines);
			}
			catch(Exception)
			{
			}
		}

		public virtual void WriteAllText(string path, string text)
		{
			try
			{
				WindowsFile.WriteAllText(path.Replace ('/', '\\'), text);
			}
			catch(Exception)
			{
			}
		}

		public virtual string PathCombine(params string[] paths)
		{
			if (paths.Length == 0)
			{
				return "";
			}
			string finalPath = paths[0];
			for (int i = 1; i < paths.Length; i++)
			{
				finalPath = Path.Combine(finalPath, paths[i]);
			}
			return finalPath;
		}
	}
}
#endif
