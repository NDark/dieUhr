using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;

public class iOSInfoPlistPostprocess
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
		if (buildTarget == BuildTarget.iOS)
		{
			// Path to the Info.plist file
			string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");

			// Read the plist file
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath); 

			// Get the root dictionary of the plist
			PlistElementDict rootDict = plist.root;

			// Add or modify properties
			// rootDict.SetString("NSLocationAlwaysUsageDescription", "Location information is used for detecting default language.");
			// rootDict.SetString("NSLocationWhenInUseUsageDescription", "Location information is used for detecting default language.");
			rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

			// Write the modified plist back to the file
			plist.WriteToFile(plistPath);
		}
	}
}
#endif