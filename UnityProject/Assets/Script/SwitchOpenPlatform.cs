using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOpenPlatform : MonoBehaviour 
{
	public GameObject m_GooglePlayMarket = null;
	public GameObject m_AppStore = null;

	// Use this for initialization
	void Start () 
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			m_GooglePlayMarket.SetActive(false);
			m_AppStore.SetActive(true);
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			m_GooglePlayMarket.SetActive(true);
			m_AppStore.SetActive(false);
		}
		else
		{
#if UNITY_ANDROID
			m_GooglePlayMarket.SetActive(true);
			m_AppStore.SetActive(false);
#elif UNITY_IPHONE
			m_GooglePlayMarket.SetActive(false);
			m_AppStore.SetActive(true);
#else
			m_GooglePlayMarket.SetActive(true);
			m_AppStore.SetActive(false);
#endif
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
