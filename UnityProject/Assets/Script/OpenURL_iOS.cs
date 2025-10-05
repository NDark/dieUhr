using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OpenURL_iOS
{
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _OpenURL(string url);
#endif

	public static void OpenURL( string url )
	{
#if UNITY_IOS
        _OpenURL(url);
#endif
	}
}
